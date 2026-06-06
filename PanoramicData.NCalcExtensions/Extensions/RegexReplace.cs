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
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var input = functionArgs.Parameters[0].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.RegexReplace}() requires a string input, a string pattern, and a string replacement.");

			var pattern = functionArgs.Parameters[1].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.RegexReplace}() requires a string input, a string pattern, and a string replacement.");

			var replacement = functionArgs.Parameters[2].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.RegexReplace}() requires a string input, a string pattern, and a string replacement.");

			functionArgs.Result = System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"{ExtensionFunction.RegexReplace}() requires a string input, a string pattern, and a string replacement.");
		}
	}
}
