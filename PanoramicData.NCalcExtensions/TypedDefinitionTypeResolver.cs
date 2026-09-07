namespace PanoramicData.NCalcExtensions;

internal readonly record struct ResolvedTypedDefinitionType(Type UnderlyingType, bool IsNullable)
{
	internal bool AllowsNull => IsNullable || !UnderlyingType.IsValueType;
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
		["guid"] = typeof(Guid)
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

		var runtimeType = Type.GetType(baseTypeName);
		if (runtimeType is null || !IsSupportedType(runtimeType))
		{
			return false;
		}

		resolvedType = new ResolvedTypedDefinitionType(runtimeType, isNullable);
		return true;
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