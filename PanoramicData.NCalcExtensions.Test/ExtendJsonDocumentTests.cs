using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class ExtendJsonDocumentTests
{
	[Fact]
	public void Extend_WithJsonDocument_PropertyAccess_Succeeds()
	{
		// Test property access on JsonDocument similar to JObject extend functionality
		var expression = new ExtendedExpression("getProperty(jsonDocument('first', 1, 'second', null, 'third', 5), 'first')");
		var result = expression.Evaluate();
		result.Should().Be(1);
	}

	[Fact]
	public void Extend_JsonDocument_WithNullValues_Succeeds()
	{
		var expression = new ExtendedExpression("getProperty(jsonDocument('First', 1, 'Second', 2, 'Third', null), 'Third')");
		var result = expression.Evaluate();
		result.Should().BeNull();
	}

	[Fact]
	public void Extend_JsonDocument_AllProperties_Accessible()
	{
		var jsonDoc = JsonDocument.Parse("{\"first\": 1, \"second\": null, \"third\": 5}");
		
		var expression1 = new ExtendedExpression("getProperty(source, 'first')");
		expression1.Parameters["source"] = jsonDoc;
		var result1 = expression1.Evaluate();
		result1.Should().Be(1);

		var expression2 = new ExtendedExpression("getProperty(source, 'second')");
		expression2.Parameters["source"] = jsonDoc;
		var result2 = expression2.Evaluate();
		result2.Should().BeNull();

		var expression3 = new ExtendedExpression("getProperty(source, 'third')");
		expression3.Parameters["source"] = jsonDoc;
		var result3 = expression3.Evaluate();
		result3.Should().Be(5);
	}

	[Fact]
	public void Extend_JsonDocument_MultiplePropertyTypes_Succeeds()
	{
		var expression = new ExtendedExpression("jsonDocument('First', 1, 'Second', 2, 'Third', null, 'Fourth', 'text', 'Fifth', true)");
		var result = expression.Evaluate() as JsonDocument;
		
		result.Should().NotBeNull();
		result!.RootElement.ValueKind.Should().Be(JsonValueKind.Object);
		result.RootElement.EnumerateObject().Should().HaveCount(5);
		
		result.RootElement.GetProperty("First").GetInt32().Should().Be(1);
		result.RootElement.GetProperty("Second").GetInt32().Should().Be(2);
		result.RootElement.GetProperty("Third").ValueKind.Should().Be(JsonValueKind.Null);
		result.RootElement.GetProperty("Fourth").GetString().Should().Be("text");
		result.RootElement.GetProperty("Fifth").GetBoolean().Should().BeTrue();
	}
}