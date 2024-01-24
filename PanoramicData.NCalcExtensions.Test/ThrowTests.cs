namespace PanoramicData.NCalcExtensions.Test;

public class ThrowTests
{
	[Fact]
	public void Throw_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("throw()");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<NCalcExtensionsException>();
	}

	[Fact]
	public void Throw_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("throw('This is a message')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<NCalcExtensionsException>();
	}

	[Fact]
	public void Throw_Example3_Succeeds()
		=> new ExtendedExpression("if(true, throw('There is a problem'), 5)")
		.Invoking(e => e.Evaluate())
		.Should()
		.ThrowExactly<NCalcExtensionsException>();

	[Fact]
	public void Throw_BadParameter_Fails()
		=> new ExtendedExpression("throw(666)")
		.Invoking(e => e.Evaluate())
		.Should()
		.ThrowExactly<FormatException>();

	[Fact]
	public void Throw_TooManyParameters_Fails()
		=> new ExtendedExpression("throw('a', 'b')")
		.Invoking(e => e.Evaluate())
		.Should()
		.ThrowExactly<FormatException>();

	[Fact]
	public void InnerThrow_Fails()
	{
		var expression = new ExtendedExpression("if(false, 1, throw('sdf' + a))");
		expression.Parameters["a"] = "blah";
		expression
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<NCalcExtensionsException>();
	}
}
