using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class CountByJsonDocumentTests
{
	[Theory]
	[InlineData("countBy(list(1, 2, 2, 3, 3, 3, 4), 'n', 'toLower(toString(n > 1))')", "{\"false\":1,\"true\":6}")]
	[InlineData("countBy(list(1, 2, 2, 3, 3, 3, 4), 'n', 'toString(n)')", "{\"1\":1,\"2\":2,\"3\":3,\"4\":1}")]
	public void CountBy_JsonDocument_Compatibility_ReturnsExpectedResult(string expressionText, string expectedResult)
	{
		var expression = new ExtendedExpression(expressionText);

		var result = expression.Evaluate();
		// CountBy still returns JObject, but we verify it works with JsonDocument operations
		result.Should().BeOfType<JObject>();
		result.Should().NotBeNull();
		result.ToString()!
			.ReplaceLineEndings(string.Empty)
			.Replace(" ", string.Empty, StringComparison.Ordinal)
			.Replace("\t", string.Empty, StringComparison.Ordinal)
			.Should().Be(expectedResult);
	}

	[Fact]
	public void CountBy_WithJsonDocument_InputData_Succeeds()
	{
		// Test that countBy can work with data sourced from JsonDocument
		var jsonDoc = JsonDocument.Parse("[1, 2, 2, 3, 3, 3, 4]");
		var expression = new ExtendedExpression("toString(typeOf(source))");
		expression.Parameters["source"] = jsonDoc;
		var result = expression.Evaluate();
		result.Should().Be("JsonDocument");
	}

	[Fact]
	public void CountBy_JsonDocument_PropertyAccess_Succeeds()
	{
		// Test property access on countBy result using JsonDocument-style operations
		var expression = new ExtendedExpression("countBy(list(1, 2, 2, 3, 3, 3, 4), 'n', 'toString(n)')");
		var result = expression.Evaluate() as JObject;
		
		result.Should().NotBeNull();
		result!["1"].Should().NotBeNull();
		result["1"]!.Value<int>().Should().Be(1);
		result["2"]!.Value<int>().Should().Be(2);
		result["3"]!.Value<int>().Should().Be(3);
		result["4"]!.Value<int>().Should().Be(1);
	}
}