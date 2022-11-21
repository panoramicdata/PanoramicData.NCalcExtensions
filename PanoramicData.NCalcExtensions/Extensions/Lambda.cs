using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class LambdaFunction
{
	internal static void Evaluate(FunctionArgs functionArgs, Dictionary<string, object?> storageDictionary)
	{
		string predicate;
		string nCalcString;
		try
		{
			predicate = (string)functionArgs.Parameters[0].Evaluate();
			nCalcString = (string)functionArgs.Parameters[1].Evaluate();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Store}() requires two parameters.");
		}

		functionArgs.Result = new Lambda(predicate, nCalcString, storageDictionary);
	}
}
