using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("jsonArray")]
	[Description("Creates and returns a new JsonDocument array with the given items.")]
	JsonDocument NewJsonArray(
		[Description("Items to include in the array.")]
		params object[] items
	);
}

internal static class NewJsonArray
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var parameterIndex = 0;

		// Create a list to hold the items
		var items = new List<object?>();
		while (parameterIndex < functionArgs.Parameters.Length)
		{
			var item = functionArgs.Parameters[parameterIndex++].Evaluate();
			items.Add(item);
		}

		// Serialize the list to create a JsonDocument array
		var jsonDocument = JsonSerializer.SerializeToDocument(items);
		functionArgs.Result = jsonDocument;
	}
}