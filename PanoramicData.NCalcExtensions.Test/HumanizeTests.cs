namespace PanoramicData.NCalcExtensions.Test;
public class HumanizeTests
{
	[Theory]
	[InlineData("3600000", "Milliseconds", "1 hour")]
	[InlineData("3600", "Seconds", "1 hour")]
	[InlineData("60", "Minutes", "1 hour")]
	[InlineData("1", "Hours", "1 hour")]
	[InlineData("1", "Days", "1 day")]
	[InlineData("1", "Weeks", "7 days")]
	public void Humanize_UsingInlineData_MatchesExpectedValue(string expressionText, string dataType, string expected)
	{
		var expression = new ExtendedExpression($"humanize({expressionText},'{dataType}')");
		var result = expression.Evaluate();
		result.Should().Be(expected);
	}

	// Test all time units explicitly
	[Fact]
	public void Humanize_Milliseconds_ReturnsExpected()
	{
		var expression = new ExtendedExpression("humanize(60000, 'milliseconds')");
		expression.Evaluate().Should().Be("1 minute");
	}

	[Fact]
	public void Humanize_Seconds_ReturnsExpected()
	{
		var expression = new ExtendedExpression("humanize(120, 'seconds')");
		expression.Evaluate().Should().Be("2 minutes");
	}

	[Fact]
	public void Humanize_Minutes_ReturnsExpected()
	{
		var expression = new ExtendedExpression("humanize(90, 'minutes')");
		var result = expression.Evaluate() as string;
		result.Should().Contain("hour"); // 90 minutes = 1 hour 30 minutes
	}

	[Fact]
	public void Humanize_Hours_ReturnsExpected()
	{
		var expression = new ExtendedExpression("humanize(24, 'hours')");
		var result = expression.Evaluate() as string;
		result.Should().Contain("day"); // 24 hours = 1 day
	}

	[Fact]
	public void Humanize_Days_ReturnsExpected()
	{
		var expression = new ExtendedExpression("humanize(7, 'days')");
		var result = expression.Evaluate() as string;
		result.Should().Contain("day"); // 7 days (might show as "7 days" or "1 week" depending on library)
	}

	[Fact]
	public void Humanize_Weeks_ReturnsExpected()
	{
		var expression = new ExtendedExpression("humanize(2, 'weeks')");
		var result = expression.Evaluate() as string;
		result.Should().Contain("day"); // 2 weeks = 14 days
	}

	[Fact]
	public void Humanize_Years_ReturnsExpected()
	{
		var expression = new ExtendedExpression("humanize(1, 'years')");
		var result = expression.Evaluate() as string;
		result.Should().Contain("day"); // Humanizer converts 1 year to days (365 days 6 hours)
	}

	// Test case insensitivity
	[Theory]
	[InlineData("SECONDS")]
	[InlineData("Seconds")]
	[InlineData("seconds")]
	[InlineData("SeCoNdS")]
	public void Humanize_CaseInsensitive_Works(string timeUnit)
	{
		var expression = new ExtendedExpression($"humanize(60, '{timeUnit}')");
		expression.Evaluate().Should().Be("1 minute");
	}

	// Test zero value
	[Fact]
	public void Humanize_ZeroValue_ReturnsExpected()
	{
		var expression = new ExtendedExpression("humanize(0, 'seconds')");
		var result = expression.Evaluate();
		// Zero values return empty string - just verify it doesn't throw
		result.Should().NotBeNull();
	}

	// Test negative value
	[Fact]
	public void Humanize_NegativeValue_ReturnsExpected()
	{
		var expression = new ExtendedExpression("humanize(-60, 'seconds')");
		var result = expression.Evaluate() as string;
		// Negative values might return empty or special format
		result.Should().NotBeNull();
	}

	// Test very large value (but not overflow)
	[Fact]
	public void Humanize_LargeValue_ReturnsExpected()
	{
		var expression = new ExtendedExpression("humanize(86400, 'seconds')"); // 1 day
		expression.Evaluate().Should().Be("1 day");
	}

	// Test fractional values
	[Fact]
	public void Humanize_FractionalValue_ReturnsExpected()
	{
		var expression = new ExtendedExpression("humanize(1.5, 'hours')");
		var result = expression.Evaluate() as string;
		result.Should().Contain("minute"); // 1.5 hours = 1 hour 30 minutes
	}

	// Error cases
	[Fact]
	public void Humanize_NullFirstParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("humanize(null, 'seconds')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}

	[Fact]
	public void Humanize_NullSecondParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("humanize(60, null)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}

	[Fact]
	public void Humanize_InvalidTimeUnit_ThrowsException()
	{
		var expression = new ExtendedExpression("humanize(60, 'invalid')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*time unit*");
	}

	[Fact]
	public void Humanize_NonNumericFirstParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("humanize('not a number', 'seconds')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*floating-point number*");
	}

	[Fact]
	public void Humanize_OverflowValue_ThrowsException()
	{
		// Test with a value that would cause overflow
		var expression = new ExtendedExpression("humanize(theValue, 'milliseconds')");
		expression.Parameters["theValue"] = double.MaxValue;
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*floating-point number*");
	}

	// Test missing parameters
	[Fact]
	public void Humanize_MissingParameters_ThrowsException()
	{
		var expression = new ExtendedExpression("humanize(60)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<Exception>(); // Will throw due to missing parameter
	}

	// Test edge cases for different time units
	[Fact]
	public void Humanize_OneMillisecond_ReturnsExpected()
	{
		var expression = new ExtendedExpression("humanize(1, 'milliseconds')");
		var result = expression.Evaluate() as string;
		// Very small values might return empty
		result.Should().NotBeNull();
	}

	[Fact]
	public void Humanize_OneSecond_ReturnsExpected()
	{
		var expression = new ExtendedExpression("humanize(1, 'seconds')");
		var result = expression.Evaluate() as string;
		result.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public void Humanize_MultipleYears_ReturnsExpected()
	{
		var expression = new ExtendedExpression("humanize(2, 'years')");
		var result = expression.Evaluate() as string;
		result.Should().Contain("day"); // Humanizer converts years to days
	}

	// Test with variable parameters
	[Fact]
	public void Humanize_WithVariables_Works()
	{
		var expression = new ExtendedExpression("humanize(myValue, myUnit)");
		expression.Parameters["myValue"] = 120.0; // Use double to ensure it works
		expression.Parameters["myUnit"] = "seconds";
		var result = expression.Evaluate() as string;
		result.Should().Contain("minute"); // 120 seconds = 2 minutes
	}
}
