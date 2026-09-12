using System.Collections.Generic;
using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class JsonDocumentIntegrationTests : NCalcTest
{
	private const string TypedJson = "{\"string\": \"text\", \"number\": 42, \"boolean\": true, \"null\": null}";

	[Fact]
	public void JsonDocument_Integration_ComplexScenario_Succeeds()
	{
		var result = Test(@"
			// Create a JsonDocument
			jsonDocument(
				'users', jsonArray(
					jsonDocument('name', 'Alice', 'age', 30, 'active', true),
					jsonDocument('name', 'Bob', 'age', 25, 'active', false),
					jsonDocument('name', 'Charlie', 'age', 35, 'active', true)
				),
				'metadata', jsonDocument('total', 3, 'created', '2024-01-01')
			)
		") as JsonDocument;

		result.Should().NotBeNull();
		result!.RootElement.ValueKind.Should().Be(JsonValueKind.Object);

		var usersArray = result.RootElement.GetProperty("users");
		usersArray.ValueKind.Should().Be(JsonValueKind.Array);
		usersArray.GetArrayLength().Should().Be(3);

		var metadata = result.RootElement.GetProperty("metadata");
		metadata.ValueKind.Should().Be(JsonValueKind.Object);
	}

	[Fact]
	public void JsonDocument_Integration_WithGetProperty_Succeeds()
	{
		var result = Test("getProperty(jsonDocument('data', jsonDocument('nested', 'value')), 'data')");
		result.Should().BeOfType<JsonElement>();
		((JsonElement)result!).ValueKind.Should().Be(JsonValueKind.Object);
	}

	[Fact]
	public void JsonDocument_Integration_WithGetProperties_Succeeds()
	{
		var result = Test("getProperties(jsonDocument('name', 'test', 'age', 30, 'active', true))") as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result.Should().Contain(["name", "age", "active"]);
	}

	[Fact]
	public void JsonDocument_Integration_ArrayParsing_Succeeds()
	{
		var result = Test("parse('JsonArray', '[{\"name\": \"item1\"}, {\"name\": \"item2\"}]')") as JsonDocument;
		result.Should().NotBeNull();
		result!.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
		result.RootElement.GetArrayLength().Should().Be(2);
	}

	[Theory]
	[InlineData("getProperty(parse('JsonDocument', '{\"key\": \"value\", \"number\": 42}'), 'key')", "value")]
	// Nulls, empty strings and white space read back through the is... functions.
	[InlineData("isNull(getProperty(jsonDocument('nullValue', null), 'nullValue'))", true)]
	[InlineData("isNullOrEmpty(getProperty(jsonDocument('emptyString', ''), 'emptyString'))", true)]
	[InlineData("isNullOrWhiteSpace(getProperty(jsonDocument('whitespace', '   '), 'whitespace'))", true)]
	public void JsonDocument_Integration_ReturnsExpectedValue(string expressionText, object expectedOutput)
		=> Test(expressionText).Should().Be(expectedOutput);

	/// <summary>
	/// Each JSON value kind reads back as the CLR type the expression language uses for it.
	/// </summary>
	[Theory]
	[InlineData("string", "text")]
	[InlineData("number", 42)]
	[InlineData("boolean", true)]
	[InlineData("null", null)]
	public void JsonDocument_Integration_TypeConversions_Succeed(string propertyName, object? expectedOutput)
		=> Test($"getProperty(source, '{propertyName}')", "source", JsonDocument.Parse(TypedJson))
			.Should().Be(expectedOutput);

	[Fact]
	public void JsonDocument_Integration_JsonDocumentParameter_IsTypedAsJsonDocument()
		=> Test("toString(typeOf(source))", "source", JsonDocument.Parse("{\"test\": \"value\"}"))
			.Should().Be("JsonDocument");

	[Fact]
	public void JsonDocument_Integration_JObjectParameter_IsTypedAsJObject()
		=> Test("toString(typeOf(source))", "source", JObject.Parse("{\"test\": \"value\"}"))
			.Should().Be("JObject");
}
