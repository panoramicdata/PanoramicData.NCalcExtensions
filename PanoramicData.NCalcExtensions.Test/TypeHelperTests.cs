using System.Collections.Generic;
using PanoramicData.NCalcExtensions.Helpers;

namespace PanoramicData.NCalcExtensions.Test;

public class TypeHelperTests
{
	[Theory]
	[InlineData("Int32")]
	[InlineData("String")]
	[InlineData("Double")]
	[InlineData("Boolean")]
	public void AsHumanString_SimpleTypes_ReturnsTypeName(string typeName)
	{
		// Using reflection to test various types dynamically
		var result = typeName switch
		{
			"Int32" => TypeHelper.AsHumanString<int>(),
			"String" => TypeHelper.AsHumanString<string>(),
			"Double" => TypeHelper.AsHumanString<double>(),
			"Boolean" => TypeHelper.AsHumanString<bool>(),
			_ => throw new ArgumentException("Unknown type")
		};
		
		result.Should().Be(typeName);
	}

	[Fact]
	public void AsHumanString_GenericList_ReturnsReadableFormat()
	{
		var result = TypeHelper.AsHumanString<List<int>>();
		result.Should().Be("List`1<Int32>");
	}

	[Fact]
	public void AsHumanString_GenericDictionary_ReturnsReadableFormat()
	{
		var result = TypeHelper.AsHumanString<Dictionary<string, object>>();
		result.Should().Be("Dictionary`2<String,Object>");
	}

	[Fact]
	public void AsHumanString_NullableInt_ReturnsReadableFormat()
	{
		var result = TypeHelper.AsHumanString<int?>();
		result.Should().Be("Nullable`1<Int32>");
	}

	[Fact]
	public void AsHumanString_DateTime_ReturnsTypeName()
	{
		var result = TypeHelper.AsHumanString<DateTime>();
		result.Should().Be("DateTime");
	}

	[Fact]
	public void AsHumanString_NullableDateTime_ReturnsReadableFormat()
	{
		var result = TypeHelper.AsHumanString<DateTime?>();
		result.Should().Be("Nullable`1<DateTime>");
	}
}
