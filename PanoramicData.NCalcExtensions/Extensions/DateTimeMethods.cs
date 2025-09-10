namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("dateTime")]
	[Description("Return the DateTime in the specified format as a string, with an optional offset.")]
	DateTime DateTimeMethods(
		[Description("TimeZone (only 'UTC' currently supported).")]
		string timeZone,
		[Description("Format to be returned, example: 'yyyy-MM-dd HH:mm:ss'.")]
		string format,
		[Description("day offset")]
		int dayOffset,
		[Description("hour offset")]
		int hourOffset,
		[Description("minute offset")]
		int minuteOffset,
		[Description("second offset")]
		int secondOffset
	);
}

internal static class DateTimeMethods
{
	internal static void Evaluate(FunctionArgs functionArgs, CultureInfo cultureInfo)
	{
		if (functionArgs.Parameters.Length > 0)
		{
			// Time Zone
			if (functionArgs.Parameters[0].Evaluate() is not string timeZone)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - The first argument should be a string, e.g. 'UTC'");
			}
			// TODO - support more than just UTC
			if (timeZone != "UTC")
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Only UTC timeZone is currently supported.");
			}
		}
		// Time zone has been determined

		// Format
		var format = functionArgs.Parameters.Length > 1
			? functionArgs.Parameters[1].Evaluate() as string
			: "yyyy-MM-dd HH:mm:ss";
		// Format has been determined

		// Days to add
		double daysToAdd = 0;
		if (functionArgs.Parameters.Length > 2)
		{
			var daysToAddNullable = GetNullableDouble(functionArgs.Parameters[2]);
			if (!daysToAddNullable.HasValue)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Days to add must be a number.");
			}

			daysToAdd = daysToAddNullable.Value;
		}

		// Hours to add
		double hoursToAdd = 0;
		if (functionArgs.Parameters.Length > 3)
		{
			var hoursToAddNullable = GetNullableDouble(functionArgs.Parameters[3]);
			if (!hoursToAddNullable.HasValue)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Hours to add must be a number.");
			}

			hoursToAdd = hoursToAddNullable.Value;
		}

		// Minutes to add
		double minutesToAdd = 0;
		if (functionArgs.Parameters.Length > 4)
		{
			var minutesToAddNullable = GetNullableDouble(functionArgs.Parameters[4]);
			if (!minutesToAddNullable.HasValue)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Minutes to add must be a number.");
			}

			minutesToAdd = minutesToAddNullable.Value;
		}

		// Seconds to add
		double secondsToAdd = 0;
		if (functionArgs.Parameters.Length > 5)
		{
			var secondsToAddNullable = GetNullableDouble(functionArgs.Parameters[5]);
			if (!secondsToAddNullable.HasValue)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Seconds to add must be a number.");
			}

			secondsToAdd = secondsToAddNullable.Value;
		}

		functionArgs.Result = DateTimeOffset
			.UtcNow
			.AddDays(daysToAdd)
			.AddHours(hoursToAdd)
			.AddMinutes(minutesToAdd)
			.AddSeconds(secondsToAdd)
			.ToString(format, cultureInfo);
	}

	private static double? GetNullableDouble(Expression expression)
		=> expression.Evaluate() switch
		{
			double doubleResult => doubleResult,
			int intResult => intResult,
			_ => null,
		};

	internal static string BetterToString(this DateTime dateTime, string format, CultureInfo cultureInfo)
		=> format switch
		{
			"dayOfYear" => dateTime.DayOfYear.ToString(cultureInfo),
			"weekOfMonth" => dateTime.WeekOfMonth().ToString(cultureInfo),
			"weekOfMonthText" => GetWeekText(dateTime.WeekOfMonth()),
			"weekDayOfMonth" => dateTime.WeekDayOfMonth().ToString(cultureInfo),
			"weekDayOfMonthText" => GetWeekText(dateTime.WeekDayOfMonth()),
			_ => dateTime.ToString(format, cultureInfo)
		};

	private static string GetWeekText(int weekOfMonth) => weekOfMonth switch
	{
		1 => "first",
		2 => "second",
		3 => "third",
		4 => "fourth",
		_ => "last"
	};

	public static int WeekOfMonth(this DateTime dateTime)
	{
		var date = dateTime.Date;
		var firstOfMonth = new DateTime(date.Year, date.Month, 1);
		return ((date - firstOfMonth
			.AddDays(-(int)firstOfMonth.DayOfWeek)).Days / 7) + 1;
	}

	public static int WeekDayOfMonth(this DateTime dateTime)
		=> ((dateTime.Day - 1) / 7) + 1;

	internal static string ToDateTimeInTargetTimeZone(this DateTime dateTime, string formatFormat, string timeZoneString, CultureInfo cultureInfo)
	{
		var timeZoneInfo = TZConvert.GetTimeZoneInfo(timeZoneString);
		var dateTimeOffset = new DateTimeOffset(dateTime, -timeZoneInfo.GetUtcOffset(dateTime));
		return dateTimeOffset.UtcDateTime.BetterToString(formatFormat, cultureInfo);
	}
}
