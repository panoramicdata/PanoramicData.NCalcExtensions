using System.Linq;
using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class JsonArrayTests : NCalcTest
{
	/// <summary>
	/// Evaluates <paramref name="expressionText"/>, requiring it to produce a JSON array of
	/// <paramref name="expectedLength"/> elements, and returns those elements.
	/// </summary>
	private static JsonElement[] EvaluateToJsonArray(string expressionText, int expectedLength)
	{
		var result = Test(expressionText);
		result.Should().BeOfType<JsonDocument>();

		var rootElement = ((JsonDocument)result!).RootElement;
		rootElement.ValueKind.Should().Be(JsonValueKind.Array);
		rootElement.GetArrayLength().Should().Be(expectedLength);

		return rootElement.EnumerateArray().ToArray();
	}

	[Fact]
	public void JsonArray_CreatesJsonArray()
	{
		var elements = EvaluateToJsonArray("jsonArray(1, 'test', null, true)", 4);

		elements[0].GetInt32().Should().Be(1);
		elements[1].GetString().Should().Be("test");
		elements[2].ValueKind.Should().Be(JsonValueKind.Null);
		elements[3].GetBoolean().Should().BeTrue();
	}

	[Fact]
	public void JsonArray_EmptyArray_CreatesEmptyJsonArray()
		=> EvaluateToJsonArray("jsonArray()", 0).Should().BeEmpty();

	[Fact]
	public void JsonArray_NestedObjects_CreatesComplexArray()
	{
		var elements = EvaluateToJsonArray("jsonArray(jsonDocument('a', 1), jsonDocument('b', 2))", 2);

		elements[0].ValueKind.Should().Be(JsonValueKind.Object);
		elements[1].ValueKind.Should().Be(JsonValueKind.Object);
	}

	[Fact]
	public void JsonArray_StringParameters_CreatesJsonArray()
	{
		var elements = EvaluateToJsonArray("jsonArray('test1', 'test2')", 2);

		elements[0].GetString().Should().Be("test1");
		elements[1].GetString().Should().Be("test2");
	}

	[Fact]
	public void JsonArray_MixedTypes_CreatesJsonArray()
	{
		var elements = EvaluateToJsonArray(
			"jsonArray(jsonDocument('a', 1, 'b', null), jsonDocument('a', 2, 'b', 'woo'), null)",
			3);

		elements[0].ValueKind.Should().Be(JsonValueKind.Object);
		elements[0].GetProperty("a").GetInt32().Should().Be(1);
		elements[0].GetProperty("b").ValueKind.Should().Be(JsonValueKind.Null);

		elements[1].ValueKind.Should().Be(JsonValueKind.Object);
		elements[1].GetProperty("a").GetInt32().Should().Be(2);
		elements[1].GetProperty("b").GetString().Should().Be("woo");

		elements[2].ValueKind.Should().Be(JsonValueKind.Null);
	}
}
