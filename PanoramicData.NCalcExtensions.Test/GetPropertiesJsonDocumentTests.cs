using System.Collections.Generic;
using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class GetPropertiesJsonDocumentTests : NCalcTest
{
	[Theory]
	[InlineData("getProperties(jsonDocument('name', 'John', 'age', 30, 'active', true))", "name,age,active")]
	[InlineData("getProperties(jsonDocument())", "")]
	[InlineData("getProperties(parse('JsonDocument', '{\"A\": 1, \"B\": 2}'))", "A,B")]
	// A nested object contributes only its own name.
	[InlineData("getProperties(jsonDocument('outer', jsonDocument('inner', 'value'), 'simple', 'text'))", "outer,simple")]
	public void GetProperties_OfJsonDocument_ReturnsExpectedPropertyNames(string expressionText, string expectedNames)
		=> ShouldHaveProperties(Test(expressionText), expectedNames);

	[Fact]
	public void GetProperties_OfJsonElement_ReturnsExpectedPropertyNames()
	{
		var element = JsonDocument.Parse("{\"person\": {\"name\": \"Jane\", \"age\": 25}}").RootElement.GetProperty("person");
		ShouldHaveProperties(Test("getProperties(source)", "source", element), "name,age");
	}

	// getProperties rejects anything that is not an object, consistently with getProperty.

	[Fact]
	public void GetProperties_OfJsonArray_ThrowsFormatException()
		=> TestShouldThrow<FormatException>(
			"getProperties(jsonArray(1, 2, 3))",
			"*must be an object to get properties*");

	[Fact]
	public void GetProperties_OfNonObjectJsonElement_ThrowsFormatException()
		=> TestShouldThrow<FormatException>(
			"getProperties(source)",
			"source",
			JsonDocument.Parse("{\"stringValue\": \"test\", \"numberValue\": 42}").RootElement.GetProperty("stringValue"),
			"*must be an object to get properties*");

	/// <summary>
	/// Asserts that <paramref name="result"/> is exactly the comma-separated property names in
	/// <paramref name="expectedNames"/>.
	/// </summary>
	private static void ShouldHaveProperties(object? result, string expectedNames)
	{
		string[] expected = expectedNames.Length == 0 ? [] : expectedNames.Split(',');
		var names = result as List<string>;

		names.Should().NotBeNull();
		names.Should().HaveCount(expected.Length);
		if (expected.Length > 0)
		{
			names.Should().Contain(expected);
		}
	}
}
