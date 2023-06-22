namespace PanoramicData.NCalcExtensions.Test;

public class IsNanTests
{
	[Fact]
	public void IsNan_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("isNan(1)");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void IsNan_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("isNan(null)");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void IsNan_Example3_Succeeds()
	{
		var expression = new ExtendedExpression("isNan('text')");
		expression.Evaluate().Should().Be(true);
	}
}
