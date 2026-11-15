using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

/// <summary>
/// Tests to verify that JValue objects are properly unwrapped to their underlying values
/// across all functions that work with JArrays
/// </summary>
public partial class JValueUnwrappingTests : NCalcTest
{
	#region select Tests

	[Fact]
	public void Select_JArrayWithStrings_ReturnsListOfStrings()
	{
		var expression = new ExtendedExpression("select(jArray('a', 'b', 'c'), 's', 's')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result.Should().AllBeOfType<string>();
	}

	[Fact]
	public void Select_JArrayWithEmptyStrings_ReturnsEmptyStrings()
	{
		var expression = new ExtendedExpression("select(jArray('a', '', 'c'), 's', 's')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result![1].Should().Be("");
	}

	[Fact]
	public void Select_JArrayWithNumbers_ReturnsNumbers()
	{
		var expression = new ExtendedExpression("select(jArray(1, 2, 3), 'n', 'n')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result![0].Should().Be(1L);
		result[1].Should().Be(2L);
	}

	[Fact]
	public void Select_JArrayWithLambdaReturningString_ReturnsStrings()
	{
		var expression = new ExtendedExpression("select(jArray(1, 2, 3), 'n', 'toString(n)')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().AllBeOfType<string>();
	}

	[Fact]
	public void Select_JArrayItemsInLambda_ReturnsUnwrappedValues()
	{
		var expression = new ExtendedExpression("select(jArray('a', 'b'), 's', 'toUpper(s)')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new[] { "A", "B" });
	}

	#endregion

	#region min/max Tests

	[Fact]
	public void Min_JArrayOfNumbers_ReturnsNumber()
	{
		var expression = new ExtendedExpression("min(jArray(3, 1, 4, 1, 5))");
		var result = expression.Evaluate();
		result.Should().Be(1);
	}

	[Fact]
	public void Min_JArrayOfStrings_ReturnsString()
	{
		var expression = new ExtendedExpression("min(jArray('zebra', 'apple', 'banana'))");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("apple");
	}

	[Fact]
	public void Max_JArrayOfNumbers_ReturnsNumber()
	{
		var expression = new ExtendedExpression("max(jArray(3, 1, 4, 1, 5))");
		var result = expression.Evaluate();
		result.Should().Be(5);
	}

	[Fact]
	public void Max_JArrayOfStrings_ReturnsString()
	{
		var expression = new ExtendedExpression("max(jArray('zebra', 'apple', 'banana'))");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("zebra");
	}

	[Fact]
	public void Min_JArrayWithLambda_ReturnsCorrectValue()
	{
		var expression = new ExtendedExpression("min(jArray(5, 3, 8, 1), 'n', 'n * 2')");
		var result = expression.Evaluate();
		result.Should().Be(2); // 1 * 2 = 2
	}

	[Fact]
	public void Max_JArrayWithLambda_ReturnsCorrectValue()
	{
		var expression = new ExtendedExpression("max(jArray(5, 3, 8, 1), 'n', 'n * 2')");
		var result = expression.Evaluate();
		result.Should().Be(16); // 8 * 2 = 16
	}

	#endregion
}
