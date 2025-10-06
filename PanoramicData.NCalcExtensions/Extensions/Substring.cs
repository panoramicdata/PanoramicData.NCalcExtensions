namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("substring")]
	[Description("Retrieves part of a string. If more characters are requested than available at the end of the string, just the available characters are returned.")]
	string Substring(
		[Description("The original string.")]
		string inputString,
		[Description("The starting index (Zero based).")]
		int startIndex,
		[Description("(Optional) Number of charaters to returned.")]
		int? length = null
	);
}

internal static class Substring
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		string input;
		int startIndex;
		try
		{
			input = (string)functionArgs.Parameters[0].Evaluate();
			startIndex = (int)functionArgs.Parameters[1].Evaluate();
		}
		catch (Exception e) when (e is not NCalcExtensionsException)
		{
			throw new FormatException($"{ExtensionFunction.Substring}() requires a string parameter and one or two numeric parameters.");
		}

		if (functionArgs.Parameters.Length > 2)
		{
			var length = (int)functionArgs.Parameters[2].Evaluate();
			functionArgs.Result = input.Substring(startIndex, Math.Min(length, input.Length - startIndex));
			return;
		}

		functionArgs.Result = input.Substring(startIndex);
	}
}
