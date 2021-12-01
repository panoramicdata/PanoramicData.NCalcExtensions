//namespace PanoramicData.NCalcExtensions.Test;

//public class AllTests
//{
//	[Fact]
//	public void List_AllTrue_ReturnsExpected()
//	{
//		var expression = new ExtendedExpression($"all(list(true, true, true))");
//		expression.Evaluate().Should().Be(true);
//	}

//	[Fact]
//	public void List_SomeFalse_ReturnsExpected()
//	{
//		var expression = new ExtendedExpression($"all(list(true, false, true))");
//		expression.Evaluate().Should().Be(false);
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
//		var expression = new ExtendedExpression($"all(true, true, true)");
//		expression.Evaluate().Should().Be(true);
//	}

//	[Fact]
//	public void SomeFalse_ReturnsExpected()
//	{
//		var expression = new ExtendedExpression($"all(true, false, true)");
//		expression.Evaluate().Should().Be(false);
//	}

//	[Fact]
//	public void AllFalse_ReturnsExpected()
//	{
//		var expression = new ExtendedExpression($"all(false, false, false)");
//		expression.Evaluate().Should().Be(false);
//	}

//	[Fact]
//	public void Evaluations_ReturnsExpected()
//	{
//		var expression = new ExtendedExpression($"all(1 == 1)");
//		expression.Evaluate().Should().Be(true);
//	}

//	[Fact]
//	public void NoneGiven_ThrowsException()
//	{
//		var expression = new ExtendedExpression($"all()");
//		Assert.Throws<NCalcExtensionsException>(() => expression.Evaluate());
//	}

//	[Fact]
//	public void NonBool_ThrowsException()
//	{
//		var expression = new ExtendedExpression($"all(1)");
//		Assert.Throws<NCalcExtensionsException>(() => expression.Evaluate());
//	}

//	[Fact]
//	public void NonBoolInList_ThrowsException()
//	{
//		var expression = new ExtendedExpression($"all(list(1))");
//		Assert.Throws<NCalcExtensionsException>(() => expression.Evaluate());
//	}
//}