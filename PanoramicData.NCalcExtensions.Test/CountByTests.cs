using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class CountByTests
{
	[Theory]
	[InlineData("countBy(list(1, 2, 2, 3, 3, 3, 4), 'n', 'toLower(toString(n > 1))')", "{\"false\":1,\"true\":6}")]
	[InlineData("countBy(list(1, 2, 2, 3, 3, 3, 4), 'n', 'toString(n)')", "{\"1\":1,\"2\":2,\"3\":3,\"4\":1}")]
	public void CountBy_ReturnsExpectedResult(string expressionText, string expectedResult)
	{
		var expression = new ExtendedExpression(expressionText);

		var result = expression.Evaluate();
		result.Should().BeOfType<JObject>();
		result.Should().NotBeNull();
		result.ToString()!
			.ReplaceLineEndings(string.Empty)
			.Replace(" ", string.Empty, StringComparison.Ordinal)
			.Replace("\t", string.Empty, StringComparison.Ordinal)
			.Should().Be(expectedResult);
	}

	// Test empty list
	[Fact]
	public void CountBy_EmptyList_ReturnsEmptyJObject()
	{
		var expression = new ExtendedExpression("countBy(list(), 'n', 'toString(n)')");
		var result = expression.Evaluate();
		result.Should().BeOfType<JObject>();
		((JObject)result).Count.Should().Be(0);
	}

	// Test all same values
	[Fact]
	public void CountBy_AllSameValues_ReturnsSingleGroup()
	{
		var expression = new ExtendedExpression("countBy(list(1, 1, 1, 1), 'n', 'toString(n)')");
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!["1"]!.Value<int>().Should().Be(4);
	}

	// Test with strings
	[Fact]
	public void CountBy_Strings_GroupsCorrectly()
	{
		var expression = new ExtendedExpression("countBy(list('apple', 'banana', 'apple', 'cherry', 'banana', 'apple'), 'n', 'n')");
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!["apple"]!.Value<int>().Should().Be(3);
		result["banana"]!.Value<int>().Should().Be(2);
		result["cherry"]!.Value<int>().Should().Be(1);
	}

	// Test with complex lambda
	[Fact]
	public void CountBy_ComplexLambda_GroupsCorrectly()
	{
		var expression = new ExtendedExpression("countBy(list(1, 2, 3, 4, 5, 6), 'n', 'if(n % 2 == 0, \"even\", \"odd\")')");
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!["even"]!.Value<int>().Should().Be(3);
		result["odd"]!.Value<int>().Should().Be(3);
	}

	// Test with null in list
	[Fact]
	public void CountBy_ListWithNull_HandlesNullGroup()
	{
		var expression = new ExtendedExpression("countBy(list(1, null, 2, null, 3), 'n', 'if(isNull(n), \"null\", toString(n))')");
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!["null"]!.Value<int>().Should().Be(2);
		result["1"]!.Value<int>().Should().Be(1);
	}

	// Test grouping by length
	[Fact]
	public void CountBy_GroupByLength_Works()
	{
		var expression = new ExtendedExpression("countBy(list('a', 'bb', 'ccc', 'dd', 'e'), 'n', 'toString(length(n))')");
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!["1"]!.Value<int>().Should().Be(2);
		result["2"]!.Value<int>().Should().Be(2);
		result["3"]!.Value<int>().Should().Be(1);
	}

	// Test with number ranges
	[Fact]
	public void CountBy_NumberRanges_GroupsCorrectly()
	{
		var expression = new ExtendedExpression("countBy(list(1, 5, 10, 15, 20, 25, 30), 'n', 'if(n < 10, \"small\", if(n < 20, \"medium\", \"large\"))')");
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!["small"]!.Value<int>().Should().Be(2);
		result["medium"]!.Value<int>().Should().Be(2);
		result["large"]!.Value<int>().Should().Be(3);
	}

	// Test single item
	[Fact]
	public void CountBy_SingleItem_ReturnsSingleGroup()
	{
		var expression = new ExtendedExpression("countBy(list(42), 'n', 'toString(n)')");
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!["42"]!.Value<int>().Should().Be(1);
	}

	// Test with boolean grouping
	[Fact]
	public void CountBy_BooleanGroups_Works()
	{
		var expression = new ExtendedExpression("countBy(list(1, 2, 3, 4, 5), 'n', 'toString(n > 3)')");
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!["False"]!.Value<int>().Should().Be(3);
		result["True"]!.Value<int>().Should().Be(2);
	}

	// Test reusing same group keys
	[Fact]
	public void CountBy_ReusedKeys_Accumulates()
	{
		var expression = new ExtendedExpression("countBy(list(1, 11, 2, 12, 3, 13), 'n', 'toString(n % 10)')");
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!["1"]!.Value<int>().Should().Be(2);
		result["2"]!.Value<int>().Should().Be(2);
		result["3"]!.Value<int>().Should().Be(2);
	}

	// Error cases
	[Fact]
	public void CountBy_NullList_ThrowsException()
	{
		var expression = new ExtendedExpression("countBy(null, 'n', 'toString(n)')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*requires IEnumerable*");
	}

	[Fact]
	public void CountBy_NullPredicate_ThrowsException()
	{
		var expression = new ExtendedExpression("countBy(list(1, 2, 3), null, 'toString(n)')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*Second*parameter must be a string*");
	}

	[Fact]
	public void CountBy_NullLambda_ThrowsException()
	{
		var expression = new ExtendedExpression("countBy(list(1, 2, 3), 'n', null)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*Third*parameter must be a string*");
	}

	[Fact]
	public void CountBy_LambdaReturnsNonString_ThrowsException()
	{
		var expression = new ExtendedExpression("countBy(list(1, 2, 3), 'n', 'n')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*should evaluate to a string*");
	}

	[Fact]
	public void CountBy_LambdaReturnsNull_ThrowsException()
	{
		var expression = new ExtendedExpression("countBy(list(1, null, 3), 'n', 'n')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*should evaluate to a string*");
	}

	// Test with variables
	[Fact]
	public void CountBy_WithVariables_Works()
	{
		var expression = new ExtendedExpression("countBy(myList, myPredicate, myLambda)");
		expression.Parameters["myList"] = new List<int> { 1, 2, 2, 3, 3, 3 };
		expression.Parameters["myPredicate"] = "n";
		expression.Parameters["myLambda"] = "toString(n)";
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!["1"]!.Value<int>().Should().Be(1);
		result["2"]!.Value<int>().Should().Be(2);
		result["3"]!.Value<int>().Should().Be(3);
	}

	// Test with typed List<int>
	[Fact]
	public void CountBy_WithTypedListInt_Works()
	{
		var expression = new ExtendedExpression("countBy(myList, 'n', 'toString(n)')");
		expression.Parameters["myList"] = new List<int> { 1, 2, 2, 3, 3, 3 };
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!["1"]!.Value<int>().Should().Be(1);
		result["2"]!.Value<int>().Should().Be(2);
		result["3"]!.Value<int>().Should().Be(3);
	}

	// Test with typed List<string>
	[Fact]
	public void CountBy_WithTypedListString_Works()
	{
		var expression = new ExtendedExpression("countBy(myList, 's', 's')");
		expression.Parameters["myList"] = new List<string> { "apple", "banana", "apple" };
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!["apple"]!.Value<int>().Should().Be(2);
		result["banana"]!.Value<int>().Should().Be(1);
	}

	// Test with array
	[Fact]
	public void CountBy_WithArray_Works()
	{
		var expression = new ExtendedExpression("countBy(myArray, 'n', 'toString(n)')");
		expression.Parameters["myArray"] = new int[] { 1, 2, 2, 3 };
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!["1"]!.Value<int>().Should().Be(1);
		result["2"]!.Value<int>().Should().Be(2);
	}

	// Test case sensitivity
	[Fact]
	public void CountBy_CaseSensitive_TreatsAsDifferent()
	{
		var expression = new ExtendedExpression("countBy(list('Apple', 'apple', 'APPLE'), 'n', 'n')");
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!.Count.Should().Be(3); // All three are different keys
	}

	// Test with special characters in keys
	[Fact]
	public void CountBy_SpecialCharactersInKeys_Works()
	{
		var expression = new ExtendedExpression("countBy(list('a-b', 'a-b', 'c_d'), 'n', 'n')");
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result!["a-b"]!.Value<int>().Should().Be(2);
		result["c_d"]!.Value<int>().Should().Be(1);
	}
}
