namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("replace")]
	[Description("Replace a string with another string.")]
	string Replace(
		[Description("The input string to be searched.")]
		string haystack,
		[Description("The smaller string being searched for.")]
		string needle1,
		[Description("The new replacement text to substituted for each match.")]
		string newNeedle1,
		[Description("The next smaller string being searched for.")]
		string needle2,
		[Description("The next new replacement text to substituted for each match.")]
		string newNeedle2
	);
}

internal static class Replace
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		// Input checks
		switch (functionArgs.Parameters.Length)
		{
			case 0:
			case 1:
			case 2:
				throw new FormatException($"{ExtensionFunction.Replace}() requires at least three string parameters.");
			default:
				if (functionArgs.Parameters.Length % 2 == 0)
				{
					throw new FormatException($"{ExtensionFunction.Replace}() requires an odd number of string parameters.");
				}
				// All good
				break;
		}

		var haystackString = functionArgs.Parameters[0].Evaluate() as string
			?? throw new NCalcExtensionsException($"{ExtensionFunction.Replace}() requires a string parameter.");

		var needleIndex = 1;
		while (needleIndex < functionArgs.Parameters.Length)
		{
			var needle = functionArgs.Parameters[needleIndex++].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.Replace}() requires string parameters.");
			var replacementNeedle = functionArgs.Parameters[needleIndex++].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.Replace}() requires string parameters.");

			haystackString = haystackString.Replace(needle, replacementNeedle);
		}

		functionArgs.Result = haystackString;
	}
}
