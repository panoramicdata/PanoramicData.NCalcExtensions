namespace PanoramicData.NCalcExtensions.Extensions;

internal static class ToLower
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		string param1;
		try
		{
			param1 = (string)functionArgs.Parameters[0].Evaluate();
			functionArgs.Result = param1.ToLowerInvariant();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.ToLower} function -  requires one string parameter.");
		}
	}
}
