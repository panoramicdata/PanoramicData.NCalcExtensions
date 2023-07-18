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
		Assert.Equal(expected, result);
	}
}
