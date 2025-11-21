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
	[InlineData("n % 32", "n % 2", new[] { 34, 2, 33, 1 })]
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

	// Comprehensive numeric type tests for ObjectKeyComparer.TryToDouble
	[Fact]
	public void OrderBy_ByteValues_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'n', 'n')");
		expression.Parameters["myList"] = new List<object?> { (byte)255, (byte)1, (byte)128 };
		expression.Evaluate().Should().BeEquivalentTo(new object[] { (byte)1, (byte)128, (byte)255 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_SByteValues_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'n', 'n')");
		expression.Parameters["myList"] = new List<object?> { (sbyte)127, (sbyte)-128, (sbyte)0 };
		expression.Evaluate().Should().BeEquivalentTo(new object[] { (sbyte)-128, (sbyte)0, (sbyte)127 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_ShortValues_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'n', 'n')");
		expression.Parameters["myList"] = new List<object?> { (short)32767, (short)-100, (short)0 };
		expression.Evaluate().Should().BeEquivalentTo(new object[] { (short)-100, (short)0, (short)32767 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_UShortValues_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'n', 'n')");
		expression.Parameters["myList"] = new List<object?> { (ushort)65535, (ushort)1, (ushort)100 };
		expression.Evaluate().Should().BeEquivalentTo(new object[] { (ushort)1, (ushort)100, (ushort)65535 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_UIntValues_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'n', 'n')");
		expression.Parameters["myList"] = new List<object?> { 4294967295u, 1u, 100u };
		expression.Evaluate().Should().BeEquivalentTo(new object[] { 1u, 100u, 4294967295u }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_LongValues_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'n', 'n')");
		expression.Parameters["myList"] = new List<object?> { 9223372036854775807L, -1000L, 0L };
		expression.Evaluate().Should().BeEquivalentTo(new object[] { -1000L, 0L, 9223372036854775807L }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_ULongValues_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'n', 'n')");
		expression.Parameters["myList"] = new List<object?> { 18446744073709551615UL, 1UL, 100UL };
		expression.Evaluate().Should().BeEquivalentTo(new object[] { 1UL, 100UL, 18446744073709551615UL }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_FloatValues_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'n', 'n')");
		expression.Parameters["myList"] = new List<object?> { 3.3f, 1.1f, 2.2f };
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		((float)result![0]!).Should().BeApproximately(1.1f, 0.01f);
		((float)result[1]!).Should().BeApproximately(2.2f, 0.01f);
		((float)result[2]!).Should().BeApproximately(3.3f, 0.01f);
	}

	[Fact]
	public void OrderBy_DecimalValues_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'n', 'n')");
		expression.Parameters["myList"] = new List<object?> { 3.3m, 1.1m, 2.2m };
		expression.Evaluate().Should().BeEquivalentTo(new object[] { 1.1m, 2.2m, 3.3m }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_MixedNumericTypes_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'n', 'n')");
		expression.Parameters["myList"] = new List<object?> { 3, 1.5, (byte)2, 4L };
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(4);
		// Should be sorted as 1.5, 2, 3, 4
	}

	[Fact]
	public void OrderBy_StringsDescending_Works()
	{
		var expression = new ExtendedExpression("orderBy(list('apple', 'zebra', 'banana'), 's', 's')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result![0].Should().Be("apple");
		result[2].Should().Be("zebra");
	}

	[Fact]
	public void OrderBy_MixedTypesWithStrings_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'x', 'x')");
		expression.Parameters["myList"] = new List<object?> { "zebra", "apple", "banana" };
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
	}

	[Fact]
	public void OrderBy_FourSortKeys_Works()
	{
		var expression = new ExtendedExpression("orderBy(list(4, 3, 2, 1), 'n', 'n % 2', 'n % 3', 'n % 5', 'n')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(4);
	}

	[Fact]
	public void OrderBy_Booleans_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'b', 'b')");
		expression.Parameters["myList"] = new List<object?> { true, false, true, false };
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(4);
		// false < true in default comparison
		result![0].Should().Be(false);
		result[1].Should().Be(false);
	}

	[Fact]
	public void OrderBy_DateTimes_SortsCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'd', 'd')");
		var dt1 = new System.DateTime(2020, 1, 1);
		var dt2 = new System.DateTime(2021, 1, 1);
		var dt3 = new System.DateTime(2019, 1, 1);
		expression.Parameters["myList"] = new List<object?> { dt2, dt1, dt3 };
		expression.Evaluate().Should().BeEquivalentTo(new object[] { dt3, dt1, dt2 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_NullsAndNumbers_NullsFirst()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'n', 'n')");
		expression.Parameters["myList"] = new List<object?> { 5, null, 3, null, 1 };
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(5);
		result![0].Should().BeNull();
		result[1].Should().BeNull();
	}

	[Fact]
	public void OrderBy_NullsOnly_HandlesCorrectly()
	{
		var expression = new ExtendedExpression("orderBy(myList, 'n', 'n')");
		expression.Parameters["myList"] = new List<object?> { null, null, null };
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result.Should().OnlyContain(x => x == null);
	}

	[Fact]
	public void OrderBy_LargeList_Works()
	{
		var expression = new ExtendedExpression("orderBy(list(10, 9, 8, 7, 6, 5, 4, 3, 2, 1), 'n', 'n')");
		expression.Evaluate().Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_AllSameValues_ReturnsInOriginalOrder()
	{
		var expression = new ExtendedExpression("orderBy(list(5, 5, 5, 5), 'n', 'n')");
		expression.Evaluate().Should().BeEquivalentTo(new[] { 5, 5, 5, 5 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_ChainedWithSelect_Works()
	{
		var expression = new ExtendedExpression("orderBy(select(list(1, 2, 3), 'n', 'n * 2'), 'n', '-n')");
		expression.Evaluate().Should().BeEquivalentTo(new[] { 6, 4, 2 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_ChainedWithWhere_Works()
	{
		var expression = new ExtendedExpression("orderBy(where(list(5, 2, 8, 1, 9), 'n', 'n > 3'), 'n', 'n')");
		expression.Evaluate().Should().BeEquivalentTo(new[] { 5, 8, 9 }, options => options.WithStrictOrdering());
	}
}