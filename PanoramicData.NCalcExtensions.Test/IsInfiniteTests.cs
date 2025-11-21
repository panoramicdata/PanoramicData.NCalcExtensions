namespace PanoramicData.NCalcExtensions.Test;

public class IsInfiniteTests
{
	[Theory]
	[InlineData("1", false)]
	[InlineData("1/0", true)]
	[InlineData("1.0 / 0.0", true)]
	[InlineData("-1.0 / 0.0", true)]
	[InlineData("0", false)]
	[InlineData("123.456", false)]
	[InlineData("-999", false)]
	[InlineData("0.0 / 0.0", false)] // NaN
	[InlineData("999999999999999", false)]
	[InlineData("null", false)]
	[InlineData("'text'", false)]
	public void IsInfinite_VariousInputs_ReturnsExpected(string input, bool expected)
	{
		var expression = new ExtendedExpression($"isInfinite({input})");
		expression.Evaluate().Should().Be(expected);
	}

	[Theory]
	[InlineData("isInfinite()")]
	[InlineData("isInfinite(1, 2)")]
	[InlineData("isInfinite(1, 2, 3)")]
	public void IsInfinite_WrongParameterCount_ThrowsException(string expressionText)
		=> new ExtendedExpression(expressionText)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*requires one parameter*");

	[Theory]
	[InlineData(double.PositiveInfinity, true)]
	[InlineData(double.NegativeInfinity, true)]
	[InlineData(42.0, false)]
	[InlineData(0.0, false)]
	public void IsInfinite_WithVariable_ReturnsExpected(double value, bool expected)
	{
		var expression = new ExtendedExpression("isInfinite(value)");
		expression.Parameters["value"] = value;
		expression.Evaluate().Should().Be(expected);
	}
}
