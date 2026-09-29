namespace PanoramicData.NCalcExtensions;

/// <summary>
/// Renders a duration as text such as "3 days 14 hours 23 minutes 56 seconds".
/// This is the implementation behind humanize(), public so that other code can produce identical text.
/// </summary>
public static class TimeSpanHumanizer
{
	/// <summary>
	/// Humanizes <paramref name="value"/> <paramref name="timeUnit"/>s, optionally rounded to <paramref name="resolution"/>.
	/// </summary>
	/// <exception cref="OverflowException">The value is too large to represent as a duration.</exception>
	public static string Humanize(double value, TimeUnit timeUnit, TimeUnit? resolution = null)
	{
		var timeSpan = timeUnit switch
		{
			TimeUnit.Milliseconds => System.TimeSpan.FromMilliseconds(value),
			TimeUnit.Seconds => System.TimeSpan.FromSeconds(value),
			TimeUnit.Minutes => System.TimeSpan.FromMinutes(value),
			TimeUnit.Hours => System.TimeSpan.FromHours(value),
			TimeUnit.Days => System.TimeSpan.FromDays(value),
			TimeUnit.Weeks => System.TimeSpan.FromDays(value * 7),
			TimeUnit.Years => System.TimeSpan.FromDays(value * 365.25),
			_ => throw new ArgumentOutOfRangeException(nameof(timeUnit), timeUnit, "Not a supported time unit for humanization."),
		};

		return Humanize(timeSpan, resolution);
	}

	/// <summary>
	/// Humanizes <paramref name="timeSpan"/> into whole days, hours, minutes and seconds.
	/// When <paramref name="resolution"/> is given, the duration is first rounded (half away from zero) to a whole
	/// number of that unit and nothing finer is shown; weeks and years are shown as a single count.
	/// A zero, negative or (without a resolution) sub-second duration gives an empty string.
	/// </summary>
	/// <exception cref="OverflowException">The rounded duration is too large to represent.</exception>
	public static string Humanize(System.TimeSpan timeSpan, TimeUnit? resolution = null)
	{
		if (resolution is null)
		{
			return Breakdown(timeSpan);
		}

		var ticksPerUnit = resolution switch
		{
			TimeUnit.Milliseconds => System.TimeSpan.TicksPerMillisecond,
			TimeUnit.Seconds => System.TimeSpan.TicksPerSecond,
			TimeUnit.Minutes => System.TimeSpan.TicksPerMinute,
			TimeUnit.Hours => System.TimeSpan.TicksPerHour,
			TimeUnit.Days => System.TimeSpan.TicksPerDay,
			TimeUnit.Weeks => System.TimeSpan.TicksPerDay * 7,
			TimeUnit.Years => System.TimeSpan.TicksPerDay * 36525 / 100,
			_ => throw new ArgumentOutOfRangeException(nameof(resolution), resolution, "Not a supported time unit for humanization."),
		};

		var units = checked((long)Math.Round((double)timeSpan.Ticks / ticksPerUnit, MidpointRounding.AwayFromZero));

		// The day-based breakdown has no week or year component, so these are shown as a single count.
		return resolution switch
		{
			TimeUnit.Weeks => Pluralise(units, "week"),
			TimeUnit.Years => Pluralise(units, "year"),
			TimeUnit.Milliseconds => $"{Breakdown(System.TimeSpan.FromTicks(checked(units * ticksPerUnit)))} {Pluralise(units % 1000, "millisecond")}".Trim(),
			_ => Breakdown(System.TimeSpan.FromTicks(checked(units * ticksPerUnit))),
		};
	}

	private static string Breakdown(System.TimeSpan t)
	{
		var durationString = t.Days >= 1 ? $"{t.Days} day{(t.Days > 1 ? "s" : "")}" : "";

		if (t.Hours >= 1)
		{
			durationString += $" {t.Hours} hour{(t.Hours > 1 ? "s" : "")}";
		}

		if (t.Minutes >= 1)
		{
			durationString += $" {t.Minutes} minute{(t.Minutes > 1 ? "s" : "")}";
		}

		if (t.Seconds >= 1)
		{
			durationString += $" {t.Seconds} second{(t.Seconds > 1 ? "s" : "")}";
		}

		return durationString.Trim();
	}

	private static string Pluralise(long count, string unit)
		=> count >= 1 ? $"{count} {unit}{(count > 1 ? "s" : "")}" : "";
}
