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
		Assert.Equal(expected, expression.Evaluate() as string);
	}

	[Fact]
	public void Substring_TwoParameters_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 1)");
		Assert.Equal("BC", expression.Evaluate() as string);
	}

	[Fact]
	public void Substring_ThreeParameters_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 1, 1)");
		Assert.Equal("B", expression.Evaluate() as string);
	}

	[Fact]
	public void Substring_ThreeParametersTruncate_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 0, 6)");
		Assert.Equal("ABC", expression.Evaluate() as string);
	}

	[Fact]
	public void Substring_ThreeParametersTruncateMidString_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 1, 6)");
		Assert.Equal("BC", expression.Evaluate() as string);
	}
}
