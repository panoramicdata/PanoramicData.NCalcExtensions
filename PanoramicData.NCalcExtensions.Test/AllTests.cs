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
}
