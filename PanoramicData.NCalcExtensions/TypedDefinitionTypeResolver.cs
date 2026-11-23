namespace PanoramicData.NCalcExtensions;

internal readonly record struct ResolvedTypedDefinitionType(Type UnderlyingType, bool IsNullable, bool RequiresJsonValue)
{
	internal ResolvedTypedDefinitionType(Type underlyingType, bool isNullable)
		: this(underlyingType, isNullable, false)
	{
	}

	internal bool AllowsNull => IsNullable || !UnderlyingType.IsValueType;

	/// <summary>
	/// The type as it appears as a collection element, so that <c>int?</c> becomes <see cref="Nullable{T}"/>.
	/// </summary>
	internal Type ElementType => IsNullable && UnderlyingType.IsValueType
		? typeof(Nullable<>).MakeGenericType(UnderlyingType)
		: UnderlyingType;
}

internal static class TypedDefinitionTypeResolver
{
	private static readonly Dictionary<string, Type> TypeAliases = new(StringComparer.OrdinalIgnoreCase)
	{
		["string"] = typeof(string),
		["bool"] = typeof(bool),
		["byte"] = typeof(byte),
		["sbyte"] = typeof(sbyte),
		["short"] = typeof(short),
		["ushort"] = typeof(ushort),
		["int"] = typeof(int),
		["uint"] = typeof(uint),
		["long"] = typeof(long),
		["ulong"] = typeof(ulong),
		["float"] = typeof(float),
		["double"] = typeof(double),
		["decimal"] = typeof(decimal),
		["datetime"] = typeof(DateTime),
		["guid"] = typeof(Guid),
		["datetimeoffset"] = typeof(DateTimeOffset),
		["object"] = typeof(object),
		["type"] = typeof(Type),
		["exception"] = typeof(Exception),
	};

	/// <summary>
	/// The structured types, whose values are written as JSON.
	/// </summary>
	private static readonly Dictionary<string, Type> JsonTypeAliases = new(StringComparer.OrdinalIgnoreCase)
	{
		["jobject"] = typeof(JObject),
		["jarray"] = typeof(JArray),
		["jsondocument"] = typeof(JsonDocument),
	};

	internal static bool TryResolve(string typeName, out ResolvedTypedDefinitionType resolvedType)
	{
		resolvedType = default;

		if (string.IsNullOrWhiteSpace(typeName))
		{
			return false;
		}

		var trimmedTypeName = typeName.Trim();
		var isNullable = trimmedTypeName.EndsWith('?');
		var baseTypeName = isNullable
			? trimmedTypeName[..^1].TrimEnd()
			: trimmedTypeName;

		if (string.IsNullOrWhiteSpace(baseTypeName))
		{
			return false;
		}

		if (TypeAliases.TryGetValue(baseTypeName, out var aliasedType))
		{
			resolvedType = new ResolvedTypedDefinitionType(aliasedType, isNullable);
			return true;
		}

		if (JsonTypeAliases.TryGetValue(baseTypeName, out var jsonType))
		{
			resolvedType = new ResolvedTypedDefinitionType(jsonType, isNullable, RequiresJsonValue: true);
			return true;
		}

		if (TryResolveGeneric(baseTypeName, out var genericType))
		{
			resolvedType = new ResolvedTypedDefinitionType(genericType, isNullable, RequiresJsonValue: true);
			return true;
		}

		var runtimeType = Type.GetType(baseTypeName);
		if (runtimeType is null || !IsSupportedType(runtimeType))
		{
			return false;
		}

		resolvedType = new ResolvedTypedDefinitionType(runtimeType, isNullable);
		return true;
	}

	/// <summary>
	/// Whether <paramref name="value"/> is acceptable for the resolved type: JSON for the structured types and an
	/// ISO 8601 offset timestamp for <see cref="DateTimeOffset"/>. Scalars are not checked here.
	/// </summary>
	internal static bool IsValidValue(ResolvedTypedDefinitionType resolvedType, string value)
	{
		if (resolvedType.RequiresJsonValue)
		{
			try
			{
				using var document = JsonDocument.Parse(value);
				return true;
			}
			catch (JsonException)
			{
				return false;
			}
		}

		return resolvedType.UnderlyingType != typeof(DateTimeOffset)
			|| DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
	}

	private static bool TryResolveGeneric(string typeName, out Type genericType)
	{
		genericType = null!;

		var open = typeName.IndexOf('<');
		if (open <= 0 || !typeName.EndsWith('>'))
		{
			return false;
		}

		var genericName = typeName[..open].Trim();
		var arguments = SplitTypeArguments(typeName[(open + 1)..^1]);

		if (genericName.Equals("list", StringComparison.OrdinalIgnoreCase) && arguments.Count == 1)
		{
			if (!TryResolve(arguments[0], out var element))
			{
				return false;
			}

			genericType = typeof(List<>).MakeGenericType(element.ElementType);
			return true;
		}

		if (genericName.Equals("dictionary", StringComparison.OrdinalIgnoreCase)
			&& arguments.Count == 2
			&& arguments[0].Equals("string", StringComparison.OrdinalIgnoreCase)
			&& TryResolve(arguments[1], out var value))
		{
			genericType = typeof(Dictionary<,>).MakeGenericType(typeof(string), value.ElementType);
			return true;
		}

		return false;
	}

	/// <summary>
	/// Splits generic type arguments on the commas that are not inside nested angle brackets.
	/// </summary>
	private static List<string> SplitTypeArguments(string arguments)
	{
		List<string> parts = [];
		var depth = 0;
		var start = 0;
		for (var i = 0; i < arguments.Length; i++)
		{
			if (arguments[i] == '<')
			{
				depth++;
			}
			else if (arguments[i] == '>')
			{
				depth--;
			}
			else if (arguments[i] == ',' && depth == 0)
			{
				parts.Add(arguments[start..i].Trim());
				start = i + 1;
			}
		}

		parts.Add(arguments[start..].Trim());
		return parts;
	}

	/// <summary>
	/// The types a typed definition may declare.
	/// </summary>
	private static readonly FrozenSet<Type> SupportedTypes = new[]
	{
		typeof(string), typeof(bool), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort),
		typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double),
		typeof(decimal), typeof(DateTime), typeof(Guid),
	}.ToFrozenSet();

	private static bool IsSupportedType(Type type) => SupportedTypes.Contains(type);
}
