namespace PanoramicData.NCalcExtensions.Test;

public class TimeSpanTests : NCalcTest
{
	[Theory]
	[InlineData("Years", 0.1645656196922453)]
	[InlineData("Weeks", 8.586798941798943)]
	[InlineData("Days", 60.107592592592596)]
	[InlineData("Hours", 1442.5822222222223)]
	[InlineData("Minutes", 86554.93333333333)]
	[InlineData("Seconds", 5193296.0)]
	[InlineData("Milliseconds", 5193296000.0)]
	[InlineData("c", "60.02:34:56")]
	[InlineData("g", "60:2:34:56")]
	[InlineData("G", "60:02:34:56.0000000")]
	[InlineData("hh", "02")]
	[InlineData("mm", "34")]
	[InlineData("ss", "56")]
	public void Timespan_Succeeds(string format, object expectedValue)
	{
		var result = Test($"timespan('2020-01-01 00:00', '2020-03-01 02:34:56', '{format}')");
		result.Should().Be(expectedValue);
	}
}
