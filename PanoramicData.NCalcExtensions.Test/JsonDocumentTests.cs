using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class JsonDocumentTests
{
	[Fact]
	public void JsonDocument_CreatesJsonDocument()
	{
		var expression = new ExtendedExpression("jsonDocument('a', 1, 'b', null)");
		var result = expression.Evaluate() as JsonDocument;
		result.Should().BeOfType<JsonDocument>();
		result.Should().NotBeNull();
		result.RootElement.EnumerateObject().Should().HaveCount(2);
		result.RootElement.GetProperty("a").Should().BeOfType<JsonElement>();
		result.RootElement.GetProperty("a").GetInt32().Should().Be(1);
		result.RootElement.GetProperty("b").Should().BeOfType<JsonElement>();
		result.RootElement.GetProperty("b").ValueKind.Should().Be(JsonValueKind.Null);
	}
}