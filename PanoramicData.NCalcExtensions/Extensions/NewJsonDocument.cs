using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("jsonDocument")]
	[Description("Creates and returns a new JsonDocument instance with the given properties.")]
	JsonDocument NewJsonDocument(
		[Description("Interlaced keys and values. You must provide an even number of parameters, and names must evaluate to strings.")]
		params object[] props
	);
}

internal static class NewJsonDocument
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length % 2 != 0)
		{
			throw new FormatException($"{ExtensionFunction.NewJsonDocument}() requires an even number of parameters.");
		}

		var parameterIndex = 0;

		// Create a dictionary to hold the key-value pairs
		var properties = new Dictionary<string, object?>();
		while (parameterIndex < functionArgs.Parameters.Length)
		{
			if (functionArgs.Parameters[parameterIndex++].Evaluate() is not string key)
			{
				throw new FormatException($"{ExtensionFunction.NewJsonDocument}() requires a string key.");
			}

			if (properties.ContainsKey(key))
			{
				throw new FormatException($"{ExtensionFunction.NewJsonDocument}() can only define property {key} once.");
			}

			var value = functionArgs.Parameters[parameterIndex++].Evaluate();
			properties.Add(key, value);
		}

		// Serialize the dictionary to create a JsonDocument
		var jsonDocument = JsonSerializer.SerializeToDocument(properties);
		functionArgs.Result = jsonDocument;
	}
}