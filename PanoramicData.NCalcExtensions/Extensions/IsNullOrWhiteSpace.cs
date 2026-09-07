namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("IsNullOrWhiteSpace")]
	[Description("Determines whether a value is null, empty or whitespace.")]
	bool IsNullOrWhiteSpace(
		[Description("The value to be tested.")]
		object? value
	);
}

internal static class IsNullOrWhiteSpace
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		if (functionArgs.Parameters.Count != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsNullOrWhiteSpace}() requires one parameter.");
		}

		try
		{
			functionArgs.Result = IsMatch(functionArgs.Parameters.Evaluate(0));
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException(e.Message, e);
		}
	}

	/// <summary>
	/// Whether <paramref name="value"/> is null, a JSON null, or a string that is empty or all white space, in any of the
	/// representations the expression language can produce.
	/// </summary>
	private static bool IsMatch(object? value) => value switch
	{
		null => true,
		JToken { Type: JTokenType.Null } => true,
		JsonElement { ValueKind: JsonValueKind.Null } => true,
		string text => string.IsNullOrWhiteSpace(text),
		JsonElement { ValueKind: JsonValueKind.String } element => string.IsNullOrWhiteSpace(element.GetString()),
		_ => false,
	};
}
