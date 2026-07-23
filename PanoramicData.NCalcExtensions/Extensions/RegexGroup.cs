using System.Collections.Concurrent;

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
	private static readonly ConcurrentDictionary<string, Regex> RegexCache = new(StringComparer.Ordinal);

	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		try
		{
			var input = functionArgs.Parameters.Evaluate(0) as string
				?? throw new FormatException($"{ExtensionFunction.RegexGroup}() requires string parameters.");
			var regexExpression = functionArgs.Parameters.Evaluate(1) as string
				?? throw new FormatException($"{ExtensionFunction.RegexGroup}() requires string parameters.");

			var regexCaptureIndex = functionArgs.Parameters.Count == 3
				? functionArgs.Parameters.Evaluate(2) as int? ?? 0
				: 0;

			var regex = RegexCache.GetOrAdd(regexExpression, static pattern => new Regex(pattern));
				var match = regex.Match(input);
				if (!match.Success)
				{
					functionArgs.Result = null;
				}
				else
				{
					// Flatten captures across all groups (1..N) in order so that
					// multi-group patterns can be indexed across all their captures.
					var allCaptures = Enumerable
						.Range(1, match.Groups.Count - 1)
						.SelectMany(i => match.Groups[i].Captures.Cast<Capture>())
						.ToList();

					functionArgs.Result = regexCaptureIndex >= allCaptures.Count
						? null
						: allCaptures[regexCaptureIndex].Value;
				}
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"{ExtensionFunction.RegexGroup}() requires string parameters.");
		}
	}
}
