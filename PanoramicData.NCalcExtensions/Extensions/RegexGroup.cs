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
			functionArgs.Result = GetCapture(regex.Match(input), regexCaptureIndex);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"{ExtensionFunction.RegexGroup}() requires string parameters.");
		}
	}

	/// <summary>
	/// The capture at <paramref name="captureIndex"/>, or null if the match failed or there is no
	/// capture at that index.
	/// </summary>
	/// <remarks>
	/// Captures are flattened across all groups (1..N) in order, so that multi-group patterns can
	/// be indexed across all their captures.
	/// </remarks>
	private static string? GetCapture(Match match, int captureIndex)
	{
		if (!match.Success)
		{
			return null;
		}

		var allCaptures = Enumerable
			.Range(1, match.Groups.Count - 1)
			.SelectMany(groupIndex => match.Groups[groupIndex].Captures.Cast<Capture>())
			.ToList();

		return captureIndex >= allCaptures.Count
			? null
			: allCaptures[captureIndex].Value;
	}
}
