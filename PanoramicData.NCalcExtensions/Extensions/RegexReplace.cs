namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("regexReplace")]
	[Description("Replaces occurrences of a regex pattern in a string with a replacement string.")]
	string RegexReplace(
		[Description("The input string.")]
		string input,
		[Description("The regular expression pattern to match.")]
		string pattern,
		[Description("The replacement string.")]
		string replacement
	);
}

internal static class RegexReplaceFunction
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		try
		{
			var input = functionArgs.Parameters.Evaluate(0) as string
				?? throw new FormatException($"{ExtensionFunction.RegexReplace}() requires a string input, a string pattern, and a string replacement.");

			var pattern = functionArgs.Parameters.Evaluate(1) as string
				?? throw new FormatException($"{ExtensionFunction.RegexReplace}() requires a string input, a string pattern, and a string replacement.");

			var replacement = functionArgs.Parameters.Evaluate(2) as string
				?? throw new FormatException($"{ExtensionFunction.RegexReplace}() requires a string input, a string pattern, and a string replacement.");

			functionArgs.Result = System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"{ExtensionFunction.RegexReplace}() requires a string input, a string pattern, and a string replacement.");
		}
	}
}
