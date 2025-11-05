namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("maxValue")]
	[Description("Emits the maximum possible value for a given numeric or date/time type.")]
	object MaxValue(
		[Description("Name of the data type.")]
		string type
	);
}

internal static class MaxValue
{
	private const string ErrorMessage = $"{ExtensionFunction.MaxValue} takes exactly one string parameter, which must be one of 'sbyte', 'byte', 'short', 'ushort', 'int', 'uint', 'long', 'ulong', 'float', 'double', 'decimal', 'DateTime' or 'DateTimeOffset'.";

	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var originalList = functionArgs.Parameters[0].Evaluate();

		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException(ErrorMessage);
		}

		functionArgs.Result = originalList switch
		{
			"sbyte" => sbyte.MaxValue,
			"byte" => byte.MaxValue,
			"short" => short.MaxValue,
			"ushort" => ushort.MaxValue,
			"int" => int.MaxValue,
			"uint" => uint.MaxValue,
			"long" => long.MaxValue,
			"ulong" => ulong.MaxValue,
			"float" => float.MaxValue,
			"double" => double.MaxValue,
			"decimal" => decimal.MaxValue,
			"DateTime" => DateTime.MaxValue,
			"DateTimeOffset" => DateTimeOffset.MaxValue,
			_ => throw new FormatException(ErrorMessage)
		};

		return;
	}
}
