namespace PanoramicData.NCalcExtensions.Extensions;

internal static class LambdaFunction
{
	internal static void Evaluate(FunctionArgs functionArgs, Dictionary<string, object?> storageDictionary)
	{
		try
		{
			var predicate = functionArgs.Parameters[0].Evaluate() as string
				?? throw new FormatException($"Lambda function requires two string parameters.");
			var nCalcString = functionArgs.Parameters[1].Evaluate() as string
				?? throw new FormatException($"Lambda function requires two string parameters.");

			functionArgs.Result = new Lambda(predicate, nCalcString, storageDictionary);
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"Lambda function requires two parameters.");
		}
	}
}
