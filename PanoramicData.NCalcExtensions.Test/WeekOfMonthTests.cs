using PanoramicData.NCalcExtensions.Extensions;

namespace PanoramicData.NCalcExtensions.Test;

public class WeekOfMonthTests : NCalcTest
{
	[Theory]
	[InlineData("2021-11-01", 1)]
	[InlineData("2021-11-03", 1)]
	[InlineData("2021-11-07", 1)]
	[InlineData("2021-11-08", 2)]
	[InlineData("2021-11-11", 2)]
	[InlineData("2021-11-14", 2)]
	[InlineData("2021-11-15", 3)]
	[InlineData("2021-11-16", 3)]
	[InlineData("2021-11-21", 3)]
	[InlineData("2021-11-22", 4)]
	[InlineData("2021-11-27", 4)]
	[InlineData("2021-11-28", 4)]
	[InlineData("2021-11-29", 5)]
	[InlineData("2021-11-30", 5)]
	public void WeekOfMonthTests_Succeed(string startDateTime, int expectedWeekOfMonth)
	=> DateTime.Parse(startDateTime)
	.WeekOfMonth()
	.Should()
	.Be(expectedWeekOfMonth);
}

