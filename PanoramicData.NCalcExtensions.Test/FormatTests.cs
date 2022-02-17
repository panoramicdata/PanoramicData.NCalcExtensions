namespace PanoramicData.NCalcExtensions.Test;

public class FormatTests
{
	[Fact]
	public void Format_InvalidFormat_Fails()
	{
		var expression = new ExtendedExpression("format(1, 1)");
		var exception = Assert.Throws<ArgumentException>(() => expression.Evaluate());
		exception.Message.Should().Be("format function - expected second argument to be a format string");
	}

	[Theory]
	[InlineData("format()")]
	[InlineData("format(1)")]
	public void Format_NotTwoParameters_Fails(string inputText)
	{
		var expression = new ExtendedExpression(inputText);
		var exception = Assert.Throws<ArgumentException>(() => expression.Evaluate());
		exception.Message.Should().Be("format function - expected between 2 and 3 arguments");
	}

	[Theory]
	[InlineData("format(1, 2, 3)")]
	public void Format_ThreeParametersForInt_Fails(string inputText)
	{
		var expression = new ExtendedExpression(inputText);
		var exception = Assert.Throws<ArgumentException>(() => expression.Evaluate());
		exception.Message.Should().Be("format function - expected second argument to be a format string");
	}

	[Fact]
	public void Format_IntFormat_Succeeds()
	{
		var expression = new ExtendedExpression("format(1, '00')");
		expression.Evaluate().Should().Be("01");
	}

	[Fact]
	public void Format_DoubleFormat_Succeeds()
	{
		var expression = new ExtendedExpression("format(1.0, '00')");
		expression.Evaluate().Should().Be("01");
	}

	[Fact]
	public void Format_DateTimeFormat_Succeeds()
	{
		var expression = new ExtendedExpression("format(dateTime('UTC', 'yyyy-MM-dd'), 'yyyy-MM-dd')");
		expression.Evaluate().Should().Be(DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
	}

	[Fact]
	public void Format_DateTimeFormatWithTimeZone_Succeeds()
	{
		var expression = new ExtendedExpression("format(theDateTime, 'yyyy-MM-dd HH:mm', 'Eastern Standard Time')");
		expression.Parameters.Add("theDateTime", DateTime.Parse("2020-03-13 16:00", CultureInfo.InvariantCulture));
		expression.Evaluate().Should().Be("2020-03-13 12:00");
	}

	[Fact]
	public void Format_StringFormat_Succeeds()
	{
		var expression = new ExtendedExpression("format('02', '0')");
		expression.Evaluate().Should().Be("2");
	}

	[Fact]
	public void Format_DateFormat_DayOfYear_Succeeds()
	{
		var expression = new ExtendedExpression("format('2021-11-29', 'dayOfYear')");
		expression.Evaluate().Should().Be("333");
	}

	[Fact]
	public void Format_DateFormat_WeekOfMonth_Succeeds()
	{
		var expression = new ExtendedExpression("format('2021-11-30', 'weekOfMonth')");
		expression.Evaluate().Should().Be("5");
	}

	[Theory]
	[InlineData("2021-11-30", 5)]
	[InlineData("2022-02-09", 2)]
	public void Format_DateFormat_WeekOfMonth2_Succeeds(string dateTimeString, int expectedWeekOfMonth)
	{
		var expression = new ExtendedExpression($"format('{dateTimeString}', 'weekOfMonth')");
		expression.Evaluate().Should().Be(expectedWeekOfMonth.ToString(CultureInfo.InvariantCulture));
	}

	[Fact]
	public void Format_DateFormat_WeekOfMonthText_Succeeds()
	{
		var expression = new ExtendedExpression("format('2021-11-30', 'weekOfMonthText')");
		expression.Evaluate().Should().Be("last");
	}

	[Fact]
	public void Format_DateTimeStringFormat_Succeeds()
	{
		var expression = new ExtendedExpression("format('01/01/2019', 'yyyy-MM-dd')");
		expression.Evaluate().Should().Be("2019-01-01");
	}

	[Fact]
	public void Format_InvalidStringFormat_Succeeds()
	{
		var expression = new ExtendedExpression("format('XXX', 'yyyy-MM-dd')");
		var exception = Assert.Throws<FormatException>(() => expression.Evaluate());
		exception.Message.Should().Be("Could not parse 'XXX' as a number or date.");
	}

	[Fact]
	public void Format_SingleH_Succeeds()
	{
		var expression = new ExtendedExpression("parseInt(format(toDateTime((dateTimeAsEpochMs('2021-01-17 12:45:00', 'yyyy-MM-dd HH:mm:ss')) + (dateTimeAsEpochMs('1970-01-01 08:00:00', 'yyyy-MM-dd HH:mm:ss')), 'ms', 'UTC'), 'HH'))");
		var result = expression.Evaluate();
		result.Should().Be(20);
	}
}
