using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class CountByTests
{
	/// <summary>
	/// Evaluates <paramref name="expression"/>, requiring it to produce a JObject.
	/// </summary>
	private static JObject EvaluateToJObject(ExtendedExpression expression)
	{
		var result = expression.Evaluate();
		result.Should().BeOfType<JObject>();
		return (JObject)result;
	}

	/// <summary>
	/// Evaluates <paramref name="expressionText"/>, requiring it to produce a JArray.
	/// </summary>
	private static JArray EvaluateToJArray(string expressionText)
	{
		var result = new ExtendedExpression(expressionText).Evaluate();
		result.Should().BeOfType<JArray>();
		return (JArray)result;
	}

	/// <summary>
	/// The counts by group name from a countBy() result.
	/// </summary>
	private static Dictionary<string, int> CountsOf(JObject result)
		=> result.Properties()
			.ToDictionary(property => property.Name, property => property.Value.Value<int>(), StringComparer.Ordinal);

	private static Dictionary<string, int> CountsOf(string expressionText)
		=> CountsOf(EvaluateToJObject(new ExtendedExpression(expressionText)));

	/// <summary>
	/// Asserts that <paramref name="expressionText"/> fails with a FormatException matching
	/// <paramref name="messagePattern"/>.
	/// </summary>
	private static void ShouldFailWith(string expressionText, string messagePattern)
		=> new ExtendedExpression(expressionText)
			.Invoking(expression => expression.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage(messagePattern);

	[Theory]
	[InlineData("countBy(list(1, 2, 2, 3, 3, 3, 4), 'n', 'toLower(toString(n > 1))')", "{\"false\":1,\"true\":6}")]
	[InlineData("countBy(list(1, 2, 2, 3, 3, 3, 4), 'n', 'toString(n)')", "{\"1\":1,\"2\":2,\"3\":3,\"4\":1}")]
	public void CountBy_ReturnsExpectedResult(string expressionText, string expectedResult)
		=> EvaluateToJObject(new ExtendedExpression(expressionText))
			.ToString()
			.ReplaceLineEndings(string.Empty)
			.Replace(" ", string.Empty, StringComparison.Ordinal)
			.Replace("\t", string.Empty, StringComparison.Ordinal)
			.Should().Be(expectedResult);

	// Test empty list
	[Fact]
	public void CountBy_EmptyList_ReturnsEmptyJObject()
		=> EvaluateToJObject(new ExtendedExpression("countBy(list(), 'n', 'toString(n)')"))
			.Count.Should().Be(0);

	// Test all same values
	[Fact]
	public void CountBy_AllSameValues_ReturnsSingleGroup()
		=> CountsOf("countBy(list(1, 1, 1, 1), 'n', 'toString(n)')")["1"].Should().Be(4);

	// Test with strings
	[Fact]
	public void CountBy_Strings_GroupsCorrectly()
	{
		var counts = CountsOf("countBy(list('apple', 'banana', 'apple', 'cherry', 'banana', 'apple'), 'n', 'n')");
		counts["apple"].Should().Be(3);
		counts["banana"].Should().Be(2);
		counts["cherry"].Should().Be(1);
	}

	// Test with complex lambda
	[Fact]
	public void CountBy_ComplexLambda_GroupsCorrectly()
	{
		var counts = CountsOf("countBy(list(1, 2, 3, 4, 5, 6), 'n', 'if(n % 2 == 0, \"even\", \"odd\")')");
		counts["even"].Should().Be(3);
		counts["odd"].Should().Be(3);
	}

	// Test with null in list
	[Fact]
	public void CountBy_ListWithNull_HandlesNullGroup()
	{
		var counts = CountsOf("countBy(list(1, null, 2, null, 3), 'n', 'if(isNull(n), \"null\", toString(n))')");
		counts["null"].Should().Be(2);
		counts["1"].Should().Be(1);
	}

	// Test grouping by length
	[Fact]
	public void CountBy_GroupByLength_Works()
	{
		var counts = CountsOf("countBy(list('a', 'bb', 'ccc', 'dd', 'e'), 'n', 'toString(length(n))')");
		counts["1"].Should().Be(2);
		counts["2"].Should().Be(2);
		counts["3"].Should().Be(1);
	}

	// Test with number ranges
	[Fact]
	public void CountBy_NumberRanges_GroupsCorrectly()
	{
		var counts = CountsOf("countBy(list(1, 5, 10, 15, 20, 25, 30), 'n', 'if(n < 10, \"small\", if(n < 20, \"medium\", \"large\"))')");
		counts["small"].Should().Be(2);
		counts["medium"].Should().Be(2);
		counts["large"].Should().Be(3);
	}

	// Test single item
	[Fact]
	public void CountBy_SingleItem_ReturnsSingleGroup()
		=> CountsOf("countBy(list(42), 'n', 'toString(n)')")["42"].Should().Be(1);

	// Test with boolean grouping
	[Fact]
	public void CountBy_BooleanGroups_Works()
	{
		var counts = CountsOf("countBy(list(1, 2, 3, 4, 5), 'n', 'toString(n > 3)')");
		counts["False"].Should().Be(3);
		counts["True"].Should().Be(2);
	}

	// Test reusing same group keys
	[Fact]
	public void CountBy_ReusedKeys_Accumulates()
	{
		var counts = CountsOf("countBy(list(1, 11, 2, 12, 3, 13), 'n', 'toString(n % 10)')");
		counts["1"].Should().Be(2);
		counts["2"].Should().Be(2);
		counts["3"].Should().Be(2);
	}

	// Error cases
	[Fact]
	public void CountBy_NullList_ThrowsException()
		=> ShouldFailWith("countBy(null, 'n', 'toString(n)')", "*requires IEnumerable*");

	[Fact]
	public void CountBy_NullPredicate_ThrowsException()
		=> ShouldFailWith("countBy(list(1, 2, 3), null, 'toString(n)')", "*Second*parameter must be a string*");

	[Fact]
	public void CountBy_NullLambda_ThrowsException()
		=> ShouldFailWith("countBy(list(1, 2, 3), 'n', null)", "*Third*parameter must be a string*");

	[Fact]
	public void CountBy_LambdaReturnsNonString_ThrowsException()
		=> ShouldFailWith("countBy(list(1, 2, 3), 'n', 'n')", "*should evaluate to a string*");

	[Fact]
	public void CountBy_LambdaReturnsNull_ThrowsException()
		=> ShouldFailWith("countBy(list(1, null, 3), 'n', 'n')", "*should evaluate to a string*");

	// Test with variables
	[Fact]
	public void CountBy_WithVariables_Works()
	{
		var expression = new ExtendedExpression("countBy(myList, myPredicate, myLambda)");
		expression.Parameters["myList"] = new List<int> { 1, 2, 2, 3, 3, 3 };
		expression.Parameters["myPredicate"] = "n";
		expression.Parameters["myLambda"] = "toString(n)";

		var counts = CountsOf(EvaluateToJObject(expression));
		counts["1"].Should().Be(1);
		counts["2"].Should().Be(2);
		counts["3"].Should().Be(3);
	}

	// Test with typed List<int>
	[Fact]
	public void CountBy_WithTypedListInt_Works()
	{
		var expression = new ExtendedExpression("countBy(myList, 'n', 'toString(n)')");
		expression.Parameters["myList"] = new List<int> { 1, 2, 2, 3, 3, 3 };

		var counts = CountsOf(EvaluateToJObject(expression));
		counts["1"].Should().Be(1);
		counts["2"].Should().Be(2);
		counts["3"].Should().Be(3);
	}

	// Test with typed List<string>
	[Fact]
	public void CountBy_WithTypedListString_Works()
	{
		var expression = new ExtendedExpression("countBy(myList, 's', 's')");
		expression.Parameters["myList"] = new List<string> { "apple", "banana", "apple" };

		var counts = CountsOf(EvaluateToJObject(expression));
		counts["apple"].Should().Be(2);
		counts["banana"].Should().Be(1);
	}

	// Test with array
	[Fact]
	public void CountBy_WithArray_Works()
	{
		var expression = new ExtendedExpression("countBy(myArray, 'n', 'toString(n)')");
		expression.Parameters["myArray"] = new[] { 1, 2, 2, 3 };

		var counts = CountsOf(EvaluateToJObject(expression));
		counts["1"].Should().Be(1);
		counts["2"].Should().Be(2);
	}

	// Test case sensitivity
	[Fact]
	public void CountBy_CaseSensitive_TreatsAsDifferent()
		=> EvaluateToJObject(new ExtendedExpression("countBy(list('Apple', 'apple', 'APPLE'), 'n', 'n')"))
			.Count.Should().Be(3); // All three are different keys

	// Test with special characters in keys
	[Fact]
	public void CountBy_SpecialCharactersInKeys_Works()
	{
		var counts = CountsOf("countBy(list('a-b', 'a-b', 'c_d'), 'n', 'n')");
		counts["a-b"].Should().Be(2);
		counts["c_d"].Should().Be(1);
	}

	// AC-01: Default behavior (no fourth parameter) returns JObject
	[Fact]
	public void CountBy_NoFormatParameter_ReturnsJObject()
		=> EvaluateToJObject(new ExtendedExpression("countBy(list(1, 2, 2, 3), 'n', 'toString(n)')"))
			.Should().NotBeNull();

	// AC-02: JArray format returns array of {name, count} objects
	[Theory]
	[InlineData("'JArray'")]
	[InlineData("'jarray'")]
	[InlineData("'JARRAY'")]
	public void CountBy_JArrayFormat_ReturnsJArray(string formatParam)
	{
		var array = EvaluateToJArray($"countBy(list(1, 2, 2, 3, 3, 3), 'n', 'toString(n)', {formatParam})");
		array.Should().HaveCount(3);
		array[0]["name"]!.Value<string>().Should().Be("1");
		array[0]["count"]!.Value<int>().Should().Be(1);
		array[1]["name"]!.Value<string>().Should().Be("2");
		array[1]["count"]!.Value<int>().Should().Be(2);
		array[2]["name"]!.Value<string>().Should().Be("3");
		array[2]["count"]!.Value<int>().Should().Be(3);
	}

	// AC-03: Explicit JObject format returns flat JObject
	[Theory]
	[InlineData("'JObject'")]
	[InlineData("'jobject'")]
	[InlineData("'JOBJECT'")]
	public void CountBy_JObjectFormat_ReturnsJObject(string formatParam)
	{
		var counts = CountsOf($"countBy(list(1, 2, 2, 3), 'n', 'toString(n)', {formatParam})");
		counts["1"].Should().Be(1);
		counts["2"].Should().Be(2);
		counts["3"].Should().Be(1);
	}

	// AC-05: Insertion order preserved in JArray format
	[Fact]
	public void CountBy_JArrayFormat_PreservesInsertionOrder()
	{
		var array = EvaluateToJArray("countBy(list('c', 'a', 'b', 'a', 'c', 'c'), 'n', 'n', 'JArray')");
		array[0]["name"]!.Value<string>().Should().Be("c");
		array[1]["name"]!.Value<string>().Should().Be("a");
		array[2]["name"]!.Value<string>().Should().Be("b");
	}

	// Unrecognized format falls back to JObject
	[Fact]
	public void CountBy_UnrecognizedFormat_ReturnsJObject()
		=> EvaluateToJObject(new ExtendedExpression("countBy(list(1, 2), 'n', 'toString(n)', 'unknown')"))
			.Should().NotBeNull();

	// JArray format with empty list returns empty array
	[Fact]
	public void CountBy_JArrayFormat_EmptyList_ReturnsEmptyArray()
		=> EvaluateToJArray("countBy(list(), 'n', 'toString(n)', 'JArray')").Should().BeEmpty();
}
