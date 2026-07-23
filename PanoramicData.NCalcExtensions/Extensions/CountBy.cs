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
		string? exprStr = null,
		[Description("Optional output format: 'JArray' returns [{name, count}] array; 'JObject' (default) returns flat {key: count} object.")]
		string? outputFormat = null
	);
}

internal static class CountBy
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		var listObject = functionArgs.Parameters.Evaluate(0);

		// Convert typed collections (like List<int>) to IEnumerable<object?>
		var listEnumerable = listObject switch
		{
			IEnumerable<object?> enumerable => enumerable,
			System.Collections.IEnumerable nonGenericEnumerable => nonGenericEnumerable.Cast<object?>(),
			_ => null
		} ?? throw new FormatException($"{ExtensionFunction.Count}() requires IEnumerable parameter.");

		var predicate = functionArgs.Parameters.Evaluate(1) as string
			?? throw new FormatException($"Second {ExtensionFunction.Count} parameter must be a string.");

		var lambdaString = functionArgs.Parameters.Evaluate(2) as string
			?? throw new FormatException($"Third {ExtensionFunction.Count} parameter must be a string.");

		var outputFormat = functionArgs.Parameters.Count > 3
			? functionArgs.Parameters.Evaluate(3) as string
			: null;

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Context.StaticParameters);

		var dictionary = new Dictionary<string, int>();
		foreach (var value in listEnumerable)
		{
			if (lambda.Evaluate(value) is not string key)
			{
				throw new FormatException($"All lambdas should evaluate to a string.  \"{value}\" did not.");
			}

			dictionary[key] = dictionary.TryGetValue(key, out var dictionaryValue) ? ++dictionaryValue : 1;
		}

		if (string.Equals(outputFormat, "JArray", StringComparison.OrdinalIgnoreCase))
		{
			var array = new JArray();
			foreach (var kvp in dictionary)
			{
				array.Add(new JObject { ["name"] = kvp.Key, ["count"] = kvp.Value });
			}

			functionArgs.Result = array;
		}
		else
		{
			functionArgs.Result = JObject.FromObject(dictionary);
		}
	}
}