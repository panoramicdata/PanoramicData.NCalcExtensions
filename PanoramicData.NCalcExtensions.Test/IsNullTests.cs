using System.Text.Json;

namespace PanoramicData.NCalcExtensions.Test;

public class IsNullTests
{
	[Fact]
	public void IsNull_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("isNull(1)");
		(expression.Evaluate() as bool?).Should().BeFalse();
	}

	[Fact]
	public void IsNull_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("isNull('text')");
		(expression.Evaluate() as bool?).Should().BeFalse();
	}

	[Fact]
	public void IsNull_Example3_Succeeds()
	{
		var expression = new ExtendedExpression("isNull(bob)");
		expression.Parameters["bob"] = null;
		(expression.Evaluate() as bool?).Should().BeTrue();
	}

	[Fact]
	public void IsNull_Example4_Succeeds()
	{
		var expression = new ExtendedExpression("isNull(null)");
		(expression.Evaluate() as bool?).Should().BeTrue();
	}

	[Fact]
	public void IsNull_JObjectWithJTokenTypeOfNull_ReturnsTrue()
	{
		var theObject = new FormatException(null);
		var jObject = JObject.FromObject(theObject);
		var expression = new ExtendedExpression($"isNull({nameof(jObject)})");
		expression.Parameters.Add(nameof(jObject), jObject["Message"]);
		(expression.Evaluate() as bool?).Should().BeTrue();
	}


	[Fact]
	public void IsNull_JsonDocumentWithJTokenTypeOfNull_ReturnsTrue()
	{
		var theObject = new FormatException(null);
		using var jsonDocument = JsonSerializer.SerializeToDocument(theObject);
		var expression = new ExtendedExpression($"isNull({nameof(jsonDocument)})");
		expression.Parameters.Add(nameof(jsonDocument), jsonDocument.RootElement.GetProperty("Message"));
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
}
