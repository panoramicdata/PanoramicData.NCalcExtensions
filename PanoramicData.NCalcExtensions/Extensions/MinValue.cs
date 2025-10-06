﻿namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("minValue")]
	[Description("Emits the minimum possible value for a given numeric type.")]
	object MinValue(
		[Description("Name of the data type.")]
		string type
	);
}

internal static class MinValue
{
	private const string ErrorMessage = $"{ExtensionFunction.MinValue} takes exactly one string parameter, which must be one of 'sbyte', 'byte', 'short', 'ushort', 'int', 'uint', 'long', 'ulong', 'float', 'double' or 'decimal'.";

	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		var originalList = functionArgs.Parameters[0].Evaluate();

		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException(ErrorMessage);
		}

		functionArgs.Result = originalList switch
		{
			"sbyte" => sbyte.MinValue,
			"byte" => byte.MinValue,
			"short" => short.MinValue,
			"ushort" => ushort.MinValue,
			"int" => int.MinValue,
			"uint" => uint.MinValue,
			"long" => long.MinValue,
			"ulong" => ulong.MinValue,
			"float" => float.MinValue,
			"double" => double.MinValue,
			"decimal" => decimal.MinValue,
			_ => throw new FormatException(ErrorMessage)
		};

		return;
	}
}
