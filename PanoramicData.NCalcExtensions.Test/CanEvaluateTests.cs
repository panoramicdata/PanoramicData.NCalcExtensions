namespace PanoramicData.NCalcExtensions.Test;

public class CanEvaluateTests
{
	[Theory]
	[InlineData("nonExistent", false)]
	[InlineData("1", true)]
	[InlineData("ex4mple3", false)]
	public void CanEvaluate_UsingInlineData_MatchesExpectedResult(string value, bool expected)
	{
		var expression = new ExtendedExpression($"canEvaluate({value})");

		var result = expression.Evaluate();

		result.Should().Be(expected);
	}

}
