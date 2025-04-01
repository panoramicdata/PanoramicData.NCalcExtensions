namespace PanoramicData.NCalcExtensions.Test;

public class MultiLineTests : NCalcTest
{
	[Theory]
	[InlineData("""true""")]
	[InlineData("""
// This is a comment
true
""")]
	[InlineData("""
// This has a blank line

true
""")]
	[InlineData("""
// Split mid-calculation
1 ==
1
""")]
	[InlineData("""
// Indented
if(
	1 == 1,
	true,
	false
)
""")]
	public void All_LessThanFive_Succeeds(string multiLineExpression)
		=> new ExtendedExpression(multiLineExpression)
		.Evaluate()
		.Should()
		.Be(true);
}
