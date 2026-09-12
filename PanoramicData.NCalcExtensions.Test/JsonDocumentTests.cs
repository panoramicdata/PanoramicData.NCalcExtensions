using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class JsonDocumentTests : NCalcTest
{
	[Fact]
	public void JsonDocument_CreatesJsonDocument()
	{
		var result = Test("jsonDocument('a', 1, 'b', null)") as JsonDocument;
		result.Should().BeOfType<JsonDocument>();
		result.Should().NotBeNull();
		result.RootElement.EnumerateObject().Should().HaveCount(2);
		result.RootElement.GetProperty("a").Should().BeOfType<JsonElement>();
		result.RootElement.GetProperty("a").GetInt32().Should().Be(1);
		result.RootElement.GetProperty("b").Should().BeOfType<JsonElement>();
		result.RootElement.GetProperty("b").ValueKind.Should().Be(JsonValueKind.Null);
	}

	[Fact]
	public void JsonDocument_EmptyJsonDocument_Succeeds()
	{
		var result = Test("jsonDocument()") as JsonDocument;
		result.Should().BeOfType<JsonDocument>();
		result.Should().NotBeNull();
		result.RootElement.EnumerateObject().Should().HaveCount(0);
	}

	[Theory]
	[InlineData("jsonDocument('a', 1, 'b')", "*even number of parameters*")]
	[InlineData("jsonDocument(123, 'value')", "*requires a string key*")]
	[InlineData("jsonDocument('a', 1, 'a', 2)", "*can only define property a once*")]
	public void JsonDocument_WithInvalidParameters_ThrowsFormatException(string expressionText, string messagePattern)
		=> TestShouldThrowExactly<FormatException>(expressionText, messagePattern);
}
