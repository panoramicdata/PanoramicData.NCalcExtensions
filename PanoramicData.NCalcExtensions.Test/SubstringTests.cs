namespace PanoramicData.NCalcExtensions.Test;

public class SubstringTests
{
	[Theory]
	[InlineData("substring('haystack', 3)", "stack")]
	[InlineData("substring('haystack', 0, 3)", "hay")]
	[InlineData("substring('haystack', 3, 100)", "stack")]
	[InlineData("substring('haystack', 0, 100)", "haystack")]
	[InlineData("substring('haystack', 0, 0)", "")]
	public void Substring_HelpExamples_Succeed(string expressionText, string expected)
	{
		var expression = new ExtendedExpression(expressionText);
		(expression.Evaluate() as string).Should().Be(expected);
	}

	[Fact]
	public void Substring_TwoParameters_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 1)");
		(expression.Evaluate() as string).Should().Be("BC");
	}

	[Fact]
	public void Substring_ThreeParameters_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 1, 1)");
		(expression.Evaluate() as string).Should().Be("B");
	}

	[Fact]
	public void Substring_ThreeParametersTruncate_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 0, 6)");
		(expression.Evaluate() as string).Should().Be("ABC");
	}

	[Fact]
	public void Substring_ThreeParametersTruncateMidString_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 1, 6)");
		(expression.Evaluate() as string).Should().Be("BC");
	}
}
