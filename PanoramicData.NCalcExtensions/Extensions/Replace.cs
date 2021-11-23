namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Replace
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var haystack = (string)functionArgs.Parameters[0].Evaluate();
			var needle = (string)functionArgs.Parameters[1].Evaluate();
			var newNeedle = (string)functionArgs.Parameters[2].Evaluate();
			functionArgs.Result = haystack.Replace(needle, newNeedle);
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Replace}() requires three string parameters.");
		}
	}
}
