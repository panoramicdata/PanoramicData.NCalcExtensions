namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Trim
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		try
		{
			var param1 = functionArgs.Parameters.Evaluate(0) as string
				?? throw new FormatException($"{ExtensionFunction.Trim} function - requires one string parameter.");

			functionArgs.Result = param1.Trim();
		}
		catch (Exception e) when (e is not NCalcExtensionsException)
		{
			throw new FormatException($"{ExtensionFunction.Trim} function - requires one string parameter.");
		}
	}
}
