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

	private static bool IsSupportedType(Type type)
	{
		return type == typeof(string)
			|| type == typeof(bool)
			|| type == typeof(byte)
			|| type == typeof(sbyte)
			|| type == typeof(short)
			|| type == typeof(ushort)
			|| type == typeof(int)
			|| type == typeof(uint)
			|| type == typeof(long)
			|| type == typeof(ulong)
			|| type == typeof(float)
			|| type == typeof(double)
			|| type == typeof(decimal)
			|| type == typeof(DateTime)
			|| type == typeof(Guid);
	}
}