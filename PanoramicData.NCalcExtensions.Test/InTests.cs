namespace PanoramicData.NCalcExtensions.Test;
public class InTests
{
	[Theory]
	[InlineData("1,2,3,4", "1", true)]
	[InlineData("1,2,3,4", "2", true)]
	[InlineData("1,2,3,4", "3", true)]
	[InlineData("1,2,3,4", "4", true)]
	[InlineData("1,2,3,4", "5", false)]


	public void In_UsingInlineData_ResultMatchesExpectation(string stringList, string item, bool expected)
	{
		var expression = new ExtendedExpression($"in({item},{stringList})");

		var result = expression.Evaluate();

		Assert.Equal(expected, result);
	}
}
