namespace PanoramicData.NCalcExtensions.Test;

public class WhereTests : NCalcTest
{
	[Theory]
	[InlineData("n == 2", 1)]
	[InlineData("n % 2 == 0", 2)]
	public void Where_Succeeds(string expression, int expectedCount)
		=> new ExtendedExpression($"length(where(list(1, 2, 3, 4, 5), 'n', '{expression}'))")
		.Evaluate()
		.Should()
		.Be(expectedCount);
}