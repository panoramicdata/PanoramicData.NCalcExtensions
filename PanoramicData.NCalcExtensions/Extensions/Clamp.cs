namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("clamp")]
	[Description("Clamps a numeric value between a minimum and maximum.")]
	double Clamp(
		[Description("The value to clamp.")]
		double value,
		[Description("The minimum allowed value.")]
		double min,
		[Description("The maximum allowed value.")]
		double max
	);
}

internal static class ClampFunction
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		try
		{
			var rawValue = functionArgs.Parameters.Evaluate(0)
				?? throw new FormatException($"{ExtensionFunction.Clamp}() requires a numeric value, a numeric min, and a numeric max.");
			var rawMin = functionArgs.Parameters.Evaluate(1)
				?? throw new FormatException($"{ExtensionFunction.Clamp}() requires a numeric value, a numeric min, and a numeric max.");
			var rawMax = functionArgs.Parameters.Evaluate(2)
				?? throw new FormatException($"{ExtensionFunction.Clamp}() requires a numeric value, a numeric min, and a numeric max.");

			var value = Convert.ToDouble(rawValue, CultureInfo.InvariantCulture);
			var min = Convert.ToDouble(rawMin, CultureInfo.InvariantCulture);
			var max = Convert.ToDouble(rawMax, CultureInfo.InvariantCulture);

			if (min > max)
			{
				throw new NCalcExtensionsException($"{ExtensionFunction.Clamp}() requires that min <= max.");
			}

			functionArgs.Result = Math.Clamp(value, min, max);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"{ExtensionFunction.Clamp}() requires a numeric value, a numeric min, and a numeric max.");
		}
	}
}
