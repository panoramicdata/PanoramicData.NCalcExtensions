namespace PanoramicData.NCalcExtensions.Test;
public class ContainsTest
{
	[Theory]
	[InlineData("contains('abc', 'a')", true)]
	[InlineData("contains('abc', 'b')", true)]
	[InlineData("contains('abc', 'c')", true)]
	[InlineData("contains('abc', 'd')", false)]
	[InlineData("contains('abc', 'ab')", true)]
	[InlineData("contains('abc', 'bc')", true)]
	[InlineData("contains('123', '12')", true)]
	[InlineData("contains('123', '13')", false)]
	public void Contains_Succeeds(string expressionText, bool expected)
	{
		var expression = new ExtendedExpression(expressionText);
		var result = expression.Evaluate();
		Assert.Equal(expected, result);
	}
}
