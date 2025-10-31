using System.Collections.Generic;
using PanoramicData.NCalcExtensions.Extensions;

namespace PanoramicData.NCalcExtensions.Test;

public class ListHelpersTests : NCalcTest
{
	[Fact]
	public void Collapse_EmptyList_ReturnsEmpty()
	{
		var list = new List<object?>();
		var result = list.Collapse();
		result.Should().BeEmpty();
	}

	[Fact]
	public void Collapse_FlatList_ReturnsSame()
	{
		var list = new List<object?> { 1, "test", 3.14 };
		var result = list.Collapse();
		result.Should().BeEquivalentTo(new List<object?> { 1, "test", 3.14 });
	}

	[Fact]
	public void Collapse_SingleNestedList_Flattens()
	{
		var inner = new List<object?> { 1, 2, 3 };
		var list = new List<object?> { inner };
		var result = list.Collapse();
		result.Should().BeEquivalentTo(new List<object?> { 1, 2, 3 });
	}

	[Fact]
	public void Collapse_MultipleNestedLists_Flattens()
	{
		var list1 = new List<object?> { 1, 2 };
		var list2 = new List<object?> { 3, 4 };
		var list = new List<object?> { list1, list2 };
		var result = list.Collapse();
		result.Should().BeEquivalentTo(new List<object?> { 1, 2, 3, 4 });
	}

	[Fact]
	public void Collapse_DeeplyNested_CollapsesAll()
	{
		var inner3 = new List<object?> { 1, 2 };
		var inner2 = new List<object?> { inner3 };
		var inner1 = new List<object?> { inner2 };
		var list = new List<object?> { inner1 };
		var result = list.Collapse();
		result.Should().BeEquivalentTo(new List<object?> { 1, 2 });
	}

	[Fact]
	public void Collapse_MixedTypes_PreservesNonLists()
	{
		var inner = new List<object?> { 1, 2 };
		var list = new List<object?> { inner, "string", 3 };
		var result = list.Collapse();
		// Should not collapse since not all items are lists
		result.Should().BeEquivalentTo(new List<object?> { inner, "string", 3 });
	}
}
