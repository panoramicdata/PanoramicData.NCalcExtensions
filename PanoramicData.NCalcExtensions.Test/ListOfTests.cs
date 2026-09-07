using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class ListOfTests : NCalcTest
{
	[Fact]
	public void List_OfInts_ReturnsExpectedType()
		=> Test($"listOf('int', 1, 2, 3)").Should().BeOfType<List<int>>();

	[Fact]
	public void List_OfInts_ReturnsExpected()
		=> Test($"listOf('int', 1, 2, 3)").Should().BeEquivalentTo(new List<int> { 1, 2, 3 }, options => options.WithStrictOrdering());

	[Fact]
	public void List_OfLongs_IsForgivingToInts()
		=> Test($"listOf('long', 1, 2, 3)").Should().BeEquivalentTo(new List<long> { 1, 2, 3 }, options => options.WithStrictOrdering());

	[Fact]
	public void List_OfInts_IsForgivingToDoubles()
		=> Test($"listOf('int', 1.0, 2.0, 3.1)").Should().BeEquivalentTo(new List<int> { 1, 2, 3 }, options => options.WithStrictOrdering());

	[Fact]
	public void List_OfDoubles_IsForgivingToInts()
		=> Test($"listOf('double', 1, 2, 3)").Should().BeEquivalentTo(new List<double> { 1, 2, 3 }, options => options.WithStrictOrdering());

	[Fact]
	public void List_OfExpressions_ReturnsExpected()
		=> Test($"listOf('int', 2 - 1, 2 + 0, 5 - 2)").Should().BeEquivalentTo(new List<int> { 1, 2, 3 }, options => options.WithStrictOrdering());

	[Fact]
	public void List_OfStrings_ReturnsExpected()
		=> Test($"listOf('string', '1', '2', '3')").Should().BeEquivalentTo(new List<string> { "1", "2", "3" }, options => options.WithStrictOrdering());

	[Fact]
	public void List_WhichIsEmpty_ReturnsExpected()
		=> Test($"listOf('string')").Should().BeEquivalentTo(new List<string>(), options => options.WithStrictOrdering());

	[Fact]
	public void List_OfMixedTypes_ReturnsExpected()
		=> Test($"listOf('object', null, 1-1, '1')").Should().BeEquivalentTo(new List<object> { null!, 0, "1" }, options => options.WithStrictOrdering());
}
