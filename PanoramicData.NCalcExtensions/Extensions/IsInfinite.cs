namespace PanoramicData.NCalcExtensions.Extensions;

internal static class IsInfinite
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsInfinite}() requires one parameter.");
		}

		try
		{
			var outputObject = functionArgs.Parameters[0].Evaluate();
			functionArgs.Result =
				outputObject is double x && (
					double.IsPositiveInfinity(x)
					|| double.IsNegativeInfinity(x)
				);
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException(e.Message);
		}
	}
}
