using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class SkipTests : NCalcTest
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
		=> Test($"skip(list(1, 2, 3), 1)").Should().BeEquivalentTo(new List<object> { 2, 3 }, options => options.WithStrictOrdering());

	[Fact]
	public void SkippingTooMany_ReturnsExpected()
		=> Test($"skip(list(1, 2, 3), 10)").Should().BeEquivalentTo(new List<object>(), options => options.WithStrictOrdering());

	[Fact]
	public void Skip_InvalidCountParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("skip(list(1, 2, 3), 'invalid')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}
}
