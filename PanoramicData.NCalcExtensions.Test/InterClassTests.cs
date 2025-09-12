namespace PanoramicData.NCalcExtensions.Test;

public class InterClassTests
{
	[Fact]
	public void AddingAnIntStringToAnInt_YieldsString()
	{
		var expression = new ExtendedExpression("0 + '1'");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("01");
		result.Should().NotBe(1);
	}

	[Fact]
	public void AddingAnIntToAnIntString_YieldsString()
	{
		var expression = new ExtendedExpression("'1' + 0");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("10");
	}
}
