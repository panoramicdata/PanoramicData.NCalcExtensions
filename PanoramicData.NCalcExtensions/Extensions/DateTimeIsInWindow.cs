using Cronos;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("dateTimeIsInWindow")]
	[Description("Returns whether the current time is within a recurring time window that starts at each fire time of a CRON expression and lasts for a duration in seconds, with an optional timezone. The window includes its start instant and excludes its end instant.")]
	bool DateTimeIsInWindow(
		[Description("CRON expression defining the window start times: 5 fields, or 6 fields when including seconds. '?' is accepted as '*'. Day names (e.g. SUN) are recommended for the day-of-week field.")]
		string cronExpression,
		[Description("Window duration in seconds. Must be positive.")]
		double durationSeconds,
		[Description("TimeZone in which the CRON expression fires. Optional; UTC when omitted.")]
		string timeZoneName
	);
}

internal static class DateTimeIsInWindow
{
	internal static void Evaluate(FunctionEventArgs functionArgs, TimeProvider timeProvider)
	{
		if (functionArgs.Parameters.Count is < 2 or > 3)
		{
			throw new FormatException($"{ExtensionFunction.DateTimeIsInWindow} function - requires two or three arguments: a CRON expression, a duration in seconds and, optionally, a timezone.");
		}

		// Read in parameter order, and parse the CRON expression only once every argument has
		// been accepted, so the first bad argument is the one reported.
		var cronString = ReadCronString(functionArgs);
		var durationSeconds = ReadDurationSeconds(functionArgs);
		var timeZoneInfo = ReadTimeZone(functionArgs);
		var cronExpression = ParseCronExpression(cronString);

		// A window containing now must have started within the last durationSeconds. The first
		// fire time strictly after (now - duration) is at or before now exactly when now is
		// inside that fire's window.
		var nowUtc = timeProvider.GetUtcNow();
		var windowStart = cronExpression.GetNextOccurrence(nowUtc.AddSeconds(-durationSeconds), timeZoneInfo);
		functionArgs.Result = windowStart is not null && windowStart.Value <= nowUtc;
	}

	private static string ReadCronString(FunctionEventArgs functionArgs)
		=> functionArgs.Parameters.Evaluate(0) is string cronString && !string.IsNullOrWhiteSpace(cronString)
			? cronString
			: throw new FormatException($"{ExtensionFunction.DateTimeIsInWindow} function - The first argument must be a CRON expression string.");

	private static double ReadDurationSeconds(FunctionEventArgs functionArgs)
	{
		double durationSeconds;
		try
		{
			durationSeconds = Convert.ToDouble(functionArgs.Parameters.Evaluate(1), CultureInfo.InvariantCulture);
		}
		catch (Exception exception) when (exception is FormatException or InvalidCastException or OverflowException)
		{
			throw new FormatException($"{ExtensionFunction.DateTimeIsInWindow} function - The second argument (durationSeconds) must be a number.");
		}

		return durationSeconds > 0
			? durationSeconds
			: throw new FormatException($"{ExtensionFunction.DateTimeIsInWindow} function - The second argument (durationSeconds) must be positive.");
	}

	/// <summary>
	/// The optional time zone parameter, defaulting to UTC.
	/// </summary>
	private static TimeZoneInfo ReadTimeZone(FunctionEventArgs functionArgs)
	{
		if (functionArgs.Parameters.Count <= 2)
		{
			return TimeZoneInfo.Utc;
		}

		if (functionArgs.Parameters.Evaluate(2) is not string timeZoneName)
		{
			throw new FormatException($"{ExtensionFunction.DateTimeIsInWindow} function - The third argument should be a string, e.g. 'UTC'");
		}

		return TZConvert.TryGetTimeZoneInfo(timeZoneName, out var timeZoneInfo)
			? timeZoneInfo
			: throw new FormatException($"{ExtensionFunction.DateTimeIsInWindow} function - The requested timezone was not recognized");
	}

	private static CronExpression ParseCronExpression(string cronString)
	{
		// 5 fields is standard CRON; 6 includes a leading seconds field. Quartz-style 7-field
		// expressions (with a trailing year) are not supported by Cronos.
		var fieldCount = cronString.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
		var cronFormat = fieldCount switch
		{
			5 => CronFormat.Standard,
			6 => CronFormat.IncludeSeconds,
			_ => throw new FormatException($"{ExtensionFunction.DateTimeIsInWindow} function - The CRON expression must have 5 fields, or 6 fields when including seconds.")
		};

		try
		{
			return CronExpression.Parse(cronString, cronFormat);
		}
		catch (CronFormatException exception)
		{
			throw new FormatException($"{ExtensionFunction.DateTimeIsInWindow} function - '{cronString}' is not a valid CRON expression: {exception.Message}");
		}
	}

}
