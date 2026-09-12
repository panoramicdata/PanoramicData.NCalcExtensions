namespace PanoramicData.NCalcExtensions.Test;

public class WeekOfYearTests : NCalcTest
{
	[Theory]
	// weekOfYear, in the invariant culture.
	[InlineData("weekOfYear", "2024-01-01", 1)]    // Monday, week 1
	[InlineData("weekOfYear", "2024-01-07", 2)]    // Sunday, week 2
	[InlineData("weekOfYear", "2024-12-31", 53)]   // Tuesday, week 53
	[InlineData("weekOfYear", "2023-01-01", 1)]    // Sunday, week 1
	[InlineData("weekOfYear", "2023-12-31", 53)]   // Sunday, week 53
	[InlineData("weekOfYear", "2021-01-01", 1)]    // Friday, week 1
	[InlineData("weekOfYear", "2021-12-31", 53)]   // Friday, week 53
	// isoWeekOfYear, where a week belongs to the year holding its Thursday.
	[InlineData("isoWeekOfYear", "2024-01-01", 1)]   // Monday starts ISO week 1
	[InlineData("isoWeekOfYear", "2024-01-07", 1)]   // Sunday ends ISO week 1
	[InlineData("isoWeekOfYear", "2024-12-30", 1)]   // Monday starts ISO week 1 of 2025
	[InlineData("isoWeekOfYear", "2023-01-01", 52)]  // Sunday is in ISO week 52 of 2022
	[InlineData("isoWeekOfYear", "2023-01-02", 1)]   // Monday starts ISO week 1 of 2023
	[InlineData("isoWeekOfYear", "2021-01-01", 53)]  // Friday is in ISO week 53 of 2020
	[InlineData("isoWeekOfYear", "2021-01-04", 1)]   // Monday starts ISO week 1 of 2021
	public void Format_WeekOfYear_ReturnsExpected(string format, string dateString, int expectedWeek)
		=> Test($"format('{dateString}', '{format}')")
			.Should().Be(expectedWeek.ToString(CultureInfo.InvariantCulture));

	[Theory]
	[InlineData("weekOfYear")]
	[InlineData("isoWeekOfYear")]
	public void Format_WeekOfYear_WithDateTime_ReturnsAWeekNumber(string format)
		=> WeekNumberOf(Test($"format(theDateTime, '{format}')", "theDateTime", new DateTime(2024, 6, 15)))
			.Should().BeGreaterThan(0).And.BeLessThanOrEqualTo(53);

	[Theory]
	[InlineData("2024-W01", "2024-01-01")]
	[InlineData("2024-W26", "2024-06-24")]
	[InlineData("2024-W52", "2024-12-23")]
	public void Format_IsoWeekOfYear_MatchesExpectedWeek(string expectedIsoWeek, string dateString)
		=> WeekNumberOf(Test($"format('{dateString}', 'isoWeekOfYear')"))
			.Should().Be(int.Parse(expectedIsoWeek.Split('-')[1][1..], CultureInfo.InvariantCulture));

	/// <summary>
	/// The week number in a format() result.
	/// </summary>
	private static int WeekNumberOf(object? result)
	{
		result.Should().NotBeNull();
		return int.Parse(result.ToString()!, CultureInfo.InvariantCulture);
	}
}
