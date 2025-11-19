namespace PanoramicData.NCalcExtensions.Test;

public class EndsWithTests
{
	[Theory]
	[InlineData("'abc', 'a'", false)]
	[InlineData("'abc', 'c'", true)]
	[InlineData("'abc', 'bc'", true)]
	public void EndsWith_UsingInlineData_MatchesExpectedValue(string expressionText, bool expected)
	{
		var expression = new ExtendedExpression($"endsWith({expressionText})");
		var result = expression.Evaluate();
		result.Should().Be(expected);
	}

	[Fact]
	public void EndsWith_NullFirstParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("endsWith(null, 'suffix')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}

	[Fact]
	public void EndsWith_NullSecondParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("endsWith('string', null)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}

	[Fact]
	public void EndsWith_NonStringParameters_ThrowsException()
	{
		var expression = new ExtendedExpression("endsWith(123, 456)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}
}
