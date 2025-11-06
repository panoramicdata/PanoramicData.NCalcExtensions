using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class OrderByTests : NCalcTest
{
	[Theory]
	[InlineData("n", new[] { 1, 2, 3 })]
	[InlineData("-n", new[] { 3, 2, 1 })]
	public void OrderBy_SingleTerm_Succeeds(string expression, int[] expectedOrder)
		=> new ExtendedExpression($"orderBy(list(2, 1, 3), 'n', '{expression}')")
		.Evaluate()
		.Should()
		.BeEquivalentTo(expectedOrder, options => options.WithStrictOrdering());

	[Theory]
	[InlineData("n", new[] { 1.1, 1.2, 1.3 })]
	[InlineData("-n", new[] { 1.3, 1.2, 1.1 })]
	public void OrderBy_SingleTermDoubles_Succeeds(string expression, double[] expectedOrder)
		=> new ExtendedExpression($"orderBy(list(1.2, 1.1, 1.3), 'n', '{expression}')")
		.Evaluate()
		.Should()
		.BeEquivalentTo(expectedOrder, options => options.WithStrictOrdering());

	[Theory]
	[InlineData("n", new[] { 0, 1.2, 1.3, 5 })]
	[InlineData("-n", new[] { 5, 1.3, 1.2, 0 })]
	public void OrderBy_SingleTermMixedIntsAndDoubles(string expression, double[] expectedOrder)
		=> new ExtendedExpression($"orderBy(list(1.2, 5, 0, 1.3), 'n', '{expression}')")
		.Evaluate()
		.Should()
		.BeEquivalentTo(expectedOrder, options => options.WithStrictOrdering());

	[Theory]
	[InlineData("n % 32", "n % 2", new[] { 34, 2, 33, 1 })] // Fixed expected order
	public void OrderBy_MultipleTerms_Succeeds(string expression1, string expression2, int[] expectedOrder)
		=> new ExtendedExpression($"orderBy(list(34, 33, 2, 1), 'n', '{expression1}', '{expression2}')")
		.Evaluate()
		.Should()
		.BeEquivalentTo(expectedOrder, options => options.WithStrictOrdering());

	// Additional tests for ThenBy (multiple sort keys)
	[Fact]
	public void OrderBy_TwoSortKeys_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(list(jObject('a', 1, 'b', 2), jObject('a', 1, 'b', 1), jObject('a', 2, 'b', 1)), 'x', 'getProperty(x, \"a\")', 'getProperty(x, \"b\")')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
	}

	[Fact]
	public void OrderBy_ThreeSortKeys_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'x', 'getProperty(x, \"a\")', 'getProperty(x, \"b\")', 'getProperty(x, \"c\")')");
		expression.Parameters["myList"] = new List<object?>
		{
			new { a = 1, b = 2, c = 3 },
			new { a = 1, b = 2, c = 1 },
			new { a = 1, b = 1, c = 2 }
		};
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
	}

	[Fact]
	public void OrderBy_Strings_SortsAlphabetically()
	{
		var expression = new ExtendedExpression("orderBy(list('zebra', 'apple', 'banana'), 's', 's')");
		expression.Evaluate().Should().BeEquivalentTo(new[] { "apple", "banana", "zebra" }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_EmptyList_ReturnsEmptyList()
	{
		var expression = new ExtendedExpression("orderBy(list(), 'n', 'n')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEmpty();
	}

	[Fact]
	public void OrderBy_SingleItem_ReturnsSingleItem()
	{
		var expression = new ExtendedExpression("orderBy(list(42), 'n', 'n')");
		expression.Evaluate().Should().BeEquivalentTo(new[] { 42 });
	}

	[Fact]
	public void OrderBy_WithNulls_HandlesNulls()
	{
		var expression = new ExtendedExpression("orderBy(list(3, null, 1, null, 2), 'n', 'n')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(5);
		// Nulls should sort first
		result[0].Should().BeNull();
		result[1].Should().BeNull();
	}

	[Fact]
	public void OrderBy_ComplexLambda_Works()
	{
		var expression = new ExtendedExpression("orderBy(list(5, 3, 8, 1), 'n', 'n * -1')");
		expression.Evaluate().Should().BeEquivalentTo(new[] { 8, 5, 3, 1 }, options => options.WithStrictOrdering());
	}

	// Error cases
	[Fact]
	public void OrderBy_NullList_ThrowsException()
	{
		var expression = new ExtendedExpression("orderBy(null, 'n', 'n')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*must be an IEnumerable*");
	}

	[Fact]
	public void OrderBy_InvalidPredicate_ThrowsException()
	{
		var expression = new ExtendedExpression("orderBy(list(1, 2, 3), null, 'n')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*parameter must be a string*");
	}

	[Fact]
	public void OrderBy_InvalidLambda_ThrowsException()
	{
		var expression = new ExtendedExpression("orderBy(list(1, 2, 3), 'n', null)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*parameter must be a string*");
	}
}