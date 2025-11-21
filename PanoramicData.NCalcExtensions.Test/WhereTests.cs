using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class WhereTests : NCalcTest
{
	[Theory]
	[InlineData("n == 2", 1)]
	[InlineData("n % 2 == 0", 2)]
	public void Where_Succeeds(string expression, int expectedCount)
		=> new ExtendedExpression($"length(where(list(1, 2, 3, 4, 5), 'n', '{expression}'))")
		.Evaluate()
		.Should()
		.Be(expectedCount);

	[Fact]
	public void Where_WithJArray_Succeeds()
	{
		var expressionText = """
where(
  jArray(
    jObject('a', 1),
    jObject('a', 2),
    jObject('a', 3)
  ),
  'n',
  'true'
)
""";

		var expression = new ExtendedExpression(expressionText);
		Action act = () => expression.Evaluate();
		act.Should().NotThrow();
		var result = expression.Evaluate();
		result.Should().BeOfType<List<object?>>();
		var list = (List<object?>)result;
		list.Count.Should().Be(3);

		// All items should be JObjects
		foreach (var item in list)
		{
			item.Should().BeOfType<JObject>();
		}
	}

	// Edge cases
	[Fact]
	public void Where_EmptyList_ReturnsEmptyList()
	{
		var expression = new ExtendedExpression("where(list(), 'n', 'n > 0')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEmpty();
	}

	[Fact]
	public void Where_NoneMatch_ReturnsEmptyList()
	{
		var expression = new ExtendedExpression("where(list(1, 2, 3), 'n', 'n > 10')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEmpty();
	}

	[Fact]
	public void Where_AllMatch_ReturnsAllItems()
	{
		var expression = new ExtendedExpression("where(list(1, 2, 3), 'n', 'n > 0')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
	}

	[Fact]
	public void Where_WithNulls_HandlesCorrectly()
	{
		var expression = new ExtendedExpression("where(list(1, null, 3, null, 5), 'n', 'n != null')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
	}

	// Lambda return value handling
	[Fact]
	public void Where_LambdaReturnsFalse_FiltersOut()
	{
		var expression = new ExtendedExpression("where(list(1, 2, 3), 'n', 'false')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEmpty();
	}

	[Fact]
	public void Where_LambdaReturnsNull_FiltersOut()
	{
		var expression = new ExtendedExpression("where(list(1, 2, 3), 'n', 'null')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEmpty();
	}

	[Fact]
	public void Where_LambdaReturnsNonBoolean_FiltersOut()
	{
		var expression = new ExtendedExpression("where(list(1, 2, 3), 'n', 'n')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEmpty();
	}

	// Error cases - First parameter
	[Fact]
	public void Where_NullFirstParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("where(null, 'n', 'n > 0')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*First*parameter must be an IEnumerable*");
	}

	[Fact]
	public void Where_NonEnumerableFirstParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("where(123, 'n', 'n > 0')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*First*parameter must be an IEnumerable*");
	}

	[Fact]
	public void Where_StringFirstParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("where('not a list', 'n', 'n > 0')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*First*parameter must be an IEnumerable*");
	}

	// Error cases - Second parameter
	[Fact]
	public void Where_NullSecondParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("where(list(1, 2, 3), null, 'n > 0')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*Second*parameter must be a string*");
	}

	[Fact]
	public void Where_NonStringSecondParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("where(list(1, 2, 3), 123, 'n > 0')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*Second*parameter must be a string*");
	}

	// Error cases - Third parameter
	[Fact]
	public void Where_NullThirdParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("where(list(1, 2, 3), 'n', null)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*Third*parameter must be a string*");
	}

	[Fact]
	public void Where_NonStringThirdParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("where(list(1, 2, 3), 'n', 456)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*Third*parameter must be a string*");
	}
}
