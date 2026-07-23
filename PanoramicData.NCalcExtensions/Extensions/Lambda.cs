namespace PanoramicData.NCalcExtensions.Extensions;

internal static class LambdaFunction
{
	internal static void Evaluate(FunctionEventArgs functionArgs, Dictionary<string, object?> storageDictionary)
	{
		try
		{
			var predicate = functionArgs.Parameters.Evaluate(0) as string
				?? throw new FormatException($"Lambda function requires two string parameters.");
			var nCalcString = functionArgs.Parameters.Evaluate(1) as string
				?? throw new FormatException($"Lambda function requires two string parameters.");

			functionArgs.Result = new Lambda(predicate, nCalcString, storageDictionary);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"Lambda function requires two parameters.");
		}
	}
}
