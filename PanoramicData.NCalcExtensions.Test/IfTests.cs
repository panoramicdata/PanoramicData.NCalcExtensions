namespace PanoramicData.NCalcExtensions.Test;
public class IfTests
{
	[Theory]
	[InlineData("1 == 1", "yes", "no", "yes")]
	[InlineData("1 == 2", "yes", "no", "no")]

	public void If_UsingInlineData_ResultMatchesExpectation(string expressionText, string trueValue, string falseValue, object expected)
	{
		var expression = new ExtendedExpression($"if({expressionText},'{trueValue}','{falseValue}')");
		var result = expression.Evaluate();
		Assert.Equal(expected, result);
	}

}
