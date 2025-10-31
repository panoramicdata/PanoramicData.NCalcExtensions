namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("split")]
	[Description("Splits a string on a given character into a list of strings.")]
	List<string> Split(
		[Description("The original string to be split.")]
		string longString,
		[Description("The string to split on.")]
		string splitOn
	);
}

internal static class Split
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var input = functionArgs.Parameters[0].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.Split}() requires two string parameters.");
			var splitString = functionArgs.Parameters[1].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.Split}() requires two string parameters.");

			functionArgs.Result = splitString.Length switch
			{
				0 => throw new FormatException($"{ExtensionFunction.Split}() requires that the second parameter is not empty."),
				1 => [.. input.Split(splitString[0])],
				_ => input.Split([splitString], StringSplitOptions.None).ToList()
			};
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.Split}() requires two string parameters.");
		}
	}
}