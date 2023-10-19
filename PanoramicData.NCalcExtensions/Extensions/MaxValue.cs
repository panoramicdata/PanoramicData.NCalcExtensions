namespace PanoramicData.NCalcExtensions.Extensions;

internal static class MaxValue
{
	private const string ErrorMessage = $"{ExtensionFunction.MaxValue} takes exactly one string parameter, which must be one of 'sbyte', 'byte', 'short', 'ushort', 'int', 'uint', 'long', 'ulong', 'float', 'double' or 'decimal'.";

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
			_ => throw new FormatException(ErrorMessage)
		};

		return;
	}
}
