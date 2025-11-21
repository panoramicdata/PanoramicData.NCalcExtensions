using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class ConcatTests
{
	[Theory]
	[InlineData("concat(list(1, 2, 3))", new object[] { 1, 2, 3 })]
	[InlineData("concat(list(1), list(2, 3))", new object[] { 1, 2, 3 })]
	[InlineData("concat(list(1), list(2), list(3))", new object[] { 1, 2, 3 })]
	[InlineData("concat(list(1, 2), 3)", new object[] { 1, 2, 3 })]
	[InlineData("concat(1, list(2, 3))", new object[] { 1, 2, 3 })]
	[InlineData("concat(list(), list())", new object[] { })]
	[InlineData("concat(list(), list(1, 2, 3))", new object[] { 1, 2, 3 })]
	[InlineData("concat(1, 2, 3, 4)", new object[] { 1, 2, 3, 4 })]
	[InlineData("concat(list(1), list(2), list(3), list(4))", new object[] { 1, 2, 3, 4 })]
	[InlineData("concat(list(1, 2), 3, list(4, 5))", new object[] { 1, 2, 3, 4, 5 })]
	[InlineData("concat(list(1, 2, 2), list(2, 3, 3))", new object[] { 1, 2, 2, 2, 3, 3 })]
	[InlineData("concat(list(1, 2, 3, 4, 5), list(6, 7, 8, 9, 10))", new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
	[InlineData("concat()", new object[] { })]
	[InlineData("concat(42)", new object[] { 42 })]
	public void Concat_VariousScenarios_ReturnsExpected(string expression, object[] expected)
	{
		var result = new ExtendedExpression(expression).Evaluate();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
	}

	[Fact]
	public void Concat_Strings_Works()
	{
		var expression = new ExtendedExpression("concat(list('a', 'b'), list('c', 'd'))");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { "a", "b", "c", "d" }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void Concat_MixedTypes_Works()
	{
		var expression = new ExtendedExpression("concat(list(1, 'two'), list(3.0, true))");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(4);
	}

	[Fact]
	public void Concat_WithNulls_PreservesNulls()
	{
		var expression = new ExtendedExpression("concat(list(1, null), list(null, 2))");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(4);
		result![1].Should().BeNull();
		result[2].Should().BeNull();
	}

	[Fact]
	public void Concat_TypedListInt_Works()
	{
		var expression = new ExtendedExpression("concat(myList1, myList2)");
		expression.Parameters["myList1"] = new List<int> { 1, 2 };
		expression.Parameters["myList2"] = new List<int> { 3, 4 };
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(4);
	}

	[Fact]
	public void Concat_Arrays_Works()
	{
		var expression = new ExtendedExpression("concat(array1, array2)");
		expression.Parameters["array1"] = new[] { 1, 2 };
		expression.Parameters["array2"] = new[] { 3, 4 };
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(4);
	}

	[Fact]
	public void Concat_Chained_Works()
	{
		var expression = new ExtendedExpression("concat(concat(list(1), list(2)), list(3))");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void Concat_WithSelect_Works()
	{
		var expression = new ExtendedExpression("concat(select(list(1, 2), 'n', 'n * 2'), list(5, 6))");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { 2, 4, 5, 6 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void Concat_BooleanValues_Works()
	{
		var expression = new ExtendedExpression("concat(list(true, false), list(true))");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { true, false, true }, options => options.WithStrictOrdering());
	}
}
