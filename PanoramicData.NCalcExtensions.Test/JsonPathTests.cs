using System.Collections.Generic;
using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class JsonPathTests : NCalcTest
{
	private const string Bob = "{\"name\": \"bob\", \"numbers\": [1, 2]}";

	[Theory]
	[InlineData("getProperty(source, 'name')", Bob, "bob")]
	[InlineData("getProperty(source, 'nonexistent')", "{\"name\": \"bob\"}", null)]
	[InlineData("toString(typeOf(source))", "{\"name\": \"bob\"}", "JsonDocument")]
	public void JsonPath_OfJsonDocument_ReturnsExpectedValue(string expressionText, string json, object? expectedOutput)
		=> Test(expressionText, "source", JsonDocument.Parse(json)).Should().Be(expectedOutput);

	[Theory]
	[InlineData("getProperty(source, 'numbers')", Bob, JsonValueKind.Array)]
	[InlineData("getProperty(source, 'details')", "{\"name\": \"bob\", \"details\": {\"age\": 30, \"city\": \"NYC\"}}", JsonValueKind.Object)]
	public void JsonPath_OfJsonDocument_ReturnsElementOfExpectedKind(string expressionText, string json, JsonValueKind expectedKind)
	{
		var result = Test(expressionText, "source", JsonDocument.Parse(json));
		result.Should().BeOfType<JsonElement>();
		((JsonElement)result!).ValueKind.Should().Be(expectedKind);
	}

	[Fact]
	public void JsonPath_OfJsonDocument_ArrayProperty_HasExpectedLength()
		=> ((JsonElement)Test("getProperty(source, 'numbers')", "source", JsonDocument.Parse(Bob))!)
			.GetArrayLength().Should().Be(2);

	[Fact]
	public void JsonPath_OfJsonDocument_PropertyNames_Succeeds()
	{
		var json = "{\"name\": \"bob\", \"numbers\": [1, 2], \"active\": true}";
		var result = Test("getProperties(source)", "source", JsonDocument.Parse(json)) as List<string>;

		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result.Should().Contain(["name", "numbers", "active"]);
	}

	[Fact]
	public void JsonPath_OfJsonElement_PropertyAccess_Succeeds()
	{
		var element = JsonDocument.Parse("{\"person\": {\"name\": \"alice\", \"age\": 25}}").RootElement.GetProperty("person");
		Test("getProperty(source, 'name')", "source", element).Should().Be("alice");
	}

	[Fact]
	public void JsonPath_OfJsonElement_TypeChecking_Succeeds()
		=> Test("toString(typeOf(source))", "source", JsonDocument.Parse("{\"name\": \"bob\"}").RootElement)
			.Should().Be("JsonElement");
}
