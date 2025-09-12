using System.Collections.Generic;
using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class GetPropertiesJsonDocumentTests
{
	[Fact]
	public void GetProperties_JsonDocument_ReturnsPropertyNames()
	{
		var expression = new ExtendedExpression("getProperties(jsonDocument('name', 'John', 'age', 30, 'active', true))");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result.Should().Contain("name");
		result.Should().Contain("age");
		result.Should().Contain("active");
	}

	[Fact]
	public void GetProperties_EmptyJsonDocument_ReturnsEmptyList()
	{
		var expression = new ExtendedExpression("getProperties(jsonDocument())");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().BeEmpty();
	}

	[Fact]
	public void GetProperties_JsonElement_ReturnsPropertyNames()
	{
		var jsonDoc = JsonDocument.Parse("{\"person\": {\"name\": \"Jane\", \"age\": 25}}");
		var expression = new ExtendedExpression("getProperties(person)");
		expression.Parameters["person"] = jsonDoc.RootElement.GetProperty("person");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(2);
		result.Should().Contain("name");
		result.Should().Contain("age");
	}

	[Fact]
	public void GetProperties_JsonDocumentArray_ThrowsException()
	{
		// Ensure getProperties throws exception for arrays, consistent with getProperty behavior
		var expression = new ExtendedExpression("getProperties(jsonArray(1, 2, 3))");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*must be an object to get properties*");
	}

	[Fact]
	public void GetProperties_JsonDocument_FromParsedJson_Succeeds()
	{
		var expression = new ExtendedExpression("getProperties(parse('JsonDocument', '{\"A\": 1, \"B\": 2}'))");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(2);
		result.Should().Contain("A");
		result.Should().Contain("B");
	}

	[Fact]
	public void GetProperties_JsonDocument_NestedObject_ReturnsTopLevelProperties()
	{
		var expression = new ExtendedExpression("getProperties(jsonDocument('outer', jsonDocument('inner', 'value'), 'simple', 'text'))");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(2);
		result.Should().Contain("outer");
		result.Should().Contain("simple");
	}

	[Fact]
	public void GetProperties_JsonElement_NonObject_ThrowsException()
	{
		var jsonDoc = JsonDocument.Parse("{\"stringValue\": \"test\", \"numberValue\": 42}");
		var expression = new ExtendedExpression("getProperties(stringElement)");
		expression.Parameters["stringElement"] = jsonDoc.RootElement.GetProperty("stringValue");

		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*must be an object to get properties*");
	}
}