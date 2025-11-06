namespace PanoramicData.NCalcExtensions.Test;

public class FormatTests
{
	[Fact]
	public void Format_InvalidFormat_Fails()
		=> new ExtendedExpression("format(1, 1)")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<ArgumentException>()
			.WithMessage("format function - expected second argument to be a format string");

	[Theory]
	[InlineData("format()")]
	[InlineData("format(1)")]
	public void Format_NotTwoParameters_Fails(string inputText)
		=> new ExtendedExpression(inputText)
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<ArgumentException>()
			.WithMessage("format function - expected between 2 and 3 arguments");

	[Theory]
	[InlineData("format(1, 2, 3)")]
	public void Format_ThreeParametersForInt_Fails(string inputText)
		=> new ExtendedExpression(inputText)
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<ArgumentException>()
			.WithMessage("format function - expected second argument to be a format string");

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

	// Additional format type tests
	[Fact]
	public void Format_NullValue_ReturnsNull()
	{
		var expression = new ExtendedExpression("format(null, 'yyyy-MM-dd')");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void Format_LongInteger_Succeeds()
	{
		var expression = new ExtendedExpression("format(123456789, '#,##0')");
		expression.Evaluate().Should().Be("123,456,789");
	}

	[Fact]
	public void Format_DecimalNumber_Succeeds()
	{
		var expression = new ExtendedExpression("format(theDecimal, '0.00')");
		expression.Parameters["theDecimal"] = 123.456m;
		expression.Evaluate().Should().Be("123.46");
	}

	[Fact]
	public void Format_Float_Succeeds()
	{
		var expression = new ExtendedExpression("format(theFloat, '0.00')");
		expression.Parameters["theFloat"] = 123.456f;
		var result = expression.Evaluate() as string;
		result.Should().StartWith("123.4");
	}

	[Fact]
	public void Format_DateTimeOffset_Succeeds()
	{
		var expression = new ExtendedExpression("format(theDateTimeOffset, 'yyyy-MM-dd')");
		expression.Parameters["theDateTimeOffset"] = new DateTimeOffset(2021, 1, 15, 0, 0, 0, TimeSpan.Zero);
		expression.Evaluate().Should().Be("2021-01-15");
	}

	[Fact]
	public void Format_InvalidTimeZone_ThrowsException()
	{
		var expression = new ExtendedExpression("format(theDateTime, 'yyyy-MM-dd', 'InvalidTimeZone')");
		expression.Parameters["theDateTime"] = DateTime.UtcNow;
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<Exception>();
	}

	[Fact]
	public void Format_CustomDateTimeFormat_Succeeds()
	{
		var expression = new ExtendedExpression("format(theDateTime, 'dd MMM yyyy')");
		expression.Parameters["theDateTime"] = new DateTime(2021, 3, 15);
		var result = expression.Evaluate() as string;
		result.Should().Contain("15");
		result.Should().Contain("2021");
	}

	/// <summary>
	///  See https://cdn.a-printable-calendar.com/images/large/full-year-calendar-2021.png and
	///  See https://cdn.a-printable-calendar.com/images/large/full-year-calendar-2022.png
	/// </summary>
	/// <param name="dateTimeString"></param>
	/// <param name="expectedWeekOfMonth"></param>
	[Theory(DisplayName = "The numeric week of month as would be shown on a calendar with one row per week with weeks starting on a Sunday")]
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
	public void Format_DateFormat_WeekOfMonth_Succeeds(string dateTimeString, int expectedWeekOfMonth)
	{
		var expression = new ExtendedExpression($"format('{dateTimeString}', 'weekOfMonth')");
		expression.Evaluate().Should().Be(expectedWeekOfMonth.ToString(CultureInfo.InvariantCulture));
	}

	[Theory(DisplayName = "weekDayOfMonth calculates the number of times (including this time) that the day of week has occurred so far.")]
	[InlineData("2021-11-28", 4)] // This is in week 5 and is the 4th Sunday
	[InlineData("2021-11-30", 5)] // This is in week 5 and is the 5th Tuesday
	[InlineData("2022-02-09", 2)] // This is in week 2 and is the 2nd Wednesday
	public void Format_DateFormat_WeekDayOfMonth_Succeeds(string dateTimeString, int expectedWeekDayOfMonth)
	{
		var expression = new ExtendedExpression($"format('{dateTimeString}', 'weekDayOfMonth')");
		expression.Evaluate().Should().Be(expectedWeekDayOfMonth.ToString(CultureInfo.InvariantCulture));
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
		=> new ExtendedExpression("format('XXX', 'yyyy-MM-dd')")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<FormatException>()
			.WithMessage("Could not parse 'XXX' as a number or date.");

	[Fact]
	public void Format_SingleH_Succeeds()
	{
		var expression = new ExtendedExpression("parseInt(format(toDateTime((dateTimeAsEpochMs('2021-01-17 12:45:00', 'yyyy-MM-dd HH:mm:ss')) + (dateTimeAsEpochMs('1970-01-01 08:00:00', 'yyyy-MM-dd HH:mm:ss')), 'ms', 'UTC'), 'HH'))");
		var result = expression.Evaluate();
		result.Should().Be(20);
	}

	// Additional edge cases
	[Fact]
	public void Format_ZeroInteger_Succeeds()
	{
		var expression = new ExtendedExpression("format(0, '0000')");
		expression.Evaluate().Should().Be("0000");
	}

	[Fact]
	public void Format_NegativeNumber_Succeeds()
	{
		var expression = new ExtendedExpression("format(-123, '0000')");
		expression.Evaluate().Should().Be("-0123");
	}

	[Fact]
	public void Format_VeryLargeNumber_Succeeds()
	{
		var expression = new ExtendedExpression("format(theLong, '#,##0')");
		expression.Parameters["theLong"] = 9223372036854775807L;
		expression.Evaluate().Should().Be("9,223,372,036,854,775,807");
	}

	[Fact]
	public void Format_WithVariables_Works()
	{
		var expression = new ExtendedExpression("format(myValue, myFormat)");
		expression.Parameters["myValue"] = 42;
		expression.Parameters["myFormat"] = "000";
		expression.Evaluate().Should().Be("042");
	}
}
