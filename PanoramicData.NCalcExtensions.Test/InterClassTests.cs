namespace PanoramicData.NCalcExtensions.Test;

public class InterClassTests
{
	[Fact]
	public void AddingAnIntStringToAnInt_YieldsDouble()
	{
		var expression = new ExtendedExpression("0 + '1'");
		var result = expression.Evaluate();
		result.Should().BeOfType<double>();
		result.Should().Be(1);
	}

	[Fact]
	public void AddingAnIntToAnIntString_YieldsDouble()
	{
		var expression = new ExtendedExpression("'1' + 0");
		var result = expression.Evaluate();
		result.Should().BeOfType<double>();
		result.Should().Be(1);
	}
}
