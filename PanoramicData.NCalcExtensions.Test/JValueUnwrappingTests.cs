using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

/// <summary>
/// Tests to verify that JValue objects are properly unwrapped to their underlying values
/// across all functions that work with JArrays
/// </summary>
public partial class JValueUnwrappingTests : NCalcTest
{
	#region itemAtIndex Tests

	[Fact]
	public void ItemAtIndex_JArrayWithEmptyString_ReturnsString()
		=> Test("itemAtIndex(jArray('a', ''), 1)").Should().BeOfType<string>();

	[Fact]
	public void ItemAtIndex_JArrayWithEmptyString_EqualsEmptyString()
		=> Test("itemAtIndex(jArray('a', ''), 1) == ''").Should().Be(true);

	[Fact]
	public void ItemAtIndex_JArrayWithString_ReturnsString()
	{
		var result = Test("itemAtIndex(jArray('hello', 'world'), 0)");
		result.Should().BeOfType<string>();
		result.Should().Be("hello");
	}

	[Fact]
	public void ItemAtIndex_JArrayWithNumber_ReturnsNumber()
	{
		var result = Test("itemAtIndex(jArray(1, 2, 3), 1)");
		result.Should().BeOfType<int>(); // JSON integers are deserialized as int
		result.Should().Be(2);
	}

	[Fact]
	public void ItemAtIndex_JArrayWithNull_ReturnsNull()
		=> Test("itemAtIndex(jArray('a', null, 'c'), 1)").Should().BeNull();

	[Fact]
	public void ItemAtIndex_JArrayWithBoolean_ReturnsBoolean()
	{
		var result = Test("itemAtIndex(jArray(true, false), 0)");
		result.Should().BeOfType<bool>();
		result.Should().Be(true);
	}

	#endregion

	#region first/firstOrDefault Tests

	[Fact]
	public void First_JArrayWithStrings_ReturnsString()
	{
		var result = Test("first(jArray('a', 'b', 'c'))");
		result.Should().BeOfType<string>();
		result.Should().Be("a");
	}

	[Fact]
	public void First_JArrayWithEmptyString_ReturnsEmptyString()
		=> Test("first(jArray('', 'b'))").Should().Be("");

	[Fact]
	public void First_JArrayWithLambda_ReturnsUnwrappedValue()
	{
		var result = Test("first(jArray(1, 2, 3, 4), 'n', 'n > 2')");
		result.Should().BeOfType<int>();
		result.Should().Be(3);
	}

	[Fact]
	public void FirstOrDefault_JArrayWithStrings_ReturnsString()
	{
		var result = Test("firstOrDefault(jArray('x', 'y'))");
		result.Should().BeOfType<string>();
		result.Should().Be("x");
	}

	[Fact]
	public void FirstOrDefault_JArrayNoMatch_ReturnsNull()
		=> Test("firstOrDefault(jArray(1, 2, 3), 'n', 'n > 10')").Should().BeNull();

	[Fact]
	public void FirstOrDefault_EmptyJArray_ReturnsNull()
		=> Test("firstOrDefault(jArray())").Should().BeNull();

	#endregion

	#region last/lastOrDefault Tests

	[Fact]
	public void Last_JArrayWithStrings_ReturnsString()
	{
		var result = Test("last(jArray('a', 'b', 'c'))");
		result.Should().BeOfType<string>();
		result.Should().Be("c");
	}

	[Fact]
	public void Last_JArrayWithEmptyString_ReturnsEmptyString()
		=> Test("last(jArray('a', ''))").Should().Be("");

	[Fact]
	public void Last_JArrayWithLambda_ReturnsUnwrappedValue()
	{
		var result = Test("last(jArray(1, 2, 3, 4, 2), 'n', 'n == 2')");
		result.Should().BeOfType<int>();
		result.Should().Be(2);
	}

	[Fact]
	public void LastOrDefault_JArrayWithStrings_ReturnsString()
	{
		var result = Test("lastOrDefault(jArray('x', 'y', 'z'))");
		result.Should().BeOfType<string>();
		result.Should().Be("z");
	}

	[Fact]
	public void LastOrDefault_JArrayNoMatch_ReturnsNull()
		=> Test("lastOrDefault(jArray(1, 2, 3), 'n', 'n > 10')").Should().BeNull();

	[Fact]
	public void LastOrDefault_EmptyJArray_ReturnsNull()
		=> Test("lastOrDefault(jArray())").Should().BeNull();

	#endregion
}
