namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// The shared implementation of isNullOrEmpty() and isNullOrWhiteSpace(), which differ only in
/// whether a string of white space counts as blank.
/// </summary>
internal static class NullOrBlank
{
	internal static void Evaluate(FunctionEventArgs functionArgs, string function, bool whiteSpaceIsBlank)
	{
		if (functionArgs.Parameters.Count != 1)
		{
			throw new FormatException($"{function}() requires one parameter.");
		}

		try
		{
			functionArgs.Result = IsMatch(functionArgs.Parameters.Evaluate(0), whiteSpaceIsBlank);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException(e.Message, e);
		}
	}

	/// <summary>
	/// Whether <paramref name="value"/> is null, a JSON null, or a blank string, in any of the
	/// representations the expression language can produce.
	/// </summary>
	private static bool IsMatch(object? value, bool whiteSpaceIsBlank) => value switch
	{
		null => true,
		JToken { Type: JTokenType.Null } => true,
		JsonElement { ValueKind: JsonValueKind.Null } => true,
		string text => IsBlank(text, whiteSpaceIsBlank),
		JsonElement { ValueKind: JsonValueKind.String } element => IsBlank(element.GetString(), whiteSpaceIsBlank),
		_ => false,
	};

	private static bool IsBlank(string? text, bool whiteSpaceIsBlank)
		=> whiteSpaceIsBlank ? string.IsNullOrWhiteSpace(text) : text?.Length == 0;
}
