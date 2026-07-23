using System.Collections.Concurrent;

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
	private static readonly ConcurrentDictionary<string, Regex> RegexCache = new(StringComparer.Ordinal);

	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		var inputString = functionArgs.Parameters.Evaluate(0) as string
			?? throw new FormatException($"{ExtensionFunction.RegexIsMatch} function - first parameter should be a string.");

		var regexExpressionString = functionArgs.Parameters.Evaluate(1) as string
			?? throw new FormatException($"{ExtensionFunction.RegexIsMatch} function - second parameter should be a string.");

		var regex = RegexCache.GetOrAdd(regexExpressionString, static pattern => new Regex(pattern));
		functionArgs.Result = regex.IsMatch(inputString);
	}
}
