namespace PanoramicData.NCalcExtensions.Test;

public class IsInfiniteTests
{
	[Theory]
	[InlineData("1", false)]
	[InlineData("1/0", true)]
	public void IsInfinite_InlineData_ResultsMatchExpectation(string expressionText, bool expected)
	{
		var expression = new ExtendedExpression($"isInfinite({expressionText})");
		var result = expression.Evaluate();
		result.Should().Be(expected);
	}

	// Additional comprehensive tests

	[Fact]
	public void IsInfinite_PositiveInfinity_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isInfinite(1.0 / 0.0)");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void IsInfinite_NegativeInfinity_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isInfinite(-1.0 / 0.0)");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void IsInfinite_Zero_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isInfinite(0)");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void IsInfinite_NormalNumber_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isInfinite(123.456)");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void IsInfinite_NegativeNumber_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isInfinite(-999)");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void IsInfinite_NaN_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isInfinite(0.0 / 0.0)");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void IsInfinite_VeryLargeNumber_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isInfinite(999999999999999)");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void IsInfinite_Null_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isInfinite(null)");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void IsInfinite_String_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isInfinite('text')");
		expression.Evaluate().Should().Be(false);
	}

	[Theory]
	[InlineData("isInfinite()")]
	[InlineData("isInfinite(1, 2)")]
	public void IsInfinite_WrongParameterCount_ThrowsException(string expressionText)
		=> new ExtendedExpression(expressionText)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*requires one parameter*");

	[Fact]
	public void IsInfinite_WithVariable_PositiveInfinity_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isInfinite(value)");
		expression.Parameters["value"] = double.PositiveInfinity;
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void IsInfinite_WithVariable_NegativeInfinity_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isInfinite(value)");
		expression.Parameters["value"] = double.NegativeInfinity;
		expression.Evaluate().Should().Be(true);
	}
}
