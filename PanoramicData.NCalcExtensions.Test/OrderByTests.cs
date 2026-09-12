using System.Collections.Generic;
using System.Linq;

namespace PanoramicData.NCalcExtensions.Test;

public class OrderByTests : NCalcTest
{
	/// <summary>
	/// Ten people whose surnames repeat, so that a second sort key has work to do.
	/// </summary>
	private static List<object?> People =>
	[
		JObject.Parse("{\"Name\":\"James\",\"Surname\":\"Chen\"}"),
		JObject.Parse("{\"Name\":\"Sarah\",\"Surname\":\"Martinez\"}"),
		JObject.Parse("{\"Name\":\"David\",\"Surname\":\"Thompson\"}"),
		JObject.Parse("{\"Name\":\"Emily\",\"Surname\":\"Anderson\"}"),
		JObject.Parse("{\"Name\":\"Michael\",\"Surname\":\"Garcia\"}"),
		JObject.Parse("{\"Name\":\"Jennifer\",\"Surname\":\"Chen\"}"),
		JObject.Parse("{\"Name\":\"Robert\",\"Surname\":\"Thompson\"}"),
		JObject.Parse("{\"Name\":\"Lisa\",\"Surname\":\"Brown\"}"),
		JObject.Parse("{\"Name\":\"Christopher\",\"Surname\":\"Martinez\"}"),
		JObject.Parse("{\"Name\":\"Amanda\",\"Surname\":\"Thompson\"}"),
	];

	// Regression tests for a closure capture bug in multi-key orderBy: with the bug every key
	// read the same lambda, so the second key either did nothing or repeated the first.

	[Theory]
	[InlineData("Surname", "Name", "Emily,Lisa,James,Jennifer,Michael,Christopher,Sarah,Amanda,David,Robert")]
	[InlineData("Name", "Surname", "Amanda,Christopher,David,Emily,James,Jennifer,Lisa,Michael,Robert,Sarah")]
	public void OrderBy_JObjectList_ByTwoJPathKeys_CorrectOrder(string firstKey, string secondKey, string expectedNames)
	{
		var expressionText = "orderBy(People, 'person', 'jPath(person, \\'" + firstKey + "\\')', 'jPath(person, \\'" + secondKey + "\\')')";
		var result = Test(expressionText, "People", People) as List<object?>;

		result.Should().NotBeNull();
		result.Should().HaveCount(10);
		result!.Cast<JObject>()
			.Select(person => person["Name"]!.Value<string>())
			.Should().Equal(expectedNames.Split(','));
	}

	[Theory]
	[InlineData("n", new[] { 1, 2, 3 })]
	[InlineData("-n", new[] { 3, 2, 1 })]
	public void OrderBy_SingleTerm_Succeeds(string expression, int[] expectedOrder)
		=> Test($"orderBy(list(2, 1, 3), 'n', '{expression}')")
			.Should().BeEquivalentTo(expectedOrder, options => options.WithStrictOrdering());

	[Theory]
	[InlineData("n", new[] { 1.1, 1.2, 1.3 })]
	[InlineData("-n", new[] { 1.3, 1.2, 1.1 })]
	public void OrderBy_SingleTermDoubles_Succeeds(string expression, double[] expectedOrder)
		=> Test($"orderBy(list(1.2, 1.1, 1.3), 'n', '{expression}')")
			.Should().BeEquivalentTo(expectedOrder, options => options.WithStrictOrdering());

	[Theory]
	[InlineData("n", new[] { 0, 1.2, 1.3, 5 })]
	[InlineData("-n", new[] { 5, 1.3, 1.2, 0 })]
	public void OrderBy_SingleTermMixedIntsAndDoubles(string expression, double[] expectedOrder)
		=> Test($"orderBy(list(1.2, 5, 0, 1.3), 'n', '{expression}')")
			.Should().BeEquivalentTo(expectedOrder, options => options.WithStrictOrdering());

	[Theory]
	[InlineData("n % 32", "n % 2", new[] { 33, 1, 34, 2 })]
	public void OrderBy_MultipleTerms_Succeeds(string expression1, string expression2, int[] expectedOrder)
		=> Test($"orderBy(list(34, 33, 2, 1), 'n', '{expression1}', '{expression2}')")
			.Should().BeEquivalentTo(expectedOrder, options => options.WithStrictOrdering());

	[Fact]
	public void OrderBy_Strings_SortsAlphabetically()
		=> Test("orderBy(list('zebra', 'apple', 'banana'), 's', 's')")
			.Should().BeEquivalentTo(new[] { "apple", "banana", "zebra" }, options => options.WithStrictOrdering());

	[Fact]
	public void OrderBy_ComplexLambda_Works()
		=> Test("orderBy(list(5, 3, 8, 1), 'n', 'n * -1')")
			.Should().BeEquivalentTo(new[] { 8, 5, 3, 1 }, options => options.WithStrictOrdering());

	[Fact]
	public void OrderBy_SingleItem_ReturnsSingleItem()
		=> Test("orderBy(list(42), 'n', 'n')").Should().BeEquivalentTo(new[] { 42 });

