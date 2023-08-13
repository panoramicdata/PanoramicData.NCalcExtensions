using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Retrieve
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		string key;
		try
		{
			key = (string)functionArgs.Parameters[0].Evaluate();
		}
		catch (Exception e) when (e is not NCalcExtensionsException)
		{
			throw new FormatException($"{ExtensionFunction.Retrieve}() requires one string parameter.");
		}

		var storageDictionary = functionArgs.Parameters[0].Parameters[ExtendedExpression.StorageDictionaryParameterName] as Dictionary<string, object?>
			?? throw new FormatException($"{ExtensionFunction.Retrieve}() requires a storage dictionary.");

		functionArgs.Result = storageDictionary.TryGetValue(key, out var value)
			? value
			: throw new FormatException("Key not found");
	}
}