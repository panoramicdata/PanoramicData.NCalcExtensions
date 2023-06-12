namespace PanoramicData.NCalcExtensions.Extensions;

internal static class IsNaN
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsNaN}() requires one parameter.");
		}

		try
		{
			var outputObject = functionArgs.Parameters[0].Evaluate();
			functionArgs.Result = outputObject is not double || double.IsNaN((double)outputObject);
			return;
		}
		catch (Exception e)
		{
			throw new FormatException(e.Message);
		}
	}
}
