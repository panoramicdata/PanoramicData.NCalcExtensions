namespace PanoramicData.NCalcExtensions.Test;

public class CanEvaluateTests
{
	[Fact]
	public void CanEvaluate_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("canEvaluate(nonExistent)");
		(expression.Evaluate() as bool?).Should().BeFalse();
	}

	[Fact]
	public void CanEvaluate_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("canEvaluate(1)");
		(expression.Evaluate() as bool?).Should().BeTrue();
	}
}
