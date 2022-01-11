namespace PanoramicData.NCalcExtensions.Extensions;

internal static class StartsWith
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		string param1;
		string param2;
		if (functionArgs.Parameters.Length != 2)
		{
			throw new FormatException($"{ExtensionFunction.StartsWith}() requires two parameters.");
		}

		try
		{
			param1 = (string)functionArgs.Parameters[0].Evaluate() as string;
			param2 = (string)functionArgs.Parameters[1].Evaluate() as string;
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception e)
		{
			throw new FormatException($"Unexpected exception in {ExtensionFunction.StartsWith}(): {e.Message}", e);
		}

		if (param1 == null)
		{
			throw new FormatException($"{ExtensionFunction.StartsWith}() parameter 1 is not a string");
		}

		if (param2 == null)
		{
			throw new FormatException($"{ExtensionFunction.StartsWith}() parameter 2 is not a string");
		}

		functionArgs.Result = param1.StartsWith(param2, StringComparison.InvariantCulture);
	}
}
