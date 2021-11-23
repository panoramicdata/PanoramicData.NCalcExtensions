namespace PanoramicData.NCalcExtensions.Test;

public class ThrowTests
{
	[Fact]
	public void Throw_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("throw()");
		Assert.Throws<NCalcExtensionsException>(() => expression.Evaluate());
	}

	[Fact]
	public void Throw_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("throw('This is a message')");
		Assert.Throws<NCalcExtensionsException>(() => expression.Evaluate());
	}

	[Fact]
	public void Throw_Example3_Succeeds()
	{
		var expression = new ExtendedExpression("if(true, throw('There is a problem'), 5)");
		Assert.Throws<NCalcExtensionsException>(() => expression.Evaluate());
	}

	[Fact]
	public void Throw_BadParameter_Fails()
	{
		var expression = new ExtendedExpression("throw(666)");
		Assert.Throws<FormatException>(() => expression.Evaluate());
	}

	[Fact]
	public void Throw_TooManyParameters_Fails()
	{
		var expression = new ExtendedExpression("throw('a', 'b')");
		Assert.Throws<FormatException>(() => expression.Evaluate());
	}

	[Fact]
	public void InnerThrow_Fails()
	{
		var expression = new ExtendedExpression("if(false, 1, throw('sdf' + a))");
		expression.Parameters["a"] = "blah";
		Assert.Throws<NCalcExtensionsException>(() => expression.Evaluate());
	}
}
