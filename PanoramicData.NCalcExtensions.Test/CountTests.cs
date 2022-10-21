namespace PanoramicData.NCalcExtensions.Test;

public class CountTests
{
	[Fact]
	public void Length_OfList_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"count(split('a piece of string', ' '))");
		var result = expression.Evaluate();
		result.Should().Be(4);
	}
}
