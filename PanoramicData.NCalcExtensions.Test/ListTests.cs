using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class ListTests
{
	[Fact]
	public void List_OfInts_ReturnsExpectedType()
	{
		var expression = new ExtendedExpression($"list(1, 2, 3)");
		expression.Evaluate().Should().BeOfType<List<object?>>();
	}

	[Fact]
	public void List_OfInts_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"list(1, 2, 3)");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void List_OfExpressions_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"list(2 - 1, 2 + 0, 5 - 2)");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void List_OfStrings_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"list('1', '2', '3')");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { "1", "2", "3" }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void List_WhichIsEmpty_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"list()");
		expression.Evaluate().Should().BeEquivalentTo(new List<object>(), options => options.WithStrictOrdering());
	}

	[Fact]
	public void List_OfMixedTypes_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"list(null, 1-1, '1')");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { null!, 0, "1" }, options => options.WithStrictOrdering());
	}
}
