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
}
