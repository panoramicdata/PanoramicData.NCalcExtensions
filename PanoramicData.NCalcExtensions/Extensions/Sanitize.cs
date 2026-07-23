using System.Text;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("sanitize")]
	[Description("Sanitize a string, replacing any characters outside of the allowed set.")]
	string Sanitize(
		[Description("The string to be sanitized.")]
		string input,
		[Description("All of the characters that are allowed.")]
		string allowedChars,
		[Description("(Optional) The characters to insert in place of any that are not allowed (defaults : empty string).")]
		string replacementChars = ""
	);
}

internal static class Sanitize
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		var replacementCharacters = string.Empty;

		if (functionArgs.Parameters.Count < 2)
		{
			throw new FormatException($"{ExtensionFunction.Sanitize}() requires at least two parameters.");
		}

		var inputObject = functionArgs.Parameters.Evaluate(0);
		if (inputObject is null)
		{
			functionArgs.Result = null;
			return;
		}

		if (inputObject is not string inputString)
		{
			throw new FormatException($"{ExtensionFunction.Sanitize}() first parameter must be a string.");
		}

		var allowedCharactersObject = functionArgs.Parameters.Evaluate(1);
		if (allowedCharactersObject is not string allowedCharacters)
		{
			throw new FormatException($"{ExtensionFunction.Sanitize}() second parameter must be a string.");
		}

		if (functionArgs.Parameters.Count > 2)
		{
			var replacementCharactersObject = functionArgs.Parameters.Evaluate(2);
			if (replacementCharactersObject is not string replacementCharactersString)
			{
				throw new FormatException($"{ExtensionFunction.Sanitize}() third parameter must be a string.");
			}

			replacementCharacters = replacementCharactersString;
		}

		functionArgs.Result = Sanitise(inputString, allowedCharacters, replacementCharacters);
	}

	private static string Sanitise(string inputString, string allowedCharacters, string replacementCharacters)
	{
		var builder = new StringBuilder(inputString.Length);

		foreach (var character in inputString)
		{
			builder.Append(allowedCharacters.Contains(character) ? character : replacementCharacters);
		}

		return builder.ToString();
	}
}
