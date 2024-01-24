namespace PanoramicData.NCalcExtensions.Test;
public class IsInfiniteTests
{
	[Theory]
	[InlineData("1", false)]
	[InlineData("1/0", true)]
	public void IsInfinite_InlineData_ResultsMatchExpectation(string expressionText, bool expected)
	{
		var expression = new ExtendedExpression($"isInfinite({expressionText})");
		var result = expression.Evaluate();
		result.Should().Be(expected);
	}
}
