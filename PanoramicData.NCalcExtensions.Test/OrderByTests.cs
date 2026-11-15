using System.Collections.Generic;
using System.Linq;

namespace PanoramicData.NCalcExtensions.Test;

public partial class OrderByTests : NCalcTest
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
}
