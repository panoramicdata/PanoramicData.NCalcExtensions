using System.Collections.Generic;
using System.Linq;

namespace PanoramicData.NCalcExtensions.Test;

public class SumTests
{
	private readonly List<int> _intList = [1, 2, 3];
	private readonly List<object?> _objectList = [1f, 2d, 3, null];

	[Fact]
	public void Sum_WithLambda_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"sum(x, 'n', 'n * n')");
		expression.Parameters.Add("x", _intList);
		var result = expression.Evaluate();
		result.Should().Be(_intList.Sum(n => n * n));
	}

	[Fact]
	public void Sum_OfIntegers_WithLambda_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"sum(list(100, 100, 100), 'n', 'n')");
		expression.Parameters.Add("x", _intList);
		var result = expression.Evaluate();
		result.Should().Be(300);
	}

	[Fact]
	public void Sum_OfEnumerableOfInt_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"sum(x)");
		expression.Parameters.Add("x", _intList.AsEnumerable());
		var result = expression.Evaluate();
		result.Should().Be(_intList.Sum());
	}

	[Fact]
	public void Sum_OfListOfInt_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"sum(x)");
		expression.Parameters.Add("x", _intList);
		var result = expression.Evaluate();
		result.Should().Be(_intList.Sum());
	}

	[Fact]
	public void Sum_OfListOfObject_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"sum(x)");
		expression.Parameters.Add("x", _objectList);
		var result = expression.Evaluate();
		result.Should().Be(_intList.Sum());
	}

	// New edge case tests

	[Fact]
	public void Sum_EmptyList_ReturnsZero()
	{
		var expression = new ExtendedExpression("sum(list())");
		var result = expression.Evaluate();
		result.Should().Be(0d);
	}

	[Fact]
	public void Sum_AllZeros_ReturnsZero()
	{
		var expression = new ExtendedExpression("sum(list(0, 0, 0, 0))");
		var result = expression.Evaluate();
		result.Should().Be(0);
	}

	[Fact]
	public void Sum_NegativeNumbers_ReturnsNegativeSum()
	{
		var expression = new ExtendedExpression("sum(list(-1, -2, -3))");
		var result = expression.Evaluate();
		result.Should().Be(-6);
	}

	[Fact]
	public void Sum_MixedPositiveAndNegative_ReturnsCorrectSum()
	{
		var expression = new ExtendedExpression("sum(list(10, -5, 3, -2))");
		var result = expression.Evaluate();
		result.Should().Be(6);
	}

	[Fact]
	public void Sum_VeryLargeNumbers_HandlesOverflow()
	{
		var expression = new ExtendedExpression("sum(list(1000000000, 1000000000, 1000000000))");
		var result = expression.Evaluate();
		result.Should().Be(3000000000);
	}

	[Fact]
	public void Sum_Decimals_RetainsDecimalPrecision()
	{
		var expression = new ExtendedExpression("sum(list(1.5, 2.5, 3.5))");
		var result = expression.Evaluate();
		result.Should().Be(7.5);
	}

	[Fact]
	public void Sum_ByteValues_ConvertsCorrectly()
	{
		var expression = new ExtendedExpression("sum(listOf('byte', 1, 2, 3))");
		var result = expression.Evaluate();
		result.Should().Be(6);
	}

	[Fact]
	public void Sum_ShortValues_ConvertsCorrectly()
	{
		var expression = new ExtendedExpression("sum(listOf('short', 100, 200, 300))");
		var result = expression.Evaluate();
		result.Should().Be(600);
	}

	[Fact]
	public void Sum_LongValues_ConvertsCorrectly()
	{
		var expression = new ExtendedExpression("sum(listOf('long', 1000, 2000, 3000))");
		var result = expression.Evaluate();
		result.Should().Be(6000L);
	}

	[Fact]
	public void Sum_FloatValues_HandlesFloatingPoint()
	{
		var expression = new ExtendedExpression("sum(listOf('float', 1.1, 2.2, 3.3))");
		var result = expression.Evaluate();
		((float)result!).Should().BeApproximately(6.6f, 0.01f);
	}

	[Fact]
	public void Sum_DoubleValues_HandlesFloatingPoint()
	{
		var expression = new ExtendedExpression("sum(listOf('double', 1.1, 2.2, 3.3))");
		var result = expression.Evaluate();
		((double)result!).Should().BeApproximately(6.6, 0.01);
	}

	[Fact]
	public void Sum_WithLambdaOnEmptyList_ReturnsZero()
	{
		var expression = new ExtendedExpression("sum(list(), 'x', 'x * 2')");
		var result = expression.Evaluate();
		result.Should().Be(0d);
	}

	[Fact]
	public void Sum_SingleItem_ReturnsThatItem()
	{
		var expression = new ExtendedExpression("sum(list(42))");
		var result = expression.Evaluate();
		result.Should().Be(42);
	}

	// Lambda tests for all numeric types
	[Fact]
	public void Sum_ByteWithLambda_ReturnsCorrectSum()
	{
		var expression = new ExtendedExpression("sum(listOf('byte', 1, 2, 3), 'n', 'n * 2')");
		var result = expression.Evaluate();
		result.Should().Be(12);
	}

	[Fact]
	public void Sum_ShortWithLambda_ReturnsCorrectSum()
	{
		var expression = new ExtendedExpression("sum(listOf('short', 10, 20, 30), 'n', 'n * 2')");
		var result = expression.Evaluate();
		result.Should().Be(120);
	}

	[Fact]
	public void Sum_LongWithLambda_ReturnsCorrectSum()
	{
		var expression = new ExtendedExpression("sum(listOf('long', 100, 200, 300), 'n', 'n * 2')");
		var result = expression.Evaluate();
		result.Should().Be(1200L);
	}

	[Fact]
	public void Sum_FloatWithLambda_ReturnsCorrectSum()
	{
		var expression = new ExtendedExpression("sum(listOf('float', 1.0, 2.0, 3.0), 'n', 'n * 2')");
		var result = expression.Evaluate();
		((float)result!).Should().BeApproximately(12.0f, 0.01f);
	}

	[Fact]
	public void Sum_DoubleWithLambda_ReturnsCorrectSum()
	{
		var expression = new ExtendedExpression("sum(listOf('double', 1.0, 2.0, 3.0), 'n', 'n * 2')");
		var result = expression.Evaluate();
		((double)result!).Should().BeApproximately(12.0, 0.01);
	}

	[Fact]
	public void Sum_DecimalWithLambda_ReturnsCorrectSum()
	{
		var expression = new ExtendedExpression("sum(listOf('decimal', 1.5, 2.5, 3.5), 'n', 'n * 2')");
		var result = expression.Evaluate();
		result.Should().Be(15.0m);
	}

	// JValue support tests
	[Fact]
	public void Sum_JValueInteger_ReturnsCorrectSum()
	{
		var expression = new ExtendedExpression("sum(jArray(1, 2, 3))");
		var result = expression.Evaluate();
		result.Should().Be(6.0);
	}

	[Fact]
	public void Sum_JValueFloat_ReturnsCorrectSum()
	{
		var expression = new ExtendedExpression("sum(jArray(1.5, 2.5, 3.0))");
		var result = expression.Evaluate();
		((double)result!).Should().BeApproximately(7.0, 0.01);
	}

	// Error case tests
	[Fact]
	public void Sum_NullParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("sum(null)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*cannot be null*");
	}

	[Fact]
	public void Sum_InvalidSecondParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("sum(list(1, 2, 3), 123, 'n')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*must be a string*");
	}

	[Fact]
	public void Sum_InvalidThirdParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("sum(list(1, 2, 3), 'n', 456)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*must be a string*");
	}

	[Fact]
	public void Sum_UnsupportedTypeWithoutLambda_ThrowsException()
	{
		var expression = new ExtendedExpression("sum(list('a', 'b', 'c'))");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}

	[Fact]
	public void Sum_UnsupportedTypeInObjectList_ThrowsException()
	{
		var expression = new ExtendedExpression("sum(TheList)");
		expression.Parameters["TheList"] = new List<object?> { 1, 2, "invalid" };
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*unsupported type*");
	}
}
