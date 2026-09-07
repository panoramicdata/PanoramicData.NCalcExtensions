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
			functionArgs.Result = IsMatch(functionArgs.Parameters.Evaluate(0));
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException(e.Message, e);
		}
	}

	/// <summary>
	/// Whether <paramref name="value"/> is null, a JSON null, or an empty string, in any of the
	/// representations the expression language can produce.
	/// </summary>
	private static bool IsMatch(object? value) => value switch
	{
		null => true,
		JToken { Type: JTokenType.Null } => true,
		JsonElement { ValueKind: JsonValueKind.Null } => true,
		string text => text.Length == 0,
		JsonElement { ValueKind: JsonValueKind.String } element => element.GetString()?.Length == 0,
		_ => false,
	};
}
