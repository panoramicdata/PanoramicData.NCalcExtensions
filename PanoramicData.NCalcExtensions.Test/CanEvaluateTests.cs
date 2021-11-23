namespace PanoramicData.NCalcExtensions.Test;

public class CanEvaluateTests
{
	[Fact]
	public void CanEvaluate_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("canEvaluate(nonExistent)");
		Assert.False(expression.Evaluate() as bool?);
	}

	[Fact]
	public void CanEvaluate_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("canEvaluate(1)");
		Assert.True(expression.Evaluate() as bool?);
	}
}
