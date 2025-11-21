using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class IsNullTests
{
	[Theory]
	[InlineData("1", false)]
	[InlineData("'text'", false)]
	[InlineData("null", true)]
	[InlineData("3.14", false)]
	[InlineData("true", false)]
	[InlineData("false", false)]
	[InlineData("0", false)]
	[InlineData("''", false)]
	[InlineData("list(1, 2, 3)", false)]
	[InlineData("list()", false)]
	[InlineData("jArray(1, 2, 3)", false)]
	[InlineData("jObject('key', 'value')", false)]
	[InlineData("now()", false)]
	public void IsNull_VariousValues_ReturnsExpected(string expression, bool expected)
	{
		var extendedExpression = new ExtendedExpression($"isNull({expression})");
		(extendedExpression.Evaluate() as bool?).Should().Be(expected);
	}

	[Fact]
	public void IsNull_NullParameter_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isNull(bob)");
		expression.Parameters["bob"] = null;
		(expression.Evaluate() as bool?).Should().BeTrue();
	}

	[Fact]
	public void IsNull_NonNullParameter_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isNull(myVar)");
		expression.Parameters["myVar"] = 42;
		(expression.Evaluate() as bool?).Should().BeFalse();
	}

	[Fact]
	public void IsNull_JObjectWithJTokenTypeOfNull_ReturnsTrue()
	{
		var theObject = new FormatException(null);
		var jObject = JObject.FromObject(theObject);
		var expression = new ExtendedExpression($"isNull({nameof(jObject)})");
		expression.Parameters.Add(nameof(jObject), jObject["InnerException"]);
		var result = expression.Evaluate();
		(result as bool?).Should().BeTrue();
	}

	[Fact]
	public void IsNull_JsonDocumentWithJTokenTypeOfNull_ReturnsTrue()
	{
		var theObject = new FormatException(null);
		using var jsonDocument = JsonSerializer.SerializeToDocument(theObject);
		var expression = new ExtendedExpression($"isNull({nameof(jsonDocument)})");
		expression.Parameters.Add(nameof(jsonDocument), jsonDocument.RootElement.GetProperty("InnerException"));
		(expression.Evaluate() as bool?).Should().BeTrue();
	}

	[Fact]
	public void IsNull_JObjectWithJTokenTypeOfString_ReturnsFalse()
	{
		var theObject = new FormatException("A message");
		var jObject = JObject.FromObject(theObject);
		var expression = new ExtendedExpression($"isNull({nameof(jObject)})");
		expression.Parameters.Add(nameof(jObject), jObject["Message"]);
		(expression.Evaluate() as bool?).Should().BeFalse();
	}

	[Fact]
	public void IsNull_JsonElementWithString_ReturnsFalse()
	{
		var theObject = new FormatException("A message");
		using var jsonDocument = JsonSerializer.SerializeToDocument(theObject);
		var expression = new ExtendedExpression($"isNull({nameof(jsonDocument)})");
		expression.Parameters.Add(nameof(jsonDocument), jsonDocument.RootElement.GetProperty("Message"));
		(expression.Evaluate() as bool?).Should().BeFalse();
	}

	[Fact]
	public void IsNull_JValue_NonNull_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isNull(theValue)");
		expression.Parameters["theValue"] = new JValue(42);
		(expression.Evaluate() as bool?).Should().BeFalse();
	}

	[Fact]
	public void IsNull_JValue_Null_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isNull(theValue)");
		expression.Parameters["theValue"] = JValue.CreateNull();
		(expression.Evaluate() as bool?).Should().BeTrue();
	}

	[Theory]
	[InlineData("isNull()")]
	[InlineData("isNull(1, 2)")]
	[InlineData("isNull(1, 2, 3)")]
	public void IsNull_WrongParameterCount_ThrowsException(string expression) => new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*requires one parameter*");
}
