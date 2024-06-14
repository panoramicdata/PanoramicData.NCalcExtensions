using System.Text;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Sanitize
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		string inputString;
		string allowedCharacters;
		string replacementCharacters = string.Empty;

		if (functionArgs.Parameters.Length < 2)
		{
			throw new FormatException($"{ExtensionFunction.Sanitize}() requires at least two parameters.");
		}

		var inputObject = functionArgs.Parameters[0].Evaluate();
		if (inputObject is null)
		{
			functionArgs.Result = null;
			return;
		}

		if (inputObject is not string)
		{
			throw new FormatException($"{ExtensionFunction.Sanitize}() first parameter must be a string.");
		}
		inputString = inputObject.ToString();

		var allowedCharactersObject = functionArgs.Parameters[1].Evaluate();
		if (allowedCharactersObject is not string)
		{
			throw new FormatException($"{ExtensionFunction.Sanitize}() second parameter must be a string.");
		}
		allowedCharacters = allowedCharactersObject.ToString();

		if (functionArgs.Parameters.Length > 2)
		{
			var replacementCharactersObject = functionArgs.Parameters[2].Evaluate();
			if (replacementCharactersObject is not string)
			{
				throw new FormatException($"{ExtensionFunction.Sanitize}() third parameter must be a string.");
			}
			replacementCharacters = replacementCharactersObject.ToString();
		}

		functionArgs.Result = Sanitise(inputString, allowedCharacters, replacementCharacters);
	}

	private static string Sanitise(string inputString, string allowedCharacters, string replacementCharacters)
	{
		var builder = new StringBuilder(inputString.Length);

		foreach (var character in inputString)
		{
			if (allowedCharacters.Contains(character))
			{
				builder.Append(character);
			}
			else
			{
				builder.Append(replacementCharacters);
			}
		}

		return builder.ToString();
	}
}
