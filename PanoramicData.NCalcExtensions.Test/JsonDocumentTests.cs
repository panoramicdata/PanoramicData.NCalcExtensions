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

	[Fact]
	public void JsonDocument_EmptyJsonDocument_Succeeds()
	{
		var expression = new ExtendedExpression("jsonDocument()");
		var result = expression.Evaluate() as JsonDocument;
		result.Should().BeOfType<JsonDocument>();
		result.Should().NotBeNull();
		result.RootElement.EnumerateObject().Should().HaveCount(0);
	}

	[Fact]
	public void JsonDocument_OddNumberOfParameters_ThrowsException()
	{
		var expression = new ExtendedExpression("jsonDocument('a', 1, 'b')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*even number of parameters*");
	}

	[Fact]
	public void JsonDocument_NonStringKey_ThrowsException()
	{
		var expression = new ExtendedExpression("jsonDocument(123, 'value')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*requires a string key*");
	}

	[Fact]
	public void JsonDocument_DuplicateKey_ThrowsException()
	{
		var expression = new ExtendedExpression("jsonDocument('a', 1, 'a', 2)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*can only define property a once*");
	}
}