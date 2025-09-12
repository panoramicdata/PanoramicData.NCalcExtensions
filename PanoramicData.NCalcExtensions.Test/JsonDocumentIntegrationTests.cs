using System.Collections.Generic;
using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class JsonDocumentIntegrationTests
{
	[Fact]
	public void JsonDocument_Integration_ComplexScenario_Succeeds()
	{
		// Test a complex scenario that uses multiple JsonDocument functions together
		var expression = new ExtendedExpression(@"
			// Create a JsonDocument
			jsonDocument(
				'users', jsonArray(
					jsonDocument('name', 'Alice', 'age', 30, 'active', true),
					jsonDocument('name', 'Bob', 'age', 25, 'active', false),
					jsonDocument('name', 'Charlie', 'age', 35, 'active', true)
				),
				'metadata', jsonDocument('total', 3, 'created', '2024-01-01')
			)
		");

		var result = expression.Evaluate() as JsonDocument;
		result.Should().NotBeNull();
		result!.RootElement.ValueKind.Should().Be(JsonValueKind.Object);

		// Test that we can access nested properties
		var usersArray = result.RootElement.GetProperty("users");
		usersArray.ValueKind.Should().Be(JsonValueKind.Array);
		usersArray.GetArrayLength().Should().Be(3);

		var metadata = result.RootElement.GetProperty("metadata");
		metadata.ValueKind.Should().Be(JsonValueKind.Object);
	}

	[Fact]
	public void JsonDocument_Integration_WithGetProperty_Succeeds()
	{
		// Test integration between jsonDocument creation and getProperty
		var expression = new ExtendedExpression("getProperty(jsonDocument('data', jsonDocument('nested', 'value')), 'data')");
		var result = expression.Evaluate();
		result.Should().BeOfType<JsonElement>();

		var jsonElement = (JsonElement)result;
		jsonElement.ValueKind.Should().Be(JsonValueKind.Object);
	}

	[Fact]
	public void JsonDocument_Integration_WithGetProperties_Succeeds()
	{
		// Test integration between jsonDocument creation and getProperties
		var expression = new ExtendedExpression("getProperties(jsonDocument('name', 'test', 'age', 30, 'active', true))");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result.Should().Contain("name");
		result.Should().Contain("age");
		result.Should().Contain("active");
	}

	[Fact]
	public void JsonDocument_Integration_WithParse_Succeeds()
	{
		// Test that parsing JSON strings creates compatible JsonDocuments
		var expression = new ExtendedExpression("getProperty(parse('JsonDocument', '{\"key\": \"value\", \"number\": 42}'), 'key')");
		var result = expression.Evaluate();
		result.Should().Be("value");
	}

	[Fact]
	public void JsonDocument_Integration_ArrayParsing_Succeeds()
	{
		// Test array parsing and access
		var expression = new ExtendedExpression("parse('JsonArray', '[{\"name\": \"item1\"}, {\"name\": \"item2\"}]')");
		var result = expression.Evaluate() as JsonDocument;
		result.Should().NotBeNull();
		result!.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
		result.RootElement.GetArrayLength().Should().Be(2);
	}

	[Fact]
	public void JsonDocument_Integration_NullHandling_Succeeds()
	{
		// Test that null values are handled correctly across different functions
		var expression1 = new ExtendedExpression("isNull(getProperty(jsonDocument('nullValue', null), 'nullValue'))");
		var result1 = expression1.Evaluate();
		result1.Should().Be(true);

		var expression2 = new ExtendedExpression("isNullOrEmpty(getProperty(jsonDocument('emptyString', ''), 'emptyString'))");
		var result2 = expression2.Evaluate();
		result2.Should().Be(true);

		var expression3 = new ExtendedExpression("isNullOrWhiteSpace(getProperty(jsonDocument('whitespace', '   '), 'whitespace'))");
		var result3 = expression3.Evaluate();
		result3.Should().Be(true);
	}

	[Fact]
	public void JsonDocument_Integration_TypeConversions_Succeeds()
	{
		// Test that different data types are properly converted
		var jsonDoc = JsonDocument.Parse("{\"string\": \"text\", \"number\": 42, \"boolean\": true, \"null\": null}");

		var expression1 = new ExtendedExpression("getProperty(source, 'string')");
		expression1.Parameters["source"] = jsonDoc;
		var result1 = expression1.Evaluate();
		result1.Should().Be("text");

		var expression2 = new ExtendedExpression("getProperty(source, 'number')");
		expression2.Parameters["source"] = jsonDoc;
		var result2 = expression2.Evaluate();
		result2.Should().Be(42);

		var expression3 = new ExtendedExpression("getProperty(source, 'boolean')");
		expression3.Parameters["source"] = jsonDoc;
		var result3 = expression3.Evaluate();
		result3.Should().Be(true);

		var expression4 = new ExtendedExpression("getProperty(source, 'null')");
		expression4.Parameters["source"] = jsonDoc;
		var result4 = expression4.Evaluate();
		result4.Should().BeNull();
	}

	[Fact]
	public void JsonDocument_Integration_MixedWithJObject_Succeeds()
	{
		// Test that JsonDocument and JObject can work together in expressions
		var expression = new ExtendedExpression("toString(typeOf(jsonDoc))");
		expression.Parameters["jsonDoc"] = JsonDocument.Parse("{\"test\": \"value\"}");
		var result = expression.Evaluate();
		result.Should().Be("JsonDocument");

		var expression2 = new ExtendedExpression("toString(typeOf(jObj))");
		expression2.Parameters["jObj"] = JObject.Parse("{\"test\": \"value\"}");
		var result2 = expression2.Evaluate();
		result2.Should().Be("JObject");
	}
}