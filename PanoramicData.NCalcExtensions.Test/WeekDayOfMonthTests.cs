using PanoramicData.NCalcExtensions.Extensions;

namespace PanoramicData.NCalcExtensions.Test;

public class WeekDayOfMonthTests : NCalcTest
{
	[Theory(DisplayName = "weekDayOfMonth is the number of times this weekday has occurred within the month so far, including this one")]
	[InlineData("2021-11-01", 1)]
	[InlineData("2021-11-02", 1)]
	[InlineData("2021-11-03", 1)]
	[InlineData("2021-11-04", 1)]
	[InlineData("2021-11-05", 1)]
	[InlineData("2021-11-06", 1)]
	[InlineData("2021-11-07", 1)]
	[InlineData("2021-11-08", 2)]
	[InlineData("2021-11-09", 2)]
	[InlineData("2021-11-10", 2)]
	[InlineData("2021-11-11", 2)]
	[InlineData("2021-11-12", 2)]
	[InlineData("2021-11-13", 2)]
	[InlineData("2021-11-14", 2)]
	[InlineData("2021-11-15", 3)]
	[InlineData("2021-11-16", 3)]
	[InlineData("2021-11-17", 3)]
	[InlineData("2021-11-19", 3)]
	[InlineData("2021-11-20", 3)]
	[InlineData("2021-11-21", 3)]
	[InlineData("2021-11-22", 4)]
	[InlineData("2021-11-23", 4)]
	[InlineData("2021-11-24", 4)]
	[InlineData("2021-11-25", 4)]
	[InlineData("2021-11-26", 4)]
	[InlineData("2021-11-27", 4)]
	[InlineData("2021-11-28", 4)]
	[InlineData("2021-11-29", 5)]
	[InlineData("2021-11-30", 5)]
	[InlineData("2022-02-09", 2)]
	public void WeekDayOfMonthTests_Succeed(string startDateTime, int expectedWeekOfMonth)
	=> DateTime.Parse(startDateTime)
	.WeekDayOfMonth()
	.Should()
	.Be(expectedWeekOfMonth);
}