	[Fact]
	public void OrderBy_LargeList_Works()
		=> Test("orderBy(list(10, 9, 8, 7, 6, 5, 4, 3, 2, 1), 'n', 'n')")
			.Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, options => options.WithStrictOrdering());

	[Fact]
	public void OrderBy_AllSameValues_ReturnsInOriginalOrder()
		=> Test("orderBy(list(5, 5, 5, 5), 'n', 'n')")
			.Should().BeEquivalentTo(new[] { 5, 5, 5, 5 }, options => options.WithStrictOrdering());

	[Fact]
	public void OrderBy_ChainedWithSelect_Works()
		=> Test("orderBy(select(list(1, 2, 3), 'n', 'n * 2'), 'n', '-n')")
			.Should().BeEquivalentTo(new[] { 6, 4, 2 }, options => options.WithStrictOrdering());

	[Fact]
	public void OrderBy_ChainedWithWhere_Works()
		=> Test("orderBy(where(list(5, 2, 8, 1, 9), 'n', 'n > 3'), 'n', 'n')")
			.Should().BeEquivalentTo(new[] { 5, 8, 9 }, options => options.WithStrictOrdering());

	[Fact]
	public void OrderBy_EmptyList_ReturnsEmptyList()
		=> EvaluateToList("orderBy(list(), 'n', 'n')", 0).Should().BeEmpty();

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

	// Every numeric type ObjectKeyComparer knows how to widen.

	[Fact]
	public void OrderBy_ByteValues_SortsCorrectly()
		=> ShouldSortTo([(byte)255, (byte)1, (byte)128], (byte)1, (byte)128, (byte)255);

	[Fact]
	public void OrderBy_SByteValues_SortsCorrectly()
		=> ShouldSortTo([(sbyte)127, (sbyte)-128, (sbyte)0], (sbyte)-128, (sbyte)0, (sbyte)127);

	[Fact]
	public void OrderBy_ShortValues_SortsCorrectly()
		=> ShouldSortTo([(short)32767, (short)-100, (short)0], (short)-100, (short)0, (short)32767);

	[Fact]
	public void OrderBy_UShortValues_SortsCorrectly()
		=> ShouldSortTo([(ushort)65535, (ushort)1, (ushort)100], (ushort)1, (ushort)100, (ushort)65535);

	[Fact]
	public void OrderBy_UIntValues_SortsCorrectly()
		=> ShouldSortTo([4294967295u, 1u, 100u], 1u, 100u, 4294967295u);

	[Fact]
	public void OrderBy_LongValues_SortsCorrectly()
		=> ShouldSortTo([9223372036854775807L, -1000L, 0L], -1000L, 0L, 9223372036854775807L);

	[Fact]
	public void OrderBy_ULongValues_SortsCorrectly()
		=> ShouldSortTo([18446744073709551615UL, 1UL, 100UL], 1UL, 100UL, 18446744073709551615UL);

	[Fact]
	public void OrderBy_DecimalValues_SortsCorrectly()
		=> ShouldSortTo([3.3m, 1.1m, 2.2m], 1.1m, 2.2m, 3.3m);

	[Fact]
	public void OrderBy_DateTimes_SortsCorrectly()
	{
		var dt1 = new DateTime(2020, 1, 1);
		var dt2 = new DateTime(2021, 1, 1);
		var dt3 = new DateTime(2019, 1, 1);

		ShouldSortTo([dt2, dt1, dt3], dt3, dt1, dt2);
	}

	[Fact]
	public void OrderBy_FloatValues_SortsCorrectly()
	{
		var result = EvaluateToList("orderBy(myList, 'n', 'n')", new List<object?> { 3.3f, 1.1f, 2.2f }, 3);
		((float)result[0]!).Should().BeApproximately(1.1f, 0.01f);
		((float)result[1]!).Should().BeApproximately(2.2f, 0.01f);
		((float)result[2]!).Should().BeApproximately(3.3f, 0.01f);
	}

	// Error cases

	[Theory]
	[InlineData("orderBy(null, 'n', 'n')", "*must be an IEnumerable*")]
	[InlineData("orderBy(list(1, 2, 3), null, 'n')", "*parameter must be a string*")]
	[InlineData("orderBy(list(1, 2, 3), 'n', null)", "*parameter must be a string*")]
	public void OrderBy_WithInvalidArguments_ThrowsFormatException(string expressionText, string messagePattern)
		=> TestShouldThrow<FormatException>(expressionText, messagePattern);

	/// <summary>
	/// Asserts that ordering <paramref name="values"/> by their own value yields exactly
	/// <paramref name="expectedOrder"/>.
	/// </summary>
	private static void ShouldSortTo(List<object?> values, params object[] expectedOrder)
		=> Test("orderBy(myList, 'n', 'n')", "myList", values)
			.Should().BeEquivalentTo(expectedOrder, options => options.WithStrictOrdering());

	/// <summary>
	/// Evaluates <paramref name="expressionText"/> to a list of <paramref name="expectedCount"/>
	/// entries.
	/// </summary>
	private static List<object?> EvaluateToList(string expressionText, int expectedCount)
		=> AsList(Test(expressionText), expectedCount);

	/// <summary>
	/// As above, with the list under test supplied as the "myList" parameter.
	/// </summary>
	private static List<object?> EvaluateToList(string expressionText, object parameterValue, int expectedCount)
		=> AsList(Test(expressionText, "myList", parameterValue), expectedCount);

	private static List<object?> AsList(object? result, int expectedCount)
	{
		var list = result as List<object?>;
		list.Should().NotBeNull();
		list.Should().HaveCount(expectedCount);
		return list!;
	}
}
