using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Store
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		string key;
		object? value;
		try
		{
			key = (string)functionArgs.Parameters[0].Evaluate();
			value = functionArgs.Parameters[1].Evaluate();
		}
		catch (Exception e) when (e is not NCalcExtensionsException)
		{
			throw new FormatException($"{ExtensionFunction.Store}() requires two parameters.");
		}

		var storageDictionary = functionArgs.Parameters[0].Parameters[ExtendedExpression.StorageDictionaryParameterName] as Dictionary<string, object?>
			?? throw new FormatException($"{ExtensionFunction.Retrieve}() requires a storage dictionary.");

		storageDictionary[key] = value;

		functionArgs.Result = true;
	}
}
