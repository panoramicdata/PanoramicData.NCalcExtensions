using System.ComponentModel;

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
	internal static void Evaluate(FunctionArgs functionArgs, CultureInfo cultureInfo)
	{
		if (functionArgs.Parameters.Length != 3)
		{
			throw new FormatException($"{ExtensionFunction.TimeSpan} function - requires three parameters.");
		}

		string? fromString;
		string? toString;
		string? timeFormat;
		try
		{
			fromString = functionArgs.Parameters[0].Evaluate().ToString();
			toString = functionArgs.Parameters[1].Evaluate().ToString();
			timeFormat = functionArgs.Parameters[2].Evaluate().ToString();
		}
		catch (Exception e) when (e is not NCalcExtensionsException)
		{
			throw new FormatException($"{ExtensionFunction.TimeSpan} function -  could not extract three parameters into strings: {e.Message}");
		}

		if (!DateTime.TryParse(fromString, out var fromDateTime))
		{
			throw new FormatException($"{ExtensionFunction.TimeSpan} function -  could not convert '{fromString}' to DateTime");
		}

		if (!DateTime.TryParse(toString, out var toDateTime))
		{
			throw new FormatException($"{ExtensionFunction.TimeSpan} function -  could not convert '{toString}' to DateTime");
		}

		// Determine the timespan
		var timeSpan = toDateTime - fromDateTime;

		functionArgs.Result = Enum.TryParse(timeFormat, true, out TimeUnit timeUnit)
			? GetUnits(timeSpan, timeUnit)
			: timeSpan.ToString(timeFormat, cultureInfo);
	}

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
