namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("isInfinite")]
	[Description("Determines whether a value is infinite.")]
	bool IsInfinite(
		[Description("The value to be tested.")]
		object? value
	);
}

internal static class IsInfinite
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		if (functionArgs.Parameters.Count != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsInfinite}() requires one parameter.");
		}

		try
		{
			var outputObject = functionArgs.Parameters.Evaluate(0);
			functionArgs.Result =
				outputObject is double x && (
					double.IsPositiveInfinity(x)
					|| double.IsNegativeInfinity(x)
				);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException(e.Message);
		}
	}
}
