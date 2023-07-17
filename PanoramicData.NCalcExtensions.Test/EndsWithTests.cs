namespace PanoramicData.NCalcExtensions.Test;

public class EndsWithTests
{
	[Theory]
	[InlineData("'abc', 'a'", false)]
	[InlineData("'abc', 'b'", false)]
	[InlineData("'abc', 'c'", true)]
	[InlineData("'abc', 'd'", false)]
	[InlineData("'abc', 'ab'", false)]
	[InlineData("'abc', 'bc'", true)]
	[InlineData("'123', '12'", false)]
	[InlineData("'123', '23'", true)]
	[InlineData("'123', '13'", false)]
	[InlineData("'123', '123'", true)]
	public void EndsWith_Succeeds(string expressionText, bool expected)
	{
		var expression = new ExtendedExpression($"endsWith({expressionText})");
		var result = expression.Evaluate();
		Assert.Equal(expected, result);
	}
}
