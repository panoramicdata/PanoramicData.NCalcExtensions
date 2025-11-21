using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class DistinctTests : NCalcTest
{
	[Theory]
	[InlineData("list(1, 2, 3, 3, 3)", new object[] { 1, 2, 3 })]
	[InlineData("list()", new object[] { })]
	[InlineData("list(1, 2, 3, 4)", new object[] { 1, 2, 3, 4 })]
	[InlineData("list(5, 5, 5, 5)", new object[] { 5 })]
	[InlineData("list(42)", new object[] { 42 })]
	[InlineData("list(1, 2, 3, 4, 5, 1, 2, 3, 4, 5)", new object[] { 1, 2, 3, 4, 5 })]
	public void Distinct_Numbers_ReturnsExpected(string input, object[] expected)
	{
		var expression = new ExtendedExpression($"distinct({input})");
		expression.Evaluate().Should().BeEquivalentTo(expected);
	}

	[Theory]
	[InlineData("list('a', 'b', 'a', 'c', 'b')", new[] { "a", "b", "c" })]
	[InlineData("list('A', 'a', 'B', 'b')", new[] { "A", "a", "B", "b" })]
	public void Distinct_Strings_ReturnsExpected(string input, string[] expected)
	{
		var expression = new ExtendedExpression($"distinct({input})");
		expression.Evaluate().Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void Distinct_MixedTypes_Works()
	{
		var expression = new ExtendedExpression("distinct(list(1, 'two', 1, 'two', 3))");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
	}

	[Fact]
	public void Distinct_WithNulls_PreservesOneNull()
	{
		var expression = new ExtendedExpression("distinct(list(1, null, 2, null, 3))");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(4); // 1, null, 2, 3
	}

	[Fact]
	public void Distinct_Booleans_Works()
	{
		var expression = new ExtendedExpression("distinct(list(true, false, true, false, true))");
		expression.Evaluate().Should().BeEquivalentTo(new List<bool> { true, false });
	}

	[Fact]
	public void Distinct_Doubles_Works()
	{
		var expression = new ExtendedExpression("distinct(list(1.5, 2.5, 1.5, 3.5))");
		expression.Evaluate().Should().BeEquivalentTo(new List<double> { 1.5, 2.5, 3.5 });
	}

	[Fact]
	public void Distinct_PreservesOrder()
	{
		var expression = new ExtendedExpression("distinct(list(3, 1, 2, 3, 1))");
		expression.Evaluate().Should().BeEquivalentTo(new List<int> { 3, 1, 2 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void Distinct_WithVariables_Works()
	{
		var expression = new ExtendedExpression("distinct(myList)");
		expression.Parameters["myList"] = new List<object> { 1, 2, 2, 3, 3, 3 };
		expression.Evaluate().Should().BeEquivalentTo(new List<int> { 1, 2, 3 });
	}

	[Fact]
	public void Distinct_TypedArray_Works()
	{
		var expression = new ExtendedExpression("distinct(myArray)");
		expression.Parameters["myArray"] = new[] { "a", "b", "a", "c" };
		expression.Evaluate().Should().BeEquivalentTo(new List<string> { "a", "b", "c" });
	}

	[Theory]
	[InlineData("distinct(select(list(1, 2, 2, 3), 'n', 'n * 2'))", new object?[] { 2, 4, 6 })]
	[InlineData("count(distinct(list(1, 1, 2, 2, 3, 3)))", 3)]
	public void Distinct_Chained_Works(string expression, object expected)
		=> new ExtendedExpression(expression).Evaluate().Should().BeEquivalentTo(expected);

	[Fact]
	public void Distinct_ThenJoin_Works()
	{
		var expression = new ExtendedExpression("join(distinct(list('x', 'y', 'x', 'z')), ',')");
		expression.Evaluate().Should().Be("x,y,z");
	}

	[Theory]
	[InlineData("distinct(null)")]
	[InlineData("distinct(42)")]
	[InlineData("distinct()")]
	public void Distinct_InvalidInput_ThrowsException(string expression)
		=> new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should().Throw<Exception>();
}
