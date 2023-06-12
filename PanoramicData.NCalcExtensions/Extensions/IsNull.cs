namespace PanoramicData.NCalcExtensions.Extensions;

internal static class IsNull
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsNull}() requires one parameter.");
		}

		try
		{
			var outputObject = functionArgs.Parameters[0].Evaluate();
			functionArgs.Result = outputObject is null || outputObject is JToken { Type: JTokenType.Null };
		}
		catch (Exception e)
		{
			throw new FormatException(e.Message);
		}
	}
}
