using System.Collections.Generic;
using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class JsonPathTests
{
	[Fact]
	public void JsonPath_JsonDocument_PropertyAccess_Succeeds()
	{
		// Test basic property access on JsonDocument (equivalent to JPath basic functionality)
		var expression = new ExtendedExpression("getProperty(source, 'name')");
		var jsonDoc = JsonDocument.Parse("{\"name\": \"bob\", \"numbers\": [1, 2]}");
		expression.Parameters["source"] = jsonDoc;
		var result = expression.Evaluate();
		result.Should().Be("bob");
	}

	[Fact]
	public void JsonPath_JsonDocument_ArrayAccess_Succeeds()
	{
		// Test array property access
		var expression = new ExtendedExpression("getProperty(source, 'numbers')");
		var jsonDoc = JsonDocument.Parse("{\"name\": \"bob\", \"numbers\": [1, 2]}");
		expression.Parameters["source"] = jsonDoc;
		var result = expression.Evaluate();
		result.Should().BeOfType<JsonElement>();

		var jsonElement = (JsonElement)result;
		jsonElement.ValueKind.Should().Be(JsonValueKind.Array);
		jsonElement.GetArrayLength().Should().Be(2);
	}

	[Fact]
	public void JsonPath_JsonDocument_NestedObject_Succeeds()
	{
		var expression = new ExtendedExpression("getProperty(source, 'details')");
		var jsonDoc = JsonDocument.Parse("{\"name\": \"bob\", \"details\": {\"age\": 30, \"city\": \"NYC\"}}");
		expression.Parameters["source"] = jsonDoc;
		var result = expression.Evaluate();
		result.Should().BeOfType<JsonElement>();

		var jsonElement = (JsonElement)result;
		jsonElement.ValueKind.Should().Be(JsonValueKind.Object);
	}

	[Fact]
	public void JsonPath_JsonDocument_PropertyNames_Succeeds()
	{
		var expression = new ExtendedExpression("getProperties(source)");
		var jsonDoc = JsonDocument.Parse("{\"name\": \"bob\", \"numbers\": [1, 2], \"active\": true}");
		expression.Parameters["source"] = jsonDoc;
		var result = expression.Evaluate() as List<string>;

		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result.Should().Contain("name");
		result.Should().Contain("numbers");
		result.Should().Contain("active");
	}

	[Fact]
	public void JsonPath_JsonDocument_MissingProperty_ReturnsNull()
	{
		var expression = new ExtendedExpression("getProperty(source, 'nonexistent')");
		var jsonDoc = JsonDocument.Parse("{\"name\": \"bob\"}");
		expression.Parameters["source"] = jsonDoc;
		var result = expression.Evaluate();
		result.Should().BeNull();
	}

	[Fact]
	public void JsonPath_JsonElement_PropertyAccess_Succeeds()
	{
		var jsonDoc = JsonDocument.Parse("{\"person\": {\"name\": \"alice\", \"age\": 25}}");
		var expression = new ExtendedExpression("getProperty(personElement, 'name')");
		expression.Parameters["personElement"] = jsonDoc.RootElement.GetProperty("person");
		var result = expression.Evaluate();
		result.Should().Be("alice");
	}

	[Fact]
	public void JsonPath_JsonDocument_TypeChecking_Succeeds()
	{
		var expression = new ExtendedExpression("toString(typeOf(source))");
		var jsonDoc = JsonDocument.Parse("{\"name\": \"bob\"}");
		expression.Parameters["source"] = jsonDoc;
		var result = expression.Evaluate();
		result.Should().Be("JsonDocument");
	}

	[Fact]
	public void JsonPath_JsonElement_TypeChecking_Succeeds()
	{
		var jsonDoc = JsonDocument.Parse("{\"name\": \"bob\"}");
		var expression = new ExtendedExpression("toString(typeOf(element))");
		expression.Parameters["element"] = jsonDoc.RootElement;
		var result = expression.Evaluate();
		result.Should().Be("JsonElement");
	}
}