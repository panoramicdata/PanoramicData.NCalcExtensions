using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class DistinctTests : NCalcTest
{
	[Fact]
	public void Distinct_Succeeds()
	{
		var result = new ExtendedExpression($"distinct(list(1, 2, 3, 3, 3))")
			.Evaluate();

		// The result should be 1, 2, 3
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<int> { 1, 2, 3 });
	}

	// Additional comprehensive tests
	[Fact]
	public void Distinct_EmptyList_ReturnsEmpty()
	{
		var expression = new ExtendedExpression("distinct(list())");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEmpty();
	}

	[Fact]
	public void Distinct_NoDuplicates_ReturnsSame()
	{
		var expression = new ExtendedExpression("distinct(list(1, 2, 3, 4))");
		expression.Evaluate().Should().BeEquivalentTo(new List<int> { 1, 2, 3, 4 });
	}

	[Fact]
	public void Distinct_AllDuplicates_ReturnsSingle()
	{
		var expression = new ExtendedExpression("distinct(list(5, 5, 5, 5))");
		expression.Evaluate().Should().BeEquivalentTo(new List<int> { 5 });
	}

	[Fact]
	public void Distinct_Strings_Works()
	{
		var expression = new ExtendedExpression("distinct(list('a', 'b', 'a', 'c', 'b'))");
		expression.Evaluate().Should().BeEquivalentTo(new List<string> { "a", "b", "c" });
	}

	[Fact]
	public void Distinct_MixedTypes_Works()
	{
		var expression = new ExtendedExpression("distinct(list(1, 'two', 1, 'two', 3))");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
	}

	[Fact]
	public void Distinct_WithNulls_PreservesOneNull()
	{
		var expression = new ExtendedExpression("distinct(list(1, null, 2, null, 3))");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(4); // 1, null, 2, 3
	}

	[Fact]
	public void Distinct_Booleans_Works()
	{
		var expression = new ExtendedExpression("distinct(list(true, false, true, false, true))");
		expression.Evaluate().Should().BeEquivalentTo(new List<bool> { true, false });
	}

	[Fact]
	public void Distinct_Doubles_Works()
	{
		var expression = new ExtendedExpression("distinct(list(1.5, 2.5, 1.5, 3.5))");
		expression.Evaluate().Should().BeEquivalentTo(new List<double> { 1.5, 2.5, 3.5 });
	}

	[Fact]
	public void Distinct_PreservesOrder()
	{
		var expression = new ExtendedExpression("distinct(list(3, 1, 2, 3, 1))");
		expression.Evaluate().Should().BeEquivalentTo(new List<int> { 3, 1, 2 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void Distinct_LongList_Works()
	{
		var expression = new ExtendedExpression("distinct(list(1, 2, 3, 4, 5, 1, 2, 3, 4, 5))");
		expression.Evaluate().Should().BeEquivalentTo(new List<int> { 1, 2, 3, 4, 5 });
	}

	[Fact]
	public void Distinct_SingleItem_ReturnsSame()
	{
		var expression = new ExtendedExpression("distinct(list(42))");
		expression.Evaluate().Should().BeEquivalentTo(new List<int> { 42 });
	}

	[Fact]
	public void Distinct_WithVariables_Works()
	{
		var expression = new ExtendedExpression("distinct(myList)");
		expression.Parameters["myList"] = new List<object> { 1, 2, 2, 3, 3, 3 };
		expression.Evaluate().Should().BeEquivalentTo(new List<int> { 1, 2, 3 });
	}

	[Fact]
	public void Distinct_TypedArray_Works()
	{
		var expression = new ExtendedExpression("distinct(myArray)");
		expression.Parameters["myArray"] = new[] { "a", "b", "a", "c" };
		expression.Evaluate().Should().BeEquivalentTo(new List<string> { "a", "b", "c" });
	}

	[Fact]
	public void Distinct_ChainedWithSelect_Works()
	{
		var expression = new ExtendedExpression("distinct(select(list(1, 2, 2, 3), 'n', 'n * 2'))");
		expression.Evaluate().Should().BeEquivalentTo(new List<int> { 2, 4, 6 });
	}

	[Fact]
	public void Distinct_ThenCount_Works()
	{
		var expression = new ExtendedExpression("count(distinct(list(1, 1, 2, 2, 3, 3)))");
		expression.Evaluate().Should().Be(3);
	}

	[Fact]
	public void Distinct_ThenJoin_Works()
	{
		var expression = new ExtendedExpression("join(distinct(list('x', 'y', 'x', 'z')), ',')");
		expression.Evaluate().Should().Be("x,y,z");
	}

	[Fact]
	public void Distinct_CaseSensitive_Strings()
	{
		var expression = new ExtendedExpression("distinct(list('A', 'a', 'B', 'b'))");
		expression.Evaluate().Should().BeEquivalentTo(new List<string> { "A", "a", "B", "b" });
	}

	// Error cases
	[Fact]
	public void Distinct_NullList_ThrowsException()
	{
		var expression = new ExtendedExpression("distinct(null)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}

	[Fact]
	public void Distinct_NonEnumerable_ThrowsException()
	{
		var expression = new ExtendedExpression("distinct(42)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}

	[Fact]
	public void Distinct_NoParameters_ThrowsException()
	{
		var expression = new ExtendedExpression("distinct()");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<Exception>();
	}
}
