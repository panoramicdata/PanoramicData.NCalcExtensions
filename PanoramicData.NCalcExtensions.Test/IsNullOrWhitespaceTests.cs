using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class IsNullOrWhiteSpaceTests
{
	[Theory]
	[InlineData("'a'", false)]
	[InlineData("' '", true)]
	[InlineData("null", true)]
	[InlineData("''", true)]
	public void IsNullOrWhiteSpace_UsingInlineData_ResultMatchesExpected(string parameter, bool expectedValue)
		=> new ExtendedExpression($"isNullOrWhiteSpace({parameter})").Evaluate().Should().Be(expectedValue);

	[Theory]
	[InlineData("'a', 'a'")]
	[InlineData("")]
	[InlineData("'a', null, null")]
	public void IsNullOrWhiteSpace_UsingAnIncorrectAmountOfParameters_ThrowsException(string parameter)
		=> new ExtendedExpression($"isNullOrWhiteSpace({parameter})")
		.Invoking(x => x.Evaluate())
		.Should()
		.Throw<FormatException>().WithMessage("isNullOrWhiteSpace() requires one parameter.");

	[Fact]
	public void IsNullOrWhiteSpace_JTokenNull_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isNullOrWhiteSpace(value)");
		expression.Parameters["value"] = JValue.CreateNull();
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void IsNullOrWhiteSpace_JsonElement_WhiteSpace_ReturnsTrue()
	{
		var jsonDoc = JsonDocument.Parse("{\"value\": \"  \"}");
		var expression = new ExtendedExpression("isNullOrWhiteSpace(value)");
		expression.Parameters["value"] = jsonDoc.RootElement.GetProperty("value");
		expression.Evaluate().Should().Be(true);
	}
}

public class IsNullOrEmptyTests
{
	[Theory]
	[InlineData("'a'", false)]
	[InlineData("' '", false)]
	[InlineData("null", true)]
	[InlineData("''", true)]
	public void IsNullOrEmpty_UsingInlineData_ResultMatchesExpected(string parameter, bool expectedValue)
		=> new ExtendedExpression($"isNullOrEmpty({parameter})").Evaluate().Should().Be(expectedValue);

	[Theory]
	[InlineData("'a', 'a'")]
	[InlineData("")]
	[InlineData("'a', null, null")]
	public void IsNullOrEmpty_UsingAnIncorrectAmountOfParameters_ThrowsException(string parameter)
		=> new ExtendedExpression($"isNullOrEmpty({parameter})")
		.Invoking(x => x.Evaluate())
		.Should()
		.Throw<FormatException>().WithMessage("isNullOrEmpty() requires one parameter.");

	[Fact]
	public void IsNullOrEmpty_JTokenNull_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isNullOrEmpty(value)");
		expression.Parameters["value"] = JValue.CreateNull();
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void IsNullOrEmpty_JsonElement_Empty_ReturnsTrue()
	{
		var jsonDoc = JsonDocument.Parse("{\"value\": \"\"}");
		var expression = new ExtendedExpression("isNullOrEmpty(value)");
		expression.Parameters["value"] = jsonDoc.RootElement.GetProperty("value");
		expression.Evaluate().Should().Be(true);
	}
}
