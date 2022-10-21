using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class ConcatTests
{
	[Fact]
	public void OneListsOfInts_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(list(1, 2, 3))");
		var result = expression.Evaluate();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void TwoListsOfInts_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(list(1), list(2, 3))");
		var result = expression.Evaluate();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void ThreeListsOfInts_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(list(1), list(2), list(3))");
		var result = expression.Evaluate();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void ListOfIntsAddingOneObject_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(list(1, 2), 3)");
		var result = expression.Evaluate();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}

	[Fact]
	public void OneObjectAddingListOfInts_Succeeds()
	{
		var expression = new ExtendedExpression($"concat(1, list(2, 3))");
		var result = expression.Evaluate();
		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());
	}
}
