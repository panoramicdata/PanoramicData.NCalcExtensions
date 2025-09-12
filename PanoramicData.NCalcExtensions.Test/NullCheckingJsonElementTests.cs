using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class NullCheckingJsonElementTests
{
	[Fact]
	public void IsNullOrEmpty_JsonElement_Null_ReturnsTrue()
	{
		var jsonDoc = JsonDocument.Parse("{\"value\": null}");
		var expression = new ExtendedExpression("isNullOrEmpty(value)");
		expression.Parameters["value"] = jsonDoc.RootElement.GetProperty("value");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Fact]
	public void IsNullOrEmpty_JsonElement_EmptyString_ReturnsTrue()
	{
		var jsonDoc = JsonDocument.Parse("{\"value\": \"\"}");
		var expression = new ExtendedExpression("isNullOrEmpty(value)");
		expression.Parameters["value"] = jsonDoc.RootElement.GetProperty("value");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Fact]
	public void IsNullOrEmpty_JsonElement_NonEmptyString_ReturnsFalse()
	{
		var jsonDoc = JsonDocument.Parse("{\"value\": \"test\"}");
		var expression = new ExtendedExpression("isNullOrEmpty(value)");
		expression.Parameters["value"] = jsonDoc.RootElement.GetProperty("value");
		var result = expression.Evaluate();
		result.Should().Be(false);
	}

	[Fact]
	public void IsNullOrWhiteSpace_JsonElement_Null_ReturnsTrue()
	{
		var jsonDoc = JsonDocument.Parse("{\"value\": null}");
		var expression = new ExtendedExpression("isNullOrWhiteSpace(value)");
		expression.Parameters["value"] = jsonDoc.RootElement.GetProperty("value");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Fact]
	public void IsNullOrWhiteSpace_JsonElement_WhitespaceString_ReturnsTrue()
	{
		var jsonDoc = JsonDocument.Parse("{\"value\": \"   \"}");
		var expression = new ExtendedExpression("isNullOrWhiteSpace(value)");
		expression.Parameters["value"] = jsonDoc.RootElement.GetProperty("value");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Fact]
	public void IsNullOrWhiteSpace_JsonElement_NonWhitespaceString_ReturnsFalse()
	{
		var jsonDoc = JsonDocument.Parse("{\"value\": \"test\"}");
		var expression = new ExtendedExpression("isNullOrWhiteSpace(value)");
		expression.Parameters["value"] = jsonDoc.RootElement.GetProperty("value");
		var result = expression.Evaluate();
		result.Should().Be(false);
	}

	[Fact]
	public void IsNull_JsonElement_Null_ReturnsTrue()
	{
		var jsonDoc = JsonDocument.Parse("{\"value\": null}");
		var expression = new ExtendedExpression("isNull(value)");
		expression.Parameters["value"] = jsonDoc.RootElement.GetProperty("value");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Fact]
	public void IsNull_JsonElement_NotNull_ReturnsFalse()
	{
		var jsonDoc = JsonDocument.Parse("{\"value\": \"test\"}");
		var expression = new ExtendedExpression("isNull(value)");
		expression.Parameters["value"] = jsonDoc.RootElement.GetProperty("value");
		var result = expression.Evaluate();
		result.Should().Be(false);
	}
}