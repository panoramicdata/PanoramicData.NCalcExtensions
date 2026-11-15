using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

/// <summary>
/// Tests to verify that JValue objects are properly unwrapped to their underlying values
/// across all functions that work with JArrays
/// </summary>
public partial class JValueUnwrappingTests : NCalcTest
{
	#region orderBy Tests

	[Fact]
	public void OrderBy_JArrayOfNumbers_ReturnsUnwrappedValues()
	{
		var result = Test("orderBy(jArray(3, 1, 4, 1, 5), 'n', 'n')") as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new object[] { 1L, 1L, 3L, 4L, 5L }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OrderBy_JArrayOfStrings_ReturnsStrings()
	{
		var result = Test("orderBy(jArray('zebra', 'apple', 'banana'), 's', 's')") as List<object?>;
		result.Should().NotBeNull();
		result.Should().AllBeOfType<string>();
		result.Should().BeEquivalentTo(new[] { "apple", "banana", "zebra" }, options => options.WithStrictOrdering());
	}

	#endregion

	#region sum Tests

	[Fact]
	public void Sum_JArrayOfNumbers_ReturnsCorrectSum()
	{
		var result = Test("sum(jArray(1, 2, 3, 4, 5))");
		// Sum handles JValues internally
		result.Should().Be(15.0); // GetSum returns double
	}

	[Fact]
	public void Sum_JArrayWithLambda_ReturnsCorrectSum()
	{
		var result = Test("sum(jArray(1, 2, 3), 'n', 'n * 2')");
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
		var result = Test("itemAtIndex(select(jArray('a', 'b', 'c'), 's', 's'), 1)");
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
		var result = Test("select(jArray(1, 'two', 3.0, true), 'x', 'toString(x)')") as List<object?>;
		result.Should().NotBeNull();
		result.Should().AllBeOfType<string>();
		result.Should().HaveCount(4);
	}

	[Fact]
	public void EmptyString_ThroughMultipleOperations_PreservesValue()
		=> Test(@"
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
			)").Should().Be("");

	[Fact]
	public void StringComparison_WithJArrayElements_Works()
		=> Test("first(jArray('test', 'value')) == 'test'").Should().Be(true);

	[Fact]
	public void NumericComparison_WithJArrayElements_Works()
		=> Test("first(jArray(42, 100)) > 40").Should().Be(true);

	[Fact]
	public void NullHandling_ThroughOperations_WorksCorrectly()
		=> Test("count(where(jArray(1, null, 3, null, 5), 'n', 'n != null'))").Should().Be(3);

	#endregion
}
