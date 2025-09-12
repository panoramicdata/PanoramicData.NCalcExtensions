using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class ParseJsonDocumentTests
{
	[Theory]
	[InlineData("{}")]
	[InlineData("{\"a\":1}")]
	[InlineData("{\"name\":\"John\",\"age\":30}")]
	public void Parse_JsonDocument_Succeeds(string json)
	{
		var expression = new ExtendedExpression($"parse('JsonDocument', '{json}')");
		var result = expression.Evaluate();
		result.Should().BeOfType<JsonDocument>();
		
		var jsonDoc = result as JsonDocument;
		jsonDoc.Should().NotBeNull();
		jsonDoc!.RootElement.ValueKind.Should().Be(JsonValueKind.Object);
	}

	[Theory]
	[InlineData("[]")]
	[InlineData("[1,2,3]")]
	[InlineData("[\"a\",\"b\",\"c\"]")]
	public void Parse_JsonArray_Succeeds(string json)
	{
		var expression = new ExtendedExpression($"parse('JsonArray', '{json}')");
		var result = expression.Evaluate();
		result.Should().BeOfType<JsonDocument>();
		
		var jsonDoc = result as JsonDocument;
		jsonDoc.Should().NotBeNull();
		jsonDoc!.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
	}

	[Theory]
	[InlineData("jsonDocument")]
	[InlineData("System.Text.Json.JsonDocument")]
	public void Parse_JsonDocument_VariousTypeNames_Succeeds(string typeName)
	{
		var expression = new ExtendedExpression($"parse('{typeName}', '{{\"test\":123}}')");
		var result = expression.Evaluate();
		result.Should().BeOfType<JsonDocument>();
	}

	[Theory]
	[InlineData("jsonArray")]
	public void Parse_JsonArray_VariousTypeNames_Succeeds(string typeName)
	{
		var expression = new ExtendedExpression($"parse('{typeName}', '[1,2,3]')");
		var result = expression.Evaluate();
		result.Should().BeOfType<JsonDocument>();
		
		var jsonDoc = result as JsonDocument;
		jsonDoc!.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
	}

	[Fact]
	public void Parse_JsonDocument_InvalidJson_ThrowsException()
	{
		var expression = new ExtendedExpression("parse('JsonDocument', '{invalid json}')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}

	[Fact]
	public void Parse_JsonArray_ObjectInstead_ThrowsException()
	{
		var expression = new ExtendedExpression("parse('JsonArray', '{\"not\":\"array\"}')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}

	[Fact]
	public void Parse_JsonDocument_WithFallback_ReturnsFallback()
	{
		var expression = new ExtendedExpression("parse('JsonDocument', '{invalid}', 'fallback')");
		var result = expression.Evaluate();
		result.Should().Be("fallback");
	}
}