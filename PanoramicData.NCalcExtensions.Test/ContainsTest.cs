namespace PanoramicData.NCalcExtensions.Test;
public class ContainsTest
{
	[Theory]
	[InlineData("contains('abc', 'a')", true)]
	[InlineData("contains('abc', 'ab')", true)]
	[InlineData("contains('abc', 'd')", false)]

	public void Contains_UsingInlineData_MatchesExpectedValue(string expressionText, bool expected)
	{
		var expression = new ExtendedExpression(expressionText);

		var result = expression.Evaluate();

		Assert.Equal(expected, result);
	}
}
