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
	public void Sum_OfIntegetrs_WithLambda_ReturnsExpectedResult()
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
}
