namespace PanoramicData.NCalcExtensions.Extensions;

internal static class ToUpper
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		string param1;
		try
		{
			param1 = (string)functionArgs.Parameters[0].Evaluate();
			functionArgs.Result = param1.ToUpperInvariant();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.ToUpper} function -  requires one string parameter.");
		}
		return;
	}
}
