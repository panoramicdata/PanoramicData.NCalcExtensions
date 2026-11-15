using System.Collections.Generic;
using System.Linq;

namespace PanoramicData.NCalcExtensions.Test;

public partial class OrderByTests : NCalcTest
{
	[Fact]
	public void OrderBy_TwoSortKeys_SortsCorrectly()
		=> EvaluateToList(
			"orderBy(list(jObject('a', 1, 'b', 2), jObject('a', 1, 'b', 1), jObject('a', 2, 'b', 1)), 'x', 'getProperty(x, \"a\")', 'getProperty(x, \"b\")')",
			3);

	[Fact]
	public void OrderBy_ThreeSortKeys_SortsCorrectly()
		=> EvaluateToList(
			"orderBy(myList, 'x', 'getProperty(x, \"a\")', 'getProperty(x, \"b\")', 'getProperty(x, \"c\")')",
			new List<object?> { new { a = 1, b = 2, c = 3 }, new { a = 1, b = 2, c = 1 }, new { a = 1, b = 1, c = 2 } },
			3);

	[Fact]
	public void OrderBy_FourSortKeys_Works()
		=> EvaluateToList("orderBy(list(4, 3, 2, 1), 'n', 'n % 2', 'n % 3', 'n % 5', 'n')", 4);

	[Fact]
	public void OrderBy_MixedNumericTypes_SortsCorrectly()
		=> EvaluateToList("orderBy(myList, 'n', 'n')", new List<object?> { 3, 1.5, (byte)2, 4L }, 4);

	[Fact]
	public void OrderBy_MixedTypesWithStrings_SortsCorrectly()
		=> EvaluateToList("orderBy(myList, 'x', 'x')", new List<object?> { "zebra", "apple", "banana" }, 3);

	[Fact]
	public void OrderBy_StringsDescending_Works()
	{
		var result = EvaluateToList("orderBy(list('apple', 'zebra', 'banana'), 's', 's')", 3);
		result[0].Should().Be("apple");
		result[2].Should().Be("zebra");
	}

	[Fact]
	public void OrderBy_Booleans_SortsCorrectly()
	{
		// false sorts before true.
		var result = EvaluateToList("orderBy(myList, 'b', 'b')", new List<object?> { true, false, true, false }, 4);
		result[0].Should().Be(false);
		result[1].Should().Be(false);
	}

	// Nulls sort first.

	[Fact]
	public void OrderBy_WithNulls_HandlesNulls()
	{
		var result = EvaluateToList("orderBy(list(3, null, 1, null, 2), 'n', 'n')", 5);
		result[0].Should().BeNull();
		result[1].Should().BeNull();
	}

	[Fact]
	public void OrderBy_NullsAndNumbers_NullsFirst()
	{
		var result = EvaluateToList("orderBy(myList, 'n', 'n')", new List<object?> { 5, null, 3, null, 1 }, 5);
		result[0].Should().BeNull();
		result[1].Should().BeNull();
	}

	[Fact]
	public void OrderBy_NullsOnly_HandlesCorrectly()
		=> EvaluateToList("orderBy(myList, 'n', 'n')", new List<object?> { null, null, null }, 3)
			.Should().OnlyContain(value => value == null);
}
