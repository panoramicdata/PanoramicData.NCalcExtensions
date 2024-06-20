namespace PanoramicData.NCalcExtensions.Test;

public class AllTests : NCalcTest
{
	[Theory]
	[InlineData("1, 2, 3", true)]
	[InlineData("4, 5, 6", false)]
	[InlineData("7, 8, 9", false)]
	public void All_LessThanFive_Succeeds(string stringList, bool allLessThanFive)
		=> new ExtendedExpression($"all(list({stringList}), 'n', 'n < 5')")
		.Evaluate()
		.Should()
		.Be(allLessThanFive);

	[Theory]
	[InlineData("true, true, true", true)]
	[InlineData("true, true, false", false)]
	[InlineData("true", true)]
	[InlineData("false", false)]
	[InlineData("", true)]
	public void All_Bools_Succeeds(string stringList, bool expectedResult)
		=> new ExtendedExpression($"all(list({stringList}))")
		.Evaluate()
		.Should()
		.Be(expectedResult);
}
