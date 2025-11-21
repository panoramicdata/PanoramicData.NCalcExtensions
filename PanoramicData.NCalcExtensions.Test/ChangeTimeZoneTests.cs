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
}
