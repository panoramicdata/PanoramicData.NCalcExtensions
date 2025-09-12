using System.Collections.Generic;
using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class JsonDocumentValidationTests
{
	[Fact]
	public void JsonDocument_BasicFunctionality_Works()
	{
		// Test that the basic jsonDocument function works
		var expression = new ExtendedExpression("jsonDocument('test', 'value')");
		var result = expression.Evaluate();
		result.Should().BeOfType<JsonDocument>();

		var jsonDoc = result as JsonDocument;
		jsonDoc.Should().NotBeNull();
		jsonDoc!.RootElement.GetProperty("test").GetString().Should().Be("value");
	}

	[Fact]
	public void JsonArray_BasicFunctionality_Works()
	{
		// Test that the basic jsonArray function works
		var expression = new ExtendedExpression("jsonArray(1, 2, 3)");
		var result = expression.Evaluate();
		result.Should().BeOfType<JsonDocument>();

		var jsonDoc = result as JsonDocument;
		jsonDoc.Should().NotBeNull();
		jsonDoc!.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
		jsonDoc.RootElement.GetArrayLength().Should().Be(3);
	}

	[Fact]
	public void GetProperty_JsonDocument_Works()
	{
		// Test that getProperty works with JsonDocument
		var expression = new ExtendedExpression("getProperty(jsonDocument('name', 'John'), 'name')");
		var result = expression.Evaluate();
		result.Should().Be("John");
	}

	[Fact]
	public void GetProperties_JsonDocument_Works()
	{
		// Test that getProperties works with JsonDocument
		var expression = new ExtendedExpression("getProperties(jsonDocument('a', 1, 'b', 2))");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(2);
		result.Should().Contain("a");
		result.Should().Contain("b");
	}

	[Fact]
	public void Parse_JsonDocument_Works()
	{
		// Test that parse function works with JsonDocument
		var expression = new ExtendedExpression("parse('JsonDocument', '{\"key\": \"value\"}')");
		var result = expression.Evaluate();
		result.Should().BeOfType<JsonDocument>();

		var jsonDoc = result as JsonDocument;
		jsonDoc.Should().NotBeNull();
		jsonDoc!.RootElement.GetProperty("key").GetString().Should().Be("value");
	}
}