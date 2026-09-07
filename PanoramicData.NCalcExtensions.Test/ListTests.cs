using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class ListTests : NCalcTest
{
	[Fact]
	public void List_OfInts_ReturnsExpectedType()
		=> Test($"list(1, 2, 3)").Should().BeOfType<List<object?>>();

	[Fact]
	public void List_OfInts_ReturnsExpected()
		=> Test($"list(1, 2, 3)").Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());

	[Fact]
	public void List_OfExpressions_ReturnsExpected()
		=> Test($"list(2 - 1, 2 + 0, 5 - 2)").Should().BeEquivalentTo(new List<object> { 1, 2, 3 }, options => options.WithStrictOrdering());

	[Fact]
	public void List_OfStrings_ReturnsExpected()
		=> Test($"list('1', '2', '3')").Should().BeEquivalentTo(new List<object> { "1", "2", "3" }, options => options.WithStrictOrdering());

	[Fact]
	public void List_WhichIsEmpty_ReturnsExpected()
		=> Test($"list()").Should().BeEquivalentTo(new List<object>(), options => options.WithStrictOrdering());

	[Fact]
	public void List_OfMixedTypes_ReturnsExpected()
		=> Test($"list(null, 1-1, '1')").Should().BeEquivalentTo(new List<object> { null!, 0, "1" }, options => options.WithStrictOrdering());
}
