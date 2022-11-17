namespace PanoramicData.NCalcExtensions.Helpers;

internal static class TimeSpanExtensions
{
	internal static string Humanise(this System.TimeSpan t)
	{
		// Humanize a TimeSpan into days, hours, minutes and seconds.
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
}
