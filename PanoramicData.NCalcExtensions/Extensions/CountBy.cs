namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("countBy")]
	[Description("Counts the number of items, grouped by a calculation.")]
	JObject CountBy(
		[Description("The original list")]
		IEnumerable<object?> list,
		[Description("A string to represent the value to be evaluated")]
		string? predicate = null,
		[Description("The string to evaluate. Must emit a string containing one or more characters: A-Z, a-z, 0-9 or _.")]
		string? exprStr = null
	);
}

internal static class CountBy
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var listObject = functionArgs.Parameters[0].Evaluate();

		// Convert typed collections (like List<int>) to IEnumerable<object?>
		var listEnumerable = listObject switch
		{
			IEnumerable<object?> enumerable => enumerable,
			System.Collections.IEnumerable nonGenericEnumerable => nonGenericEnumerable.Cast<object?>(),
			_ => null
		} ?? throw new FormatException($"{ExtensionFunction.Count}() requires IEnumerable parameter.");

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

			dictionary[key] = dictionary.TryGetValue(key, out var dictionaryValue) ? ++dictionaryValue : 1;
		}

		functionArgs.Result = JObject.FromObject(dictionary);
	}
}