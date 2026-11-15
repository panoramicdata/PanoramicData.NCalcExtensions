using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

/// <summary>
/// Behaviour shared by the functions that return part of a list: take and skip.
/// </summary>
public abstract class SliceFunctionTests(string functionName, object[] expectedFromOne, object[] expectedFromTooMany) : NCalcTest
{
	[Fact]
	public void List_OfInts_ReturnsExpectedType()
		=> Test($"{functionName}(list(1, 2, 3), 1)").Should().BeOfType<List<object?>>();

	[Fact]
	public void Array_OfInts_ReturnsExpectedTypeAndItems()
	{
		var result = Test($"{functionName}(theArray, 1)", "theArray", new[] { 1, 2, 3 });

		result.Should().BeOfType<List<object?>>();
		result.Should().BeEquivalentTo(expectedFromOne, options => options.WithStrictOrdering());
	}

	[Fact]
	public void List_OfInts_ReturnsExpected()
		=> Test($"{functionName}(list(1, 2, 3), 1)").Should().BeEquivalentTo(expectedFromOne, options => options.WithStrictOrdering());

	[Fact]
	public void CountBeyondTheEnd_ReturnsExpected()
		=> Test($"{functionName}(list(1, 2, 3), 10)").Should().BeEquivalentTo(expectedFromTooMany, options => options.WithStrictOrdering());

	[Fact]
	public void InvalidCountParameter_ThrowsException()
		=> TestShouldThrowExactly<FormatException>($"{functionName}(list(1, 2, 3), 'invalid')", "*");
}
