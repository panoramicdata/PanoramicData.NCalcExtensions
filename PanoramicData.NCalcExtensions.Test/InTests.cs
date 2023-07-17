namespace PanoramicData.NCalcExtensions.Test;
public class InTests
{
	[Theory]
	[InlineData("1,2,3,4", "1", true)]
	[InlineData("1,2,3,4", "2", true)]
	[InlineData("1,2,3,4", "3", true)]
	[InlineData("1,2,3,4", "4", true)]
	[InlineData("1,2,3,4", "5", false)]
	[InlineData("1,2,3,4", "6", false)]
	[InlineData("1,2,3,4", "7", false)]
	[InlineData("1,2,3,4", "8", false)]

	public void In_Succeeds(string stringList, string item, bool isIn)
		=> new ExtendedExpression($"in({item},{stringList})")
		.Evaluate()
		.Should()
		.Be(isIn);
}
