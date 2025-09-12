using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class JsonDocumentPropertyTypeTests
{
	[Fact]
	public void JsonDocument_PropertyAccess_ReturnsCorrectTypes()
	{
		// Test that JsonDocument properties return the same types as JObject properties
		var jsonDoc = JsonDocument.Parse("{\"intValue\": 42, \"stringValue\": \"test\", \"boolValue\": true, \"nullValue\": null}");
		
		// Test integer property
		var expression1 = new ExtendedExpression("getProperty(source, 'intValue')");
		expression1.Parameters["source"] = jsonDoc;
		var result1 = expression1.Evaluate();
		result1.Should().BeOfType<int>();
		result1.Should().Be(42);

		// Test string property
		var expression2 = new ExtendedExpression("getProperty(source, 'stringValue')");
		expression2.Parameters["source"] = jsonDoc;
		var result2 = expression2.Evaluate();
		result2.Should().BeOfType<string>();
		result2.Should().Be("test");

		// Test boolean property
		var expression3 = new ExtendedExpression("getProperty(source, 'boolValue')");
		expression3.Parameters["source"] = jsonDoc;
		var result3 = expression3.Evaluate();
		result3.Should().BeOfType<bool>();
		result3.Should().Be(true);

		// Test null property
		var expression4 = new ExtendedExpression("getProperty(source, 'nullValue')");
		expression4.Parameters["source"] = jsonDoc;
		var result4 = expression4.Evaluate();
		result4.Should().BeNull();
	}

	[Fact]
	public void JsonDocument_Vs_JObject_TypeCompatibility()
	{
		// Ensure JsonDocument and JObject return the same types for equivalent data
		var jsonDoc = JsonDocument.Parse("{\"value\": 123}");
		var jObject = JObject.Parse("{\"value\": 123}");

		var jsonDocExpression = new ExtendedExpression("getProperty(source, 'value')");
		jsonDocExpression.Parameters["source"] = jsonDoc;
		var jsonDocResult = jsonDocExpression.Evaluate();

		var jObjectExpression = new ExtendedExpression("getProperty(source, 'value')");
		jObjectExpression.Parameters["source"] = jObject;
		var jObjectResult = jObjectExpression.Evaluate();

		// Both should return the same type and value
		jsonDocResult.Should().BeOfType<int>();
		jObjectResult.Should().BeOfType<int>();
		jsonDocResult.Should().Be(jObjectResult);
		jsonDocResult.Should().Be(123);
	}
}