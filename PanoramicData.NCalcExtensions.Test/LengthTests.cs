namespace PanoramicData.NCalcExtensions.Test;

public class LengthTests
{
	[Fact]
	public void Length_OfString_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"length('a piece of string')");
		var result = expression.Evaluate();
		result.Should().Be(17);
	}

	[Fact]
	public void Length_OfList_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"length(split('a piece of string', ' '))");
		var result = expression.Evaluate();
		result.Should().Be(4);
	}
}
