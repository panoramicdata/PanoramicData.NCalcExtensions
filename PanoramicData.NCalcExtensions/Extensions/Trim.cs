namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Trim
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		try
		{
			var param1 = (string)functionArgs.Parameters[0].Evaluate();
			functionArgs.Result = param1.Trim();
		}
		catch (Exception e) when (e is not NCalcExtensionsException)
		{
			throw new FormatException($"{ExtensionFunction.Trim} function -  requires one string parameter.");
		}
	}
}
