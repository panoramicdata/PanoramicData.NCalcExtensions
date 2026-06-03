namespace PanoramicData.NCalcExtensions.Test;

public class ClampTests
{
	[Fact]
	public void Clamp_ValueWithinRange_ReturnsValue()
	{
		var expression = new ExtendedExpression("clamp(5.0, 1.0, 10.0)");
		expression.Evaluate().Should().Be(5.0);
	}

	[Fact]
	public void Clamp_ValueBelowMin_ReturnsMin()
	{
		var expression = new ExtendedExpression("clamp(-5.0, 0.0, 10.0)");
		expression.Evaluate().Should().Be(0.0);
	}

	[Fact]
	public void Clamp_ValueAboveMax_ReturnsMax()
	{
		var expression = new ExtendedExpression("clamp(15.0, 0.0, 10.0)");
		expression.Evaluate().Should().Be(10.0);
	}

	[Fact]
	public void Clamp_ValueEqualsMin_ReturnsMin()
	{
		var expression = new ExtendedExpression("clamp(0.0, 0.0, 10.0)");
		expression.Evaluate().Should().Be(0.0);
	}

	[Fact]
	public void Clamp_ValueEqualsMax_ReturnsMax()
	{
		var expression = new ExtendedExpression("clamp(10.0, 0.0, 10.0)");
		expression.Evaluate().Should().Be(10.0);
	}

	[Fact]
	public void Clamp_IntegerValues_Succeeds()
	{
		var expression = new ExtendedExpression("clamp(5, 1, 10)");
		Convert.ToDouble(expression.Evaluate()).Should().Be(5.0);
	}

	[Fact]
	public void Clamp_MinGreaterThanMax_FailsAsExpected()
		=> new ExtendedExpression("clamp(5.0, 10.0, 1.0)")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<NCalcExtensionsException>()
			.WithMessage("clamp() requires that min <= max.");

	[Fact]
	public void Clamp_NullValue_ThrowsException()
		=> new ExtendedExpression("clamp(null, 0.0, 10.0)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*numeric value*numeric min*numeric max*");
}
