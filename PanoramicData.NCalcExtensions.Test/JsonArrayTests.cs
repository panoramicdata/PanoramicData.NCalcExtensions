using System.Linq;
using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class JsonArrayTests
{
	[Fact]
	public void JsonArray_CreatesJsonArray()
	{
		var expression = new ExtendedExpression("jsonArray(1, 'test', null, true)");
		var result = expression.Evaluate() as JsonDocument;
		result.Should().BeOfType<JsonDocument>();
		result.Should().NotBeNull();
		result!.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
		result.RootElement.GetArrayLength().Should().Be(4);

		var elements = result.RootElement.EnumerateArray().ToArray();
		elements[0].GetInt32().Should().Be(1);
		elements[1].GetString().Should().Be("test");
		elements[2].ValueKind.Should().Be(JsonValueKind.Null);
		elements[3].GetBoolean().Should().BeTrue();
	}

	[Fact]
	public void JsonArray_EmptyArray_CreatesEmptyJsonArray()
	{
		var expression = new ExtendedExpression("jsonArray()");
		var result = expression.Evaluate() as JsonDocument;
		result.Should().BeOfType<JsonDocument>();
		result.Should().NotBeNull();
		result!.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
		result.RootElement.GetArrayLength().Should().Be(0);
	}

	[Fact]
	public void JsonArray_NestedObjects_CreatesComplexArray()
	{
		var expression = new ExtendedExpression("jsonArray(jsonDocument('a', 1), jsonDocument('b', 2))");
		var result = expression.Evaluate() as JsonDocument;
		result.Should().BeOfType<JsonDocument>();
		result.Should().NotBeNull();
		result!.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
		result.RootElement.GetArrayLength().Should().Be(2);
		
		var elements = result.RootElement.EnumerateArray().ToArray();
		elements[0].ValueKind.Should().Be(JsonValueKind.Object);
		elements[1].ValueKind.Should().Be(JsonValueKind.Object);
	}

	[Fact]
	public void JsonArray_StringParameters_CreatesJsonArray()
	{
		var expression = new ExtendedExpression("jsonArray('test1', 'test2')");
		var result = expression.Evaluate() as JsonDocument;
		result.Should().BeOfType<JsonDocument>();
		result.Should().NotBeNull();
		result!.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
		result.RootElement.GetArrayLength().Should().Be(2);
		
		var elements = result.RootElement.EnumerateArray().ToArray();
		elements[0].GetString().Should().Be("test1");
		elements[1].GetString().Should().Be("test2");
	}

	[Fact]
	public void JsonArray_MixedTypes_CreatesJsonArray()
	{
		var expression = new ExtendedExpression("jsonArray(jsonDocument('a', 1, 'b', null), jsonDocument('a', 2, 'b', 'woo'), null)");
		var result = expression.Evaluate() as JsonDocument;
		result.Should().BeOfType<JsonDocument>();
		result.Should().NotBeNull();
		result!.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
		result.RootElement.GetArrayLength().Should().Be(3);
		
		var elements = result.RootElement.EnumerateArray().ToArray();
		elements[0].ValueKind.Should().Be(JsonValueKind.Object);
		elements[0].GetProperty("a").GetInt32().Should().Be(1);
		elements[0].GetProperty("b").ValueKind.Should().Be(JsonValueKind.Null);
		
		elements[1].ValueKind.Should().Be(JsonValueKind.Object);
		elements[1].GetProperty("a").GetInt32().Should().Be(2);
		elements[1].GetProperty("b").GetString().Should().Be("woo");
		
		elements[2].ValueKind.Should().Be(JsonValueKind.Null);
	}
}