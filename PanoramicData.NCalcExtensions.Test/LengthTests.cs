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

	[Fact]
	public void Length_EmptyString_ReturnsZero()
	{
		var expression = new ExtendedExpression("length('')");
		var result = expression.Evaluate();
		result.Should().Be(0);
	}

	[Fact]
	public void Length_EmptyList_ReturnsZero()
	{
		var expression = new ExtendedExpression("length(list())");
		var result = expression.Evaluate();
		result.Should().Be(0);
	}

	[Fact]
	public void Length_NullParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("length(null)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}

	[Fact]
	public void Length_NonStringNonListParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("length(123)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}
}
