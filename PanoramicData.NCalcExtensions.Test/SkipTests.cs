using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class SkipTests
{
	[Fact]
	public void List_OfInts_ReturnsExpectedType()
	{
		var expression = new ExtendedExpression($"skip(list(1, 2, 3), 1)");
		var result = expression.Evaluate();
		result.Should().BeOfType<List<object?>>();
	}

	[Fact]
	public void Array_OfInts_ReturnsExpectedType()
	{
		var expression = new ExtendedExpression($"skip(theArray, 1)");
		expression.Parameters["theArray"] = new[] { 1, 2, 3 };
		var result = expression.Evaluate();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void List_OfInts_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"skip(list(1, 2, 3), 1)");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void SkippingTooMany_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"skip(list(1, 2, 3), 10)");
		expression.Evaluate().Should().BeEquivalentTo(new List<object>(), options => options.WithStrictOrdering());
	}
}
