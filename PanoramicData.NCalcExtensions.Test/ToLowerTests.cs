namespace PanoramicData.NCalcExtensions.Test;
public class ToLowerTests
{
	[Theory]
	[InlineData("ABC", "abc")]
	[InlineData("abc", "abc")]
	[InlineData("AbC", "abc")]
	[InlineData("123ABC", "123abc")]
	public void ToLower_UsingInlineData_ResultMatchExpectedValue(string parameter, string expectedValue)
	{
		var expression = new ExtendedExpression($"toLower('{parameter}')");
		var result = expression.Evaluate();
		Assert.Equal(expectedValue, result);
	}
}
