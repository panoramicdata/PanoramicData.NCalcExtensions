using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

/// <summary>
/// Tests to verify that JValue objects are properly unwrapped to their underlying values
/// across all functions that work with JArrays
/// </summary>
public partial class JValueUnwrappingTests : NCalcTest
{
	#region where Tests

	[Fact]
	public void Where_JArray_ReturnsListWithUnwrappedValues()
	{
		var expression = new ExtendedExpression("where(jArray(1, 2, 3, 4, 5), 'n', 'n > 2')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new object[] { 3L, 4L, 5L });
	}

	[Fact]
	public void Where_JArrayOfStrings_ReturnsStrings()
	{
		var expression = new ExtendedExpression("where(jArray('apple', 'banana', 'apricot'), 's', 'startsWith(s, \"a\")')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().AllBeOfType<string>();
		result.Should().HaveCount(2);
	}

	[Fact]
	public void Where_JArrayWithEmptyString_PreservesEmptyString()
	{
		var expression = new ExtendedExpression("where(jArray('a', '', 'c'), 's', 's != null')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result![1].Should().Be("");
	}

	#endregion

	#region all/any Tests

	[Fact]
	public void All_JArrayOfBooleans_ReturnsCorrectResult()
		=> Test("all(jArray(true, true, true))").Should().Be(true);

	[Fact]
	public void All_JArrayWithLambda_WorksCorrectly()
		=> Test("all(jArray(2, 4, 6), 'n', 'n % 2 == 0')").Should().Be(true);

	[Fact]
	public void Any_JArrayOfNumbers_WorksCorrectly()
		=> Test("any(jArray(1, 3, 5, 6), 'n', 'n % 2 == 0')").Should().Be(true);

	[Fact]
	public void Any_JArrayOfStrings_WorksCorrectly()
		=> Test("any(jArray('cat', 'dog', 'bird'), 's', 'startsWith(s, \"d\")')").Should().Be(true);

	#endregion

	#region count Tests

	[Fact]
	public void Count_JArray_ReturnsCorrectCount()
		=> Test("count(jArray(1, 2, 3, 4, 5))").Should().Be(5);

	[Fact]
	public void Count_JArrayWithLambda_ReturnsCorrectCount()
		=> Test("count(jArray(1, 2, 3, 4, 5), 'n', 'n > 3')").Should().Be(2);

	#endregion

	#region selectDistinct Tests

	[Fact]
	public void SelectDistinct_JArray_ReturnsUnwrappedValues()
	{
		var expression = new ExtendedExpression("selectDistinct(jArray(1, 2, 2, 3), 'n', 'n')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new object[] { 1L, 2L, 3L });
	}

	[Fact]
	public void SelectDistinct_JArrayOfStrings_ReturnsStrings()
	{
		var expression = new ExtendedExpression("selectDistinct(jArray('a', 'b', 'a', 'c'), 's', 's')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().AllBeOfType<string>();
		result.Should().BeEquivalentTo(new[] { "a", "b", "c" });
	}

	#endregion
}
