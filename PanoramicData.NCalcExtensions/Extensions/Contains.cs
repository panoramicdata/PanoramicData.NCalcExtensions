namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Contains
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var haystack = (string)functionArgs.Parameters[0].Evaluate();
			var needle = (string)functionArgs.Parameters[1].Evaluate();
			functionArgs.Result = haystack.IndexOf(needle, StringComparison.InvariantCulture) >= 0;
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.Contains}() requires two string parameters.");
		}
	}
}
