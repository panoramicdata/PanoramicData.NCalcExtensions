namespace PanoramicData.NCalcExtensions.Test;

public class AnyTests : NCalcTest
{
	[Theory]
	[InlineData("1, 2, 3", true)]
	[InlineData("7, 8, 9", false)]

	public void Any_LessThanFive_ResultMatchesExpectation(string stringList, bool expected)
	{
		var expression = new ExtendedExpression($"any(list({stringList}), 'n', 'n < 5')");

		var result = expression.Evaluate();

		result.Should().Be(expected);
	}

	[Fact]
	public void Any_Items_ReturnsTrue()
	{
		var expression = new ExtendedExpression($"any(list(1, 2, 3))");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Fact]
	public void Any_NoItems_ReturnsFalse()
	{
		var expression = new ExtendedExpression($"any(list())");
		var result = expression.Evaluate();
		result.Should().Be(false);
	}

	[Theory]
	[InlineData("null")]
	[InlineData("'a'")]
	[InlineData("list(1), 1")]
	[InlineData("list(1), null, 1")]
	[InlineData("list(1), 'a', 1")]
	[InlineData("list(1), 'a', null")]
	[InlineData("1, 2")]
	[InlineData("1, 2, 3, 4")]
	public void Any_BadParameters_ThrowsException(string parametersString)
		=> new ExtendedExpression($"any({parametersString})").Invoking(x => x.Evaluate()).Should().ThrowExactly<FormatException>();
}
