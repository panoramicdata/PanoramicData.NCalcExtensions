namespace PanoramicData.NCalcExtensions.Extensions;

internal static class ConvertFunction
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 2)
		{
			throw new FormatException($"{ExtensionFunction.Convert}() requires two parameters.");
		}

		try
		{
			// Feed the result of the first parameter into the variables available to the second parameter
			var param1 = functionArgs.Parameters[0].Evaluate();
			functionArgs.Parameters[1].Parameters["value"] = param1;
			functionArgs.Result = functionArgs.Parameters[1].Evaluate();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}
}
