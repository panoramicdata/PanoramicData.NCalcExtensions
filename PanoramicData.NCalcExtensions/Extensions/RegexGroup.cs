namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("regexGroup")]
	[Description("Selects a regex group capture.")]
	string RegexGroup(
		[Description("The input string to be searched.")]
		string input,
		[Description("The regular expression to be matched.")]
		string regex,
		[Description("Zero-based index of the capture whose value will be returned (default: 0).")]
		int captureIndex
	);
}

internal static class RegexGroup
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var input = (string)functionArgs.Parameters[0].Evaluate();
			var regexExpression = (string)functionArgs.Parameters[1].Evaluate();
			var regexCaptureIndex = functionArgs.Parameters.Length == 3
				? (int)functionArgs.Parameters[2].Evaluate()
				: 0;
			var regex = new Regex(regexExpression);
			if (!regex.IsMatch(input))
			{
				functionArgs.Result = null;
			}
			else
			{
				var group = regex
					.Match(input)
					.Groups[1];
				functionArgs.Result = regexCaptureIndex >= group.Captures.Count
					? null
					: group.Captures[regexCaptureIndex].Value;
			}
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.Replace}() requires three string parameters.");
		}
	}
}
