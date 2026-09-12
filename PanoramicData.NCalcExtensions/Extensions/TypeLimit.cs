namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// The shared implementation of maxValue() and minValue(), which differ only in which end of
/// each type's range they report.
/// </summary>
internal static class TypeLimit
{
	private static readonly (string Name, object Max, object Min)[] Limits =
	[
		("sbyte", sbyte.MaxValue, sbyte.MinValue),
		("byte", byte.MaxValue, byte.MinValue),
		("short", short.MaxValue, short.MinValue),
		("ushort", ushort.MaxValue, ushort.MinValue),
		("int", int.MaxValue, int.MinValue),
		("uint", uint.MaxValue, uint.MinValue),
		("long", long.MaxValue, long.MinValue),
		("ulong", ulong.MaxValue, ulong.MinValue),
		("float", float.MaxValue, float.MinValue),
		("double", double.MaxValue, double.MinValue),
		("decimal", decimal.MaxValue, decimal.MinValue),
		("DateTime", DateTime.MaxValue, DateTime.MinValue),
		("DateTimeOffset", DateTimeOffset.MaxValue, DateTimeOffset.MinValue)
	];

	private static readonly FrozenDictionary<string, (object Max, object Min)> LimitsByName =
		Limits.ToFrozenDictionary(limit => limit.Name, limit => (limit.Max, limit.Min));

	/// <summary>
	/// The supported type names, as they appear in the failure message: "'sbyte', 'byte', ... or
	/// 'DateTimeOffset'".
	/// </summary>
	private static readonly string TypeNames = string.Join(
		" or ",
		string.Join(", ", Limits.Take(Limits.Length - 1).Select(limit => $"'{limit.Name}'")),
		$"'{Limits[^1].Name}'");

	internal static void Evaluate(FunctionEventArgs functionArgs, string function, bool isMax)
	{
		if (functionArgs.Parameters.Count != 1
			|| functionArgs.Parameters.Evaluate(0) is not string name
			|| !LimitsByName.TryGetValue(name, out var limit))
		{
			throw new FormatException($"{function} takes exactly one string parameter, which must be one of {TypeNames}.");
		}

		functionArgs.Result = isMax ? limit.Max : limit.Min;
	}
}
