using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class ConcatTests
{
	[Fact]
	public void OneListsOfInts_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(list(1, 2, 3))");
		var result = expression.Evaluate();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void TwoListsOfInts_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(list(1), list(2, 3))");
		var result = expression.Evaluate();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void ThreeListsOfInts_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(list(1), list(2), list(3))");
		var result = expression.Evaluate();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void ListOfIntsAddingOneObject_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(list(1, 2), 3)");
		var result = expression.Evaluate();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OneObjectAddingListOfInts_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(1, list(2, 3))");
		var result = expression.Evaluate();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	// Additional comprehensive tests
	[Fact]
	public void Concat_EmptyLists_ReturnsEmpty()
	{
		var expression = new ExtendedExpression("concat(list(), list())");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEmpty();
	}

	[Fact]
	public void Concat_EmptyWithNonEmpty_ReturnNonEmpty()
	{
		var expression = new ExtendedExpression("concat(list(), list(1, 2, 3))");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
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
	public void Concat_MultipleScalars_CreatesList()
	{
		var expression = new ExtendedExpression("concat(1, 2, 3, 4)");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { 1, 2, 3, 4 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void Concat_ScalarAtStart_Works()
	{
		var expression = new ExtendedExpression("concat('first', list('second', 'third'))");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { "first", "second", "third" }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void Concat_ScalarAtEnd_Works()
	{
		var expression = new ExtendedExpression("concat(list('first', 'second'), 'third')");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { "first", "second", "third" }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void Concat_ScalarInMiddle_Works()
	{
		var expression = new ExtendedExpression("concat(list(1, 2), 3, list(4, 5))");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { 1, 2, 3, 4, 5 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void Concat_FourLists_Works()
	{
		var expression = new ExtendedExpression("concat(list(1), list(2), list(3), list(4))");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { 1, 2, 3, 4 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void Concat_WithDuplicates_PreservesDuplicates()
	{
		var expression = new ExtendedExpression("concat(list(1, 2, 2), list(2, 3, 3))");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { 1, 2, 2, 2, 3, 3 }, options => options.WithStrictOrdering());
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
	public void Concat_LargeLists_Works()
	{
		var expression = new ExtendedExpression("concat(list(1, 2, 3, 4, 5), list(6, 7, 8, 9, 10))");
		expression.Evaluate().Should().BeEquivalentTo(
			new List<object> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
			options => options.WithStrictOrdering());
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

	// Error cases
	[Fact]
	public void Concat_NoParameters_ReturnsEmptyList()
	{
		var expression = new ExtendedExpression("concat()");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEmpty();
	}

	[Fact]
	public void Concat_SingleScalar_ReturnsAsList()
	{
		var expression = new ExtendedExpression("concat(42)");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { 42 });
	}

	[Fact]
	public void Concat_BooleanValues_Works()
	{
		var expression = new ExtendedExpression("concat(list(true, false), list(true))");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { true, false, true }, options => options.WithStrictOrdering());
	}
}
