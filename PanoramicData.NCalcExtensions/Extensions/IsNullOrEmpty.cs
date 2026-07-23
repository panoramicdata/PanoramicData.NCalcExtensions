namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("isNullOrEmpty")]
	[Description("Determines whether a value is null or empty.")]
	bool IsNullOrEmpty(
		[Description("The value to be tested.")]
		object? value
	);
}

internal static class IsNullOrEmpty
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		if (functionArgs.Parameters.Count != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsNullOrEmpty}() requires one parameter.");
		}

		try
		{
			var outputObject = functionArgs.Parameters.Evaluate(0);
			functionArgs.Result = outputObject is null ||
				outputObject is JToken { Type: JTokenType.Null } ||
				outputObject is JsonElement { ValueKind: JsonValueKind.Null } ||
				(outputObject is string outputString && outputString == string.Empty) ||
				(outputObject is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.String && jsonElement.GetString() == string.Empty);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException(e.Message);
		}
	}
}
