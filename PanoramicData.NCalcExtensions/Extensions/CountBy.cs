using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class CountBy
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var listObject = functionArgs.Parameters[0].Evaluate();
		var listEnumerable = listObject as IEnumerable<object?>;

		if (functionArgs.Parameters.Length == 1)
		{
			if (listObject is string text)
			{
				functionArgs.Result = text.Length;
				return;
			}

			if (listEnumerable is null)
			{
				throw new FormatException($"{ExtensionFunction.Count}() requires IEnumerable parameter.");
			}

			functionArgs.Result = listEnumerable.Count();
			return;
		}

		if (listEnumerable is null)
		{
			throw new FormatException($"{ExtensionFunction.Count}() requires IEnumerable parameter.");
		}

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.Count} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.Count} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);

		var dictionary = new Dictionary<string, int>();
		foreach (var value in listEnumerable)
		{
			if (lambda.Evaluate(value) is not string key)
			{
				throw new FormatException($"All lambdas should evaluate to a string.  \"{value}\" did not.");
			}

			if (dictionary.TryGetValue(key, out var dictionaryValue))
			{
				dictionary[key] = ++dictionaryValue;
			}
			else
			{
				dictionary[key] = 1;
			}
		}

		functionArgs.Result = JObject.FromObject(dictionary);
	}
}