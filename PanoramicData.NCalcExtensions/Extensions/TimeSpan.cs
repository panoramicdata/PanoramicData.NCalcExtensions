namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("timeSpan")]
	[Description("Determines the amount of time between two DateTimes. The following units are supported:\r\n\r\n"
		+ "- Years"
		+ "- Weeks"
		+ "- Days"
		+ "- Hours"
		+ "- Minutes"
		+ "- Seconds"
		+ "- Milliseconds"
		+ "- Any other string is handled with TimeSpan.ToString(timeUnit). See https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings"
	)]
	double TimeSpan(
		[Description("The starting DateTime.")]
		DateTime startDateTime,
		[Description("The sending DateTime.")]
		DateTime endDateTime,
		[Description("The time unit to return the duration in.")]
		string timeUnit
	);
}

internal static class TimeSpan
{
	internal static void Evaluate(FunctionEventArgs functionArgs, CultureInfo cultureInfo)
	{
		if (functionArgs.Parameters.Count != 3)
		{
			throw new FormatException($"{ExtensionFunction.TimeSpan} function - requires three parameters.");
		}

		try
		{
			// All three parameters are read before any is parsed, so a null parameter is still
			// reported ahead of an unparseable one.
			var fromString = RequiredString(functionArgs, 0, "first");
			var toString = RequiredString(functionArgs, 1, "second");
			var timeFormat = RequiredString(functionArgs, 2, "third");

			var fromDateTime = ParseDateTime(fromString);
			var toDateTime = ParseDateTime(toString);
			var timeSpan = toDateTime - fromDateTime;

			functionArgs.Result = Enum.TryParse(timeFormat, true, out TimeUnit timeUnit)
				? GetUnits(timeSpan, timeUnit)
				: timeSpan.ToString(timeFormat, cultureInfo);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"{ExtensionFunction.TimeSpan} function - could not extract three parameters into strings: {e.Message}");
		}
	}

	/// <summary>
	/// Reads the parameter at <paramref name="index"/> as a non-null string.
	/// </summary>
	private static string RequiredString(FunctionEventArgs functionArgs, int index, string ordinal)
		=> functionArgs.Parameters.Evaluate(index)?.ToString()
			?? throw new FormatException($"{ExtensionFunction.TimeSpan} function - {ordinal} parameter cannot be null.");

	private static DateTime ParseDateTime(string text)
		=> DateTime.TryParse(text, out var dateTime)
			? dateTime
			: throw new FormatException($"{ExtensionFunction.TimeSpan} function - could not convert '{text}' to DateTime");

	private static double GetUnits(System.TimeSpan timeSpan, TimeUnit timeUnit)
		=> timeUnit switch
		{
			TimeUnit.Milliseconds => timeSpan.TotalMilliseconds,
			TimeUnit.Seconds => timeSpan.TotalSeconds,
			TimeUnit.Minutes => timeSpan.TotalMinutes,
			TimeUnit.Hours => timeSpan.TotalHours,
			TimeUnit.Days => timeSpan.TotalDays,
			TimeUnit.Weeks => timeSpan.TotalDays / 7,
			TimeUnit.Years => timeSpan.TotalDays / 365.25,
			_ => throw new ArgumentOutOfRangeException($"Time unit not supported: '{timeUnit}'"),
		};
}
