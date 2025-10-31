namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("regexIsMatch")]
	[Description("Determine whether a string matches a regex.")]
	bool RegexIsMatch(
		[Description("The input string to be searched.")]
		string input,
		[Description("The regular expression to be matched.")]
		string regex
	);
}

internal static class RegexIsMatch
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var inputString = functionArgs.Parameters[0].Evaluate() as string
			?? throw new FormatException($"{ExtensionFunction.RegexIsMatch} function - first parameter should be a string.");

		var regexExpressionString = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"{ExtensionFunction.RegexIsMatch} function - second parameter should be a string.");

		var regex = new Regex(regexExpressionString);
		functionArgs.Result = regex.IsMatch(inputString);
	}
}
