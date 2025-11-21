namespace PanoramicData.NCalcExtensions.Test;

public class ChangeTimeZonesTests : NCalcTest
{
	[Fact]
	public void UtcToEst_Succeeds()
	{
		const string parameterName = "theDateTimeUtc";
		var expression = new ExtendedExpression($"changeTimeZone({parameterName}, 'UTC', 'Eastern Standard Time')");
		expression.Parameters[parameterName] = new DateTime(2020, 03, 13, 16, 00, 00);
		var result = expression.Evaluate();
		result.Should().Be(new DateTime(2020, 03, 13, 12, 00, 00));
	}

	[Fact]
	public void EstToUtc_Succeeds()
	{
		const string parameterName = "theDateTimeUtc";
		var expression = new ExtendedExpression($"changeTimeZone({parameterName}, 'Eastern Standard Time', 'UTC')");
		expression.Parameters[parameterName] = new DateTime(2020, 03, 13, 12, 00, 00);
		var result = expression.Evaluate();
		result.Should().Be(new DateTime(2020, 03, 13, 16, 00, 00));
	}

	[Fact]
	public void ChangeTimeZone_WrongParameterCount_ThrowsException()
	{
		var expression = new ExtendedExpression("changeTimeZone(now(), 'UTC')");
		expression.Invoking(e => e.Evaluate()).Should().Throw<ArgumentException>()
			.WithMessage("*Expected 3 arguments*");
	}

