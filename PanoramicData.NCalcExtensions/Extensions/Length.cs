namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Length
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var param1 = (string)functionArgs.Parameters[0].Evaluate();
			functionArgs.Result = param1.Length;
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Length}() requires one string parameter.");
		}
	}
}
