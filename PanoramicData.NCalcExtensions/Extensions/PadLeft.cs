namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("padLeft")]
	[Description("Pad the left of a string with a character to a desired string length.")]
	string PadLeft(
		[Description("The string to pad.")]
		string stringToPad,
		[Description("The desired string length (must be >= 1).")]
		int length,
		[Description("The character used to pad.")]
		string paddingCharacter
	);
}

internal static class PadLeft
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		try
		{
			var input = functionArgs.Parameters.Evaluate(0) as string
				?? throw new FormatException($"{ExtensionFunction.PadLeft}() requires a string Input, an integer DesiredStringLength, and a single Padding character.");

			if (functionArgs.Parameters.Evaluate(1) is not int desiredStringLength)
			{
				throw new FormatException($"{ExtensionFunction.PadLeft}() requires a string Input, an integer DesiredStringLength, and a single Padding character.");
			}

			if (desiredStringLength < 1)
			{
				throw new NCalcExtensionsException($"{ExtensionFunction.PadLeft}() requires a DesiredStringLength for parameter 2 that is >= 1.");
			}

			var paddingString = functionArgs.Parameters.Evaluate(2) as string
				?? throw new NCalcExtensionsException($"{ExtensionFunction.PadLeft}() requires that parameter 3 be a string.");

			if (paddingString.Length != 1)
			{
				throw new NCalcExtensionsException($"{ExtensionFunction.PadLeft}() requires a single character string for parameter 3.");
			}

			var paddingCharacter = paddingString[0];
			functionArgs.Result = input.PadLeft(desiredStringLength, paddingCharacter);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"{ExtensionFunction.PadLeft}() requires a string Input, an integer DesiredStringLength, and a single Padding character.");
		}
	}
}
