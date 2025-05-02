using System.ComponentModel;

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
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		var input = functionArgs.Parameters[0].Evaluate();
		var regexExpression = functionArgs.Parameters[1].Evaluate();
		if (input is not string inputString)
		{
			throw new FormatException($"{ExtensionFunction.RegexIsMatch} function - first parameter should be a string.");
		}

		if (regexExpression is not string regexExpressionString)
		{
			throw new FormatException($"{ExtensionFunction.RegexIsMatch} function - second parameter should be a string.");
		}

		var regex = new Regex(regexExpressionString);
		functionArgs.Result = regex.IsMatch(inputString);
	}
}
