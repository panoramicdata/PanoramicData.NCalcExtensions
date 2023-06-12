namespace PanoramicData.NCalcExtensions.Extensions;

internal static class IsNullOrWhiteSpace
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsNullOrWhiteSpace}() requires one parameter.");
		}

		try
		{
			var outputObject = functionArgs.Parameters[0].Evaluate();
			functionArgs.Result = outputObject is null ||
				outputObject is JToken { Type: JTokenType.Null } ||
				(outputObject is string outputString && string.IsNullOrWhiteSpace(outputString));
		}
		catch (Exception e)
		{
			throw new FormatException(e.Message);
		}
	}
}
