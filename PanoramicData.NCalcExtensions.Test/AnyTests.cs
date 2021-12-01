//namespace PanoramicData.NCalcExtensions.Test;

//public class AnyTests
//{
//	[Fact]
//	public void List_AllTrue_ReturnsExpected()
//	{
//		var expression = new ExtendedExpression($"any(list(true, true, true))");
//		expression.Evaluate().Should().Be(true);
//	}

//	[Fact]
//	public void List_SomeFalse_ReturnsExpected()
//	{
//		var expression = new ExtendedExpression($"any(list(true, false, true))");
//		expression.Evaluate().Should().Be(true);
//	}

//	[Fact]
//	public void List_AllFalse_ReturnsExpected()
//	{
//		var expression = new ExtendedExpression($"all(list(false, false, false))");
//		expression.Evaluate().Should().Be(false);
//	}

//	[Fact]
//	public void AllTrue_ReturnsExpected()
//	{
//		var expression = new ExtendedExpression($"any(true, true, true)");
//		expression.Evaluate().Should().Be(true);
//	}

//	[Fact]
//	public void SomeFalse_ReturnsExpected()
//	{
//		var expression = new ExtendedExpression($"any(true, false, true)");
//		expression.Evaluate().Should().Be(true);
//	}

//	[Fact]
//	public void AllFalse_ReturnsExpected()
//	{
//		var expression = new ExtendedExpression($"any(false, false, false)");
//		expression.Evaluate().Should().Be(false);
//	}

//	[Fact]
//	public void Evaluations_ReturnsExpected()
//	{
//		var expression = new ExtendedExpression($"any(1 == 1)");
//		expression.Evaluate().Should().Be(true);
//	}

//	[Fact]
//	public void NoneGiven_ReturnsExpected()
//	{
//		var expression = new ExtendedExpression($"any()");
//		expression.Evaluate().Should().Be(false);
//	}

//	[Fact]
//	public void NonBool_ThrowsException()
//	{
//		var expression = new ExtendedExpression($"any(1)");
//		Assert.Throws<NCalcExtensionsException>(() => expression.Evaluate());
//	}

//	[Fact]
//	public void NonBoolInList_ThrowsException()
//	{
//		var expression = new ExtendedExpression($"any(list(1))");
//		Assert.Throws<NCalcExtensionsException>(() => expression.Evaluate());
//	}
//}
