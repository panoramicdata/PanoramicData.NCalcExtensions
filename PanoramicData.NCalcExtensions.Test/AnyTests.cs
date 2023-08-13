namespace PanoramicData.NCalcExtensions.Test;

public class AnyTests : NCalcTest
{
	[Theory]
	[InlineData("1, 2, 3", true)]
	[InlineData("7, 8, 9", false)]

	public void Any_LessThanFive_ResultMatchesExpectation(string stringList, bool expected)
	{
		var expression = new ExtendedExpression($"any(list({stringList}), 'n', 'n < 5')");

		var result = expression.Evaluate();

		Assert.Equal(expected, result);
	}
}
