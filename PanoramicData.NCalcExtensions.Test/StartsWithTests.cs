namespace PanoramicData.NCalcExtensions.Test;
public class StartsWithTests
{
	[Theory]
	[InlineData("abc","a",true)]
	[InlineData("abc","b",false)]
	[InlineData("abc","ab",true)]
	[InlineData("abc","bc",false)]
	public void StartsWith_UsingInlineData_ResultMatchExpectedValue(string parameter1, string parameter2, bool expectedValue)
	{
		var expression = new ExtendedExpression($"startsWith('{parameter1}','{parameter2}')");
		var result = expression.Evaluate();
		result.Should().Be(expectedValue);

	}
}
