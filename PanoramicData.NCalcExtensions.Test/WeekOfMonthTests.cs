using PanoramicData.NCalcExtensions.Extensions;

namespace PanoramicData.NCalcExtensions.Test;

public class WeekOfMonthTests : NCalcTest
{
	[Theory]
	[InlineData("2021-11-01", 1)]
	[InlineData("2021-11-02", 1)]
	[InlineData("2021-11-03", 1)]
	[InlineData("2021-11-04", 1)]
	[InlineData("2021-11-05", 1)]
	[InlineData("2021-11-06", 1)]
	[InlineData("2021-11-07", 2)]
	[InlineData("2021-11-08", 2)]
	[InlineData("2021-11-09", 2)]
	[InlineData("2021-11-10", 2)]
	[InlineData("2021-11-11", 2)]
	[InlineData("2021-11-12", 2)]
	[InlineData("2021-11-13", 2)]
	[InlineData("2021-11-14", 3)]
	[InlineData("2021-11-15", 3)]
	[InlineData("2021-11-16", 3)]
	[InlineData("2021-11-17", 3)]
	[InlineData("2021-11-19", 3)]
	[InlineData("2021-11-20", 3)]
	[InlineData("2021-11-21", 4)]
	[InlineData("2021-11-22", 4)]
	[InlineData("2021-11-23", 4)]
	[InlineData("2021-11-24", 4)]
	[InlineData("2021-11-25", 4)]
	[InlineData("2021-11-26", 4)]
	[InlineData("2021-11-27", 4)]
	[InlineData("2021-11-28", 5)]
	[InlineData("2021-11-29", 5)]
	[InlineData("2021-11-30", 5)]
	[InlineData("2022-02-09", 2)]
	public void WeekOfMonthTests_Succeed(string startDateTime, int expectedWeekOfMonth)
	=> DateTime.Parse(startDateTime, CultureInfo.InvariantCulture)
	.WeekOfMonth()
	.Should()
	.Be(expectedWeekOfMonth);
}