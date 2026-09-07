using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class ParseJsonDocumentTests : NCalcTest
{
	/// <summary>
	/// Evaluates <paramref name="expressionText"/>, requiring it to produce a JsonDocument.
	/// </summary>
	private static JsonDocument EvaluateToJsonDocument(string expressionText)
	{
		var result = Test(expressionText);
		result.Should().BeOfType<JsonDocument>();
		return (JsonDocument)result!;
	}

	[Theory]
	[InlineData("{}")]
	[InlineData("{\"a\":1}")]
	[InlineData("{\"name\":\"John\",\"age\":30}")]
	public void Parse_JsonDocument_Succeeds(string json)
		=> EvaluateToJsonDocument($"parse('JsonDocument', '{json}')")
			.RootElement.ValueKind.Should().Be(JsonValueKind.Object);

	[Theory]
	[InlineData("[]")]
	[InlineData("[1,2,3]")]
	[InlineData("[\"a\",\"b\",\"c\"]")]
	public void Parse_JsonArray_Succeeds(string json)
		=> EvaluateToJsonDocument($"parse('JsonArray', '{json}')")
			.RootElement.ValueKind.Should().Be(JsonValueKind.Array);

	[Theory]
	[InlineData("jsonDocument")]
	[InlineData("System.Text.Json.JsonDocument")]
	public void Parse_JsonDocument_VariousTypeNames_Succeeds(string typeName)
		=> EvaluateToJsonDocument($"parse('{typeName}', '{{\"test\":123}}')").Should().NotBeNull();

	[Theory]
	[InlineData("jsonArray")]
	public void Parse_JsonArray_VariousTypeNames_Succeeds(string typeName)
		=> EvaluateToJsonDocument($"parse('{typeName}', '[1,2,3]')")
			.RootElement.ValueKind.Should().Be(JsonValueKind.Array);

	[Theory]
	[InlineData("parse('JsonDocument', '{invalid json}')")]
	[InlineData("parse('JsonArray', '{\"not\":\"array\"}')")]
	public void Parse_InvalidJson_ThrowsException(string expressionText)
		=> new ExtendedExpression(expressionText)
			.Invoking(expression => expression.Evaluate())
			.Should().Throw<FormatException>();

	[Fact]
	public void Parse_JsonDocument_WithFallback_ReturnsFallback()
		=> Test("parse('JsonDocument', '{invalid}', 'fallback')").Should().Be("fallback");
}
