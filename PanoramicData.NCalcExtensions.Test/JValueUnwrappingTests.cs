using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

/// <summary>
/// Tests to verify that JValue objects are properly unwrapped to their underlying values
/// across all functions that work with JArrays
/// </summary>
public class JValueUnwrappingTests
{
	#region itemAtIndex Tests

	[Fact]
	public void ItemAtIndex_JArrayWithEmptyString_ReturnsString()
	{
		var expression = new ExtendedExpression("itemAtIndex(jArray('a', ''), 1)");
		expression.Evaluate().Should().BeOfType<string>();
	}

	[Fact]
	public void ItemAtIndex_JArrayWithEmptyString_EqualsEmptyString()
	{
		var expression = new ExtendedExpression("itemAtIndex(jArray('a', ''), 1) == ''");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void ItemAtIndex_JArrayWithString_ReturnsString()
	{
		var expression = new ExtendedExpression("itemAtIndex(jArray('hello', 'world'), 0)");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("hello");
	}

	[Fact]
	public void ItemAtIndex_JArrayWithNumber_ReturnsNumber()
	{
		var expression = new ExtendedExpression("itemAtIndex(jArray(1, 2, 3), 1)");
		var result = expression.Evaluate();
		result.Should().BeOfType<int>(); // JSON integers are deserialized as int
		result.Should().Be(2);
	}

	[Fact]
	public void ItemAtIndex_JArrayWithNull_ReturnsNull()
	{
		var expression = new ExtendedExpression("itemAtIndex(jArray('a', null, 'c'), 1)");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void ItemAtIndex_JArrayWithBoolean_ReturnsBoolean()
	{
		var expression = new ExtendedExpression("itemAtIndex(jArray(true, false), 0)");
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();
		result.Should().Be(true);
	}

	#endregion

	#region first/firstOrDefault Tests

	[Fact]
	public void First_JArrayWithStrings_ReturnsString()
	{
		var expression = new ExtendedExpression("first(jArray('a', 'b', 'c'))");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("a");
	}

	[Fact]
	public void First_JArrayWithEmptyString_ReturnsEmptyString()
	{
		var expression = new ExtendedExpression("first(jArray('', 'b'))");
		expression.Evaluate().Should().Be("");
	}

	[Fact]
	public void First_JArrayWithLambda_ReturnsUnwrappedValue()
	{
		var expression = new ExtendedExpression("first(jArray(1, 2, 3, 4), 'n', 'n > 2')");
		var result = expression.Evaluate();
		result.Should().BeOfType<int>();
		result.Should().Be(3);
	}

	[Fact]
	public void FirstOrDefault_JArrayWithStrings_ReturnsString()
	{
		var expression = new ExtendedExpression("firstOrDefault(jArray('x', 'y'))");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("x");
	}

	[Fact]
	public void FirstOrDefault_JArrayNoMatch_ReturnsNull()
	{
		var expression = new ExtendedExpression("firstOrDefault(jArray(1, 2, 3), 'n', 'n > 10')");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void FirstOrDefault_EmptyJArray_ReturnsNull()
	{
		var expression = new ExtendedExpression("firstOrDefault(jArray())");
		expression.Evaluate().Should().BeNull();
	}

	#endregion

	#region last/lastOrDefault Tests

	[Fact]
	public void Last_JArrayWithStrings_ReturnsString()
	{
		var expression = new ExtendedExpression("last(jArray('a', 'b', 'c'))");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("c");
	}

	[Fact]
	public void Last_JArrayWithEmptyString_ReturnsEmptyString()
	{
		var expression = new ExtendedExpression("last(jArray('a', ''))");
		expression.Evaluate().Should().Be("");
	}

	[Fact]
	public void Last_JArrayWithLambda_ReturnsUnwrappedValue()
	{
		var expression = new ExtendedExpression("last(jArray(1, 2, 3, 4, 2), 'n', 'n == 2')");
		var result = expression.Evaluate();
		result.Should().BeOfType<int>();
		result.Should().Be(2);
	}

	[Fact]
	public void LastOrDefault_JArrayWithStrings_ReturnsString()
	{
		var expression = new ExtendedExpression("lastOrDefault(jArray('x', 'y', 'z'))");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("z");
	}

	[Fact]
	public void LastOrDefault_JArrayNoMatch_ReturnsNull()
	{
		var expression = new ExtendedExpression("lastOrDefault(jArray(1, 2, 3), 'n', 'n > 10')");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void LastOrDefault_EmptyJArray_ReturnsNull()
	{
		var expression = new ExtendedExpression("lastOrDefault(jArray())");
		expression.Evaluate().Should().BeNull();
	}

	#endregion

	#region select Tests

	[Fact]
	public void Select_JArrayWithStrings_ReturnsListOfStrings()
	{
		var expression = new ExtendedExpression("select(jArray('a', 'b', 'c'), 's', 's')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result.Should().AllBeOfType<string>();
	}

	[Fact]
	public void Select_JArrayWithEmptyStrings_ReturnsEmptyStrings()
	{
		var expression = new ExtendedExpression("select(jArray('a', '', 'c'), 's', 's')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result![1].Should().Be("");
	}

	[Fact]
	public void Select_JArrayWithNumbers_ReturnsNumbers()
	{
		var expression = new ExtendedExpression("select(jArray(1, 2, 3), 'n', 'n')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result![0].Should().Be(1L);
		result[1].Should().Be(2L);
	}

	[Fact]
	public void Select_JArrayWithLambdaReturningString_ReturnsStrings()
	{
		var expression = new ExtendedExpression("select(jArray(1, 2, 3), 'n', 'toString(n)')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().AllBeOfType<string>();
	}

	[Fact]
	public void Select_JArrayItemsInLambda_ReturnsUnwrappedValues()
	{
		var expression = new ExtendedExpression("select(jArray('a', 'b'), 's', 'toUpper(s)')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new[] { "A", "B" });
	}

	#endregion

	#region min/max Tests

	[Fact]
	public void Min_JArrayOfNumbers_ReturnsNumber()
	{
		var expression = new ExtendedExpression("min(jArray(3, 1, 4, 1, 5))");
		var result = expression.Evaluate();
		result.Should().Be(1);
	}

	[Fact]
	public void Min_JArrayOfStrings_ReturnsString()
	{
		var expression = new ExtendedExpression("min(jArray('zebra', 'apple', 'banana'))");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("apple");
	}

	[Fact]
	public void Max_JArrayOfNumbers_ReturnsNumber()
	{
		var expression = new ExtendedExpression("max(jArray(3, 1, 4, 1, 5))");
		var result = expression.Evaluate();
		result.Should().Be(5);
	}

	[Fact]
	public void Max_JArrayOfStrings_ReturnsString()
	{
		var expression = new ExtendedExpression("max(jArray('zebra', 'apple', 'banana'))");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("zebra");
	}

	[Fact]
	public void Min_JArrayWithLambda_ReturnsCorrectValue()
	{
		var expression = new ExtendedExpression("min(jArray(5, 3, 8, 1), 'n', 'n * 2')");
		var result = expression.Evaluate();
		result.Should().Be(2); // 1 * 2 = 2
	}

	[Fact]
	public void Max_JArrayWithLambda_ReturnsCorrectValue()
	{
		var expression = new ExtendedExpression("max(jArray(5, 3, 8, 1), 'n', 'n * 2')");
		var result = expression.Evaluate();
		result.Should().Be(16); // 8 * 2 = 16
	}

	#endregion

	#region where Tests

	[Fact]
	public void Where_JArray_ReturnsListWithUnwrappedValues()
	{
		var expression = new ExtendedExpression("where(jArray(1, 2, 3, 4, 5), 'n', 'n > 2')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new object[] { 3L, 4L, 5L });
	}

	[Fact]
	public void Where_JArrayOfStrings_ReturnsStrings()
	{
		var expression = new ExtendedExpression("where(jArray('apple', 'banana', 'apricot'), 's', 'startsWith(s, \"a\")')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().AllBeOfType<string>();
		result.Should().HaveCount(2);
	}

	[Fact]
	public void Where_JArrayWithEmptyString_PreservesEmptyString()
	{
		var expression = new ExtendedExpression("where(jArray('a', '', 'c'), 's', 's != null')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result![1].Should().Be("");
	}

	#endregion

	#region all/any Tests

	[Fact]
	public void All_JArrayOfBooleans_ReturnsCorrectResult()
	{
		var expression = new ExtendedExpression("all(jArray(true, true, true))");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void All_JArrayWithLambda_WorksCorrectly()
	{
		var expression = new ExtendedExpression("all(jArray(2, 4, 6), 'n', 'n % 2 == 0')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_JArrayOfNumbers_WorksCorrectly()
	{
		var expression = new ExtendedExpression("any(jArray(1, 3, 5, 6), 'n', 'n % 2 == 0')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_JArrayOfStrings_WorksCorrectly()
	{
		var expression = new ExtendedExpression("any(jArray('cat', 'dog', 'bird'), 's', 'startsWith(s, \"d\")')");
		expression.Evaluate().Should().Be(true);
	}

	#endregion

	#region count Tests

	[Fact]
	public void Count_JArray_ReturnsCorrectCount()
	{
		var expression = new ExtendedExpression("count(jArray(1, 2, 3, 4, 5))");
		expression.Evaluate().Should().Be(5);
	}

	[Fact]
	public void Count_JArrayWithLambda_ReturnsCorrectCount()
	{
		var expression = new ExtendedExpression("count(jArray(1, 2, 3, 4, 5), 'n', 'n > 3')");
		expression.Evaluate().Should().Be(2);
	}

	#endregion

	#region selectDistinct Tests

	[Fact]
	public void SelectDistinct_JArray_ReturnsUnwrappedValues()
	{
		var expression = new ExtendedExpression("selectDistinct(jArray(1, 2, 2, 3), 'n', 'n')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new object[] { 1L, 2L, 3L });
	}

	[Fact]
	public void SelectDistinct_JArrayOfStrings_ReturnsStrings()
	{
		var expression = new ExtendedExpression("selectDistinct(jArray('a', 'b', 'a', 'c'), 's', 's')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().AllBeOfType<string>();
		result.Should().BeEquivalentTo(new[] { "a", "b", "c" });
	}

	#endregion

	#region orderBy Tests

	[Fact]
	public void OrderBy_JArrayOfNumbers_ReturnsUnwrappedValues()
	{
		var expression = new ExtendedExpression("orderBy(jArray(3, 1, 4, 1, 5), 'n', 'n')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new object[] { 1L, 1L, 3L, 4L, 5L }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_JArrayOfStrings_ReturnsStrings()
	{
		var expression = new ExtendedExpression("orderBy(jArray('zebra', 'apple', 'banana'), 's', 's')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().AllBeOfType<string>();
		result.Should().BeEquivalentTo(new[] { "apple", "banana", "zebra" }, options => options.WithStrictOrdering());
	}

	#endregion

	#region sum Tests

	[Fact]
	public void Sum_JArrayOfNumbers_ReturnsCorrectSum()
	{
		var expression = new ExtendedExpression("sum(jArray(1, 2, 3, 4, 5))");
		var result = expression.Evaluate();
		// Sum handles JValues internally
		result.Should().Be(15.0); // GetSum returns double
	}

	[Fact]
	public void Sum_JArrayWithLambda_ReturnsCorrectSum()
	{
		var expression = new ExtendedExpression("sum(jArray(1, 2, 3), 'n', 'n * 2')");
		var result = expression.Evaluate();
		result.Should().BeOfType<double>();
		result.Should().Be(12.0); // (1*2) + (2*2) + (3*2) = 12
	}

	#endregion

	#region Complex/Integration Tests

	[Fact]
	public void ChainedOperations_JArray_AllValuesUnwrapped()
	{
		var expression = new ExtendedExpression(@"
			first(
				orderBy(
					where(
						select(jArray(1, 2, 3, 4, 5), 'n', 'n * 2'),
						'n',
						'n > 4'
					),
					'n',
					'n'
				)
			)");
		var result = expression.Evaluate();
		result.Should().Be(6); // First value after filtering (2*3=6) and ordering
	}

	[Fact]
	public void ItemAtIndex_WithSelect_ReturnsUnwrappedValue()
	{
		var expression = new ExtendedExpression("itemAtIndex(select(jArray('a', 'b', 'c'), 's', 's'), 1)");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("b");
	}

	[Fact]
	public void JArrayOfJObjects_ItemAtIndex_ReturnsJObject()
	{
		var expression = new ExtendedExpression(@"
			itemAtIndex(
				jArray(
					jObject('name', 'Alice'),
					jObject('name', 'Bob')
				),
				0
			)");
		var result = expression.Evaluate();
		result.Should().BeOfType<JObject>();
	}

	[Fact]
	public void MixedTypes_JArray_HandlesCorrectly()
	{
		var expression = new ExtendedExpression("select(jArray(1, 'two', 3.0, true), 'x', 'toString(x)')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().AllBeOfType<string>();
		result.Should().HaveCount(4);
	}

	[Fact]
	public void EmptyString_ThroughMultipleOperations_PreservesValue()
	{
		var expression = new ExtendedExpression(@"
			itemAtIndex(
				select(
					where(
						jArray('', 'a', '', 'b'),
						's',
						'length(s) == 0'
					),
					's',
					's'
				),
				0
			)");
		expression.Evaluate().Should().Be("");
	}

	[Fact]
	public void StringComparison_WithJArrayElements_Works()
	{
		var expression = new ExtendedExpression("first(jArray('test', 'value')) == 'test'");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void NumericComparison_WithJArrayElements_Works()
	{
		var expression = new ExtendedExpression("first(jArray(42, 100)) > 40");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void NullHandling_ThroughOperations_WorksCorrectly()
	{
		var expression = new ExtendedExpression("count(where(jArray(1, null, 3, null, 5), 'n', 'n != null'))");
		expression.Evaluate().Should().Be(3);
	}

	#endregion
}
