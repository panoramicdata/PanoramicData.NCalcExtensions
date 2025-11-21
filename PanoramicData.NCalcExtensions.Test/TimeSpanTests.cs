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

	[Theory]
	[InlineData("'2020-01-01 00:00', '2020-01-01 00:00', 'Days'", 0.0)]
	[InlineData("'2020-01-01', '2020-01-02', 'Days'", 1.0)]
	[InlineData("'2020-01-01 00:00', '2020-01-01 01:00', 'Hours'", 1.0)]
	[InlineData("'2020-01-01 00:00', '2020-01-01 00:01', 'Minutes'", 1.0)]
	[InlineData("'2020-01-01 00:00:00', '2020-01-01 00:00:01', 'Seconds'", 1.0)]
	[InlineData("'2020-01-02', '2020-01-01', 'Days'", -1.0)]
	[InlineData("'2020-01-01 00:00:00', '2020-01-01 00:00:01', 'Milliseconds'", 1000.0)]
	[InlineData("'2020-01-01', '2020-01-15', 'Weeks'", 2.0)]
	public void TimeSpan_VariousTimeUnits_ReturnsExpected(string args, double expected)
	{
		var result = Test($"timespan({args})");
		result.Should().Be(expected);
	}

	[Fact]
	public void TimeSpan_WithVariables_Works()
	{
		var expression = new ExtendedExpression("timespan(start, end, 'Hours')");
		expression.Parameters["start"] = "2020-01-01 00:00";
		expression.Parameters["end"] = "2020-01-01 12:00";
		var result = expression.Evaluate();
		result.Should().Be(12.0);
	}

	[Fact]
	public void TimeSpan_YearsFormat_Works()
	{
		var result = Test("timespan('2020-01-01', '2022-01-01', 'Years')");
		var years = (double)result!;
		years.Should().BeApproximately(2.0, 0.01);
	}

	[Fact]
	public void TimeSpan_DateTimeObjects_Works()
	{
		var expression = new ExtendedExpression("timespan(dt1, dt2, 'Days')");
		expression.Parameters["dt1"] = new DateTime(2020, 1, 1);
		expression.Parameters["dt2"] = new DateTime(2020, 1, 10);
		expression.Evaluate().Should().Be(9.0);
	}

	[Theory]
	[InlineData("c")]
	[InlineData("g")]
	public void TimeSpan_StandardFormats_ReturnsString(string format)
	{
		var result = Test($"timespan('2020-01-01 00:00', '2020-01-01 01:30:45', '{format}')");
		result.Should().BeOfType<string>();
		((string)result!).Should().Contain(":");
	}

	[Fact]
	public void TimeSpan_ComponentFormat_dd_Works()
	{
		var result = Test("timespan('2020-01-01', '2020-01-05', 'dd')");
		result.Should().Be("04");
	}

	[Fact]
	public void TimeSpan_LargeTimeSpan_Works()
	{
		var result = Test("timespan('2020-01-01', '2021-01-01', 'Days')");
		var days = (double)result!;
		days.Should().BeGreaterThan(365.0);
	}

	[Theory]
	[InlineData("timespan('2020-01-01')")]
	[InlineData("timespan('2020-01-01', '2020-01-02')")]
	[InlineData("timespan(null, '2020-01-01', 'Days')")]
	[InlineData("timespan('not a date', '2020-01-01', 'Days')")]
	[InlineData("timespan('2020-01-01', '2020-01-02', 'InvalidFormat')")]
	public void TimeSpan_InvalidInput_ThrowsException(string expression) => new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should().Throw<Exception>();
}
