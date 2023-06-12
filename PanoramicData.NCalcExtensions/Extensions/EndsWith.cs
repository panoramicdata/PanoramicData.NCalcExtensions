namespace PanoramicData.NCalcExtensions.Extensions;

internal static class EndsWith
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var param1 = (string)functionArgs.Parameters[0].Evaluate();
			var param2 = (string)functionArgs.Parameters[1].Evaluate();
			functionArgs.Result = param1.EndsWith(param2, StringComparison.InvariantCulture);
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.EndsWith} function requires two string parameters.");
		}
	}
}
