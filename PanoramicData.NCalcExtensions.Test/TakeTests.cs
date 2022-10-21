using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class TakeTests
{
	[Fact]
	public void List_OfInts_ReturnsExpectedType()
	{
		var expression = new ExtendedExpression($"take(list(1, 2, 3), 1)");
		expression.Evaluate().Should().BeOfType<List<object?>>();
	}

	[Fact]
	public void List_OfInts_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"take(list('a', 2, 3), 1)");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { "a" }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void TakingTooMany_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"take(list(1, 2, 3), 10)");
		expression.Evaluate().Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}
}
