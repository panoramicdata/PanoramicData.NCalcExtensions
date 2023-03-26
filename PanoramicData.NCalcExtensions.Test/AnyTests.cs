namespace PanoramicData.NCalcExtensions.Test;

public class AnyTests : NCalcTest
{
	[Theory]
	[InlineData("1, 2, 3", true)]
	[InlineData("7, 8, 9", false)]
	public void Any_LessThanFive_Succeeds(string stringList, bool anyLessThanFive)
		=> new ExtendedExpression($"any(list({stringList}), 'n', 'n < 5')")
		.Evaluate()
		.Should()
		.Be(anyLessThanFive);
}
