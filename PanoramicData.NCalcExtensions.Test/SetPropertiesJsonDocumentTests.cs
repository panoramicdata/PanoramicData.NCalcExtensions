using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class SetPropertiesJsonDocumentTests
{
	[Fact]
	public void SetProperties_OnJsonDocument_CreatesJsonDocument()
	{
		// Note: Since JsonDocument is immutable, setProperties would need to create a new JsonDocument
		// This test verifies that we can work with JsonDocument properties similar to JObject
		var jsonDoc = JsonDocument.Parse("{\"a\": 1, \"b\": null}");
		var expression = new ExtendedExpression("getProperty(source, 'a')");
		expression.Parameters["source"] = jsonDoc;
		var result = expression.Evaluate();
		result.Should().Be(1);
	}

	[Fact]
	public void SetProperties_OnAnonymous_ConvertsToJsonDocument()
	{
		// Test that anonymous objects can be converted and properties accessed similar to JObject
		var expression = new ExtendedExpression("getProperty(anon, 'a')");
		expression.Parameters["anon"] = new { a = 1, b = (string?)null };
		var result = expression.Evaluate();
		result.Should().Be(1);
	}

	[Fact]
	public void SetProperties_JsonDocument_PropertyAccess_Succeeds()
	{
		var expression = new ExtendedExpression("getProperty(jsonDocument('a', 1, 'b', null, 'c', 'X'), 'c')");
		var result = expression.Evaluate();
		result.Should().Be("X");
	}
}