namespace PanoramicData.NCalcExtensions.Test;

public class InterClassTests
{
	[Fact]
	public void AddingAnIntStringToAnInt_YieldsInt()
	{
		var expression = new ExtendedExpression("0 + '1'");
		// BREAKING CHANGE in 5.6.0: previously this yielded a decimal
		var result = expression.Evaluate();
		result.Should().BeOfType<double>();
		result.Should().Be(1);
		result.Should().NotBe("1");
	}

	[Fact]
	public void AddingAnIntToAnIntString_YieldsString()
	{
		var expression = new ExtendedExpression("'1' + 0");
		// BREAKING CHANGE in 5.6.0: previously this yielded a string
		var result = expression.Evaluate();
		result.Should().BeOfType<double>();
		result.Should().Be(1);
	}
}
