namespace PanoramicData.NCalcExtensions.Test;

public class IsSetTests
{
	[Fact]
	public void IsSet_IsNotSet_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isSet('a')");
		Assert.False(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsSet_IsSet_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isSet('a')");
		expression.Parameters["a"] = 1;
		Assert.True(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsSet_IsSetToNull_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isSet('a')");
		expression.Parameters["a"] = null;
		Assert.True(expression.Evaluate() as bool?);
	}
}
