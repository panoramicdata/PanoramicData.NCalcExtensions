namespace PanoramicData.NCalcExtensions.Test;

public class IsNanTests
{
	[Fact]
	public void IsNan_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("isNaN(1)");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void IsNan_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("isNaN(null)");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void IsNan_Example3_Succeeds()
	{
		var expression = new ExtendedExpression("isNaN('text')");
		expression.Evaluate().Should().Be(true);
	}
}
