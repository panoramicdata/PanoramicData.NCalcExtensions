using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class GetPropertyJsonDocumentTests
{
	[Fact]
	public void GetProperty_JsonDocument_ReturnsCorrectValue()
	{
		var expression = new ExtendedExpression("getProperty(jsonDocument('name', 'John', 'age', 30), 'name')");
		var result = expression.Evaluate();
		result.Should().Be("John");
	}

	[Fact]
	public void GetProperty_JsonDocument_ReturnsNumber()
	{
		var expression = new ExtendedExpression("getProperty(jsonDocument('name', 'John', 'age', 30), 'age')");
		var result = expression.Evaluate();
		result.Should().Be(30);
	}

	[Fact]
	public void GetProperty_JsonDocument_ReturnsNull_ForNullValue()
	{
		var expression = new ExtendedExpression("getProperty(jsonDocument('name', 'John', 'spouse', null), 'spouse')");
		var result = expression.Evaluate();
		result.Should().BeNull();
	}

	[Fact]
	public void GetProperty_JsonDocument_ReturnsNull_ForMissingProperty()
	{
		var expression = new ExtendedExpression("getProperty(jsonDocument('name', 'John'), 'age')");
		var result = expression.Evaluate();
		result.Should().BeNull();
	}

	[Fact]
	public void GetProperty_JsonElement_ReturnsCorrectValue()
	{
		var jsonDoc = JsonDocument.Parse("{\"person\": {\"name\": \"Jane\", \"age\": 25}}");
		var expression = new ExtendedExpression("getProperty(person, 'name')");
		expression.Parameters["person"] = jsonDoc.RootElement.GetProperty("person");
		var result = expression.Evaluate();
		result.Should().Be("Jane");
	}

	[Fact]
	public void GetProperty_JsonDocument_Boolean_ReturnsBoolean()
	{
		var expression = new ExtendedExpression("getProperty(jsonDocument('active', true), 'active')");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Fact]
	public void GetProperty_JsonDocument_FromParsedJson_Succeeds()
	{
		var expression = new ExtendedExpression("getProperty(parse('JsonDocument', '{\"A\": 1, \"B\": 2}'), 'B')");
		var result = expression.Evaluate();
		result.Should().Be(2);
	}

	[Fact]
	public void GetProperty_JsonDocument_NestedObject_ReturnsJsonElement()
	{
		var expression = new ExtendedExpression("getProperty(jsonDocument('nested', jsonDocument('inner', 'value')), 'nested')");
		var result = expression.Evaluate();
		result.Should().BeOfType<JsonElement>();
		
		var jsonElement = (JsonElement)result;
		jsonElement.ValueKind.Should().Be(JsonValueKind.Object);
	}

	[Fact]
	public void GetProperty_JsonDocument_Array_ReturnsJsonElement()
	{
		var expression = new ExtendedExpression("getProperty(jsonDocument('items', jsonArray(1, 2, 3)), 'items')");
		var result = expression.Evaluate();
		result.Should().BeOfType<JsonElement>();
		
		var jsonElement = (JsonElement)result;
		jsonElement.ValueKind.Should().Be(JsonValueKind.Array);
	}

	[Fact]
	public void GetProperty_JsonDocument_InvalidProperty_ThrowsException()
	{
		var expression = new ExtendedExpression("getProperty(jsonArray(1, 2, 3), 'nonexistent')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*must be an object*");
	}

	[Fact]
	public void GetProperty_JsonDocument_MatchesJObjectBehavior()
	{
		// Test that JsonDocument property access returns the same types as JObject
		var expression = new ExtendedExpression("getProperty(parse('JsonDocument', '{\"A\": 1, \"B\": 2}'), 'B')");
		var result = expression.Evaluate();
		result.Should().BeOfType<int>();
		result.Should().Be(2);
	}
}