namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Contains
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var param1 = (string)functionArgs.Parameters[0].Evaluate();
			var param2 = (string)functionArgs.Parameters[1].Evaluate();
			functionArgs.Result = param1.IndexOf(param2, StringComparison.InvariantCulture) >= 0;
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Contains}() requires two string parameters.");
		}
	}
}