	[Fact]
	public void ChangeTimeZone_NonDateTimeFirstParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("changeTimeZone('not a date', 'UTC', 'EST')");
		expression.Invoking(e => e.Evaluate()).Should().Throw<ArgumentException>()
			.WithMessage("*parameter 1 should be a DateTime*");
	}

	[Fact]
	public void ChangeTimeZone_NonStringSecondParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("changeTimeZone(now(), 123, 'EST')");
		expression.Invoking(e => e.Evaluate()).Should().Throw<ArgumentException>()
			.WithMessage("*parameter 2 should be a string*");
	}

	[Fact]
	public void ChangeTimeZone_NonStringThirdParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("changeTimeZone(now(), 'UTC', 456)");
		expression.Invoking(e => e.Evaluate()).Should().Throw<ArgumentException>()
			.WithMessage("*parameter 3 should be a string*");
	}

	// Additional comprehensive tests
	[Fact]
	public void ChangeTimeZone_UtcToUtc_NoChange()
	{
		var dateTime = new DateTime(2023, 6, 15, 12, 0, 0);
		var expression = new ExtendedExpression("changeTimeZone(dt, 'UTC', 'UTC')");
		expression.Parameters["dt"] = dateTime;
		expression.Evaluate().Should().Be(dateTime);
	}

	[Fact]
	public void ChangeTimeZone_PacificToEastern_Succeeds()
	{
		var expression = new ExtendedExpression("changeTimeZone(dt, 'Pacific Standard Time', 'Eastern Standard Time')");
		expression.Parameters["dt"] = new DateTime(2023, 1, 15, 10, 0, 0);
		var result = expression.Evaluate() as DateTime?;
		result.Should().NotBeNull();
		// PST is UTC-8, EST is UTC-5, so +3 hours
		result.Should().Be(new DateTime(2023, 1, 15, 13, 0, 0));
	}

	[Fact]
	public void ChangeTimeZone_TokyoToLondon_Succeeds()
	{
		var expression = new ExtendedExpression("changeTimeZone(dt, 'Tokyo Standard Time', 'GMT Standard Time')");
		expression.Parameters["dt"] = new DateTime(2023, 1, 15, 18, 0, 0);
		var result = expression.Evaluate() as DateTime?;
		result.Should().NotBeNull();
	}

	[Fact]
	public void ChangeTimeZone_InvalidSourceTimezone_ThrowsException()
	{
		var expression = new ExtendedExpression("changeTimeZone(now(), 'Invalid/Timezone', 'UTC')");
		expression.Invoking(e => e.Evaluate()).Should().Throw<Exception>();
	}

	[Fact]
	public void ChangeTimeZone_InvalidTargetTimezone_ThrowsException()
	{
		var expression = new ExtendedExpression("changeTimeZone(now(), 'UTC', 'Invalid/Timezone')");
		expression.Invoking(e => e.Evaluate()).Should().Throw<Exception>();
	}

	[Fact]
	public void ChangeTimeZone_WithNow_Works()
	{
		var expression = new ExtendedExpression("changeTimeZone(now(), 'UTC', 'Eastern Standard Time')");
		var result = expression.Evaluate();
		result.Should().BeOfType<DateTime>();
	}

	[Fact]
	public void ChangeTimeZone_DateTimeMin_Succeeds()
	{
		var expression = new ExtendedExpression("changeTimeZone(dt, 'UTC', 'Eastern Standard Time')");
		expression.Parameters["dt"] = DateTime.MinValue;
		var result = expression.Evaluate();
		result.Should().BeOfType<DateTime>();
	}

	[Fact]
	public void ChangeTimeZone_DateTimeMax_Succeeds()
	{
		var expression = new ExtendedExpression("changeTimeZone(dt, 'UTC', 'Eastern Standard Time')");
		expression.Parameters["dt"] = DateTime.MaxValue;
		var result = expression.Evaluate();
		result.Should().BeOfType<DateTime>();
	}

	[Fact]
	public void ChangeTimeZone_NoParameters_ThrowsException()
	{
		var expression = new ExtendedExpression("changeTimeZone()");
		expression.Invoking(e => e.Evaluate()).Should().Throw<ArgumentException>();
	}

	[Fact]
	public void ChangeTimeZone_OneParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("changeTimeZone(now())");
		expression.Invoking(e => e.Evaluate()).Should().Throw<ArgumentException>();
	}

	[Fact]
	public void ChangeTimeZone_FourParameters_ThrowsException()
	{
		var expression = new ExtendedExpression("changeTimeZone(now(), 'UTC', 'EST', 'extra')");
		expression.Invoking(e => e.Evaluate()).Should().Throw<ArgumentException>();
	}

	[Fact]
	public void ChangeTimeZone_NullDateTime_ThrowsException()
	{
		var expression = new ExtendedExpression("changeTimeZone(null, 'UTC', 'EST')");
		expression.Invoking(e => e.Evaluate()).Should().Throw<ArgumentException>();
	}

	[Fact]
	public void ChangeTimeZone_NullSourceTimezone_ThrowsException()
	{
		var expression = new ExtendedExpression("changeTimeZone(now(), null, 'EST')");
		expression.Invoking(e => e.Evaluate()).Should().Throw<ArgumentException>();
	}

	[Fact]
	public void ChangeTimeZone_NullTargetTimezone_ThrowsException()
	{
		var expression = new ExtendedExpression("changeTimeZone(now(), 'UTC', null)");
		expression.Invoking(e => e.Evaluate()).Should().Throw<ArgumentException>();
	}

	[Fact]
	public void ChangeTimeZone_DuringDaylightSaving_Succeeds()
	{
		// During daylight saving time
		var expression = new ExtendedExpression("changeTimeZone(dt, 'UTC', 'Eastern Standard Time')");
		expression.Parameters["dt"] = new DateTime(2023, 7, 15, 12, 0, 0);
		var result = expression.Evaluate();
		result.Should().BeOfType<DateTime>();
	}

	[Fact]
	public void ChangeTimeZone_ChainedWithFormat_Works()
	{
		var expression = new ExtendedExpression("format(changeTimeZone(dt, 'UTC', 'Eastern Standard Time'), 'yyyy-MM-dd HH:mm')");
		expression.Parameters["dt"] = new DateTime(2023, 1, 15, 12, 0, 0);
		var result = expression.Evaluate() as string;
		result.Should().NotBeNullOrEmpty();
	}
}
