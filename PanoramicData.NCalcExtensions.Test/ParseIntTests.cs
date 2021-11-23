namespace PanoramicData.NCalcExtensions.Test;

public class ParseIntTests
{
	[Fact]
	public void ParseInt_NoParameter_Throws()
	{
		var expression = new ExtendedExpression("parseInt()");
		var exception = Assert.Throws<FormatException>(() => expression.Evaluate());
	}

	[Fact]
	public void ParseInt_TooManyParameters_Throws()
	{
		var expression = new ExtendedExpression("parseInt('1', '2')");
		var exception = Assert.Throws<FormatException>(() => expression.Evaluate());
	}

	[Fact]
	public void ParseInt_NotAString_Throws()
	{
		var expression = new ExtendedExpression("parseInt(1)");
		var exception = Assert.Throws<FormatException>(() => expression.Evaluate());
	}

	[Fact]
	public void ParseInt_Valid_Succeeds()
	{
		var expression = new ExtendedExpression("parseInt('1')");
		var result = expression.Evaluate();
		result.Should().BeOfType<int>();
		result.Should().Be(1);
	}
}
