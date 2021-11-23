namespace PanoramicData.NCalcExtensions.Test;

public class IsNullTests
{
	[Fact]
	public void IsNull_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("isNull(1)");
		Assert.False(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsNull_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("isNull('text')");
		Assert.False(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsNull_Example3_Succeeds()
	{
		var expression = new ExtendedExpression("isNull(bob)");
		expression.Parameters["bob"] = null;
		Assert.True(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsNull_Example4_Succeeds()
	{
		var expression = new ExtendedExpression("isNull(null)");
		Assert.True(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsNull_JObjectWithJTokenTypeOfNull_ReturnsTrue()
	{
		var theObject = new Exception(null);
		var jObject = JObject.FromObject(theObject);
		var expression = new ExtendedExpression($"isNull({nameof(jObject)})");
		expression.Parameters.Add(nameof(jObject), jObject["Message"]);
		Assert.True(expression.Evaluate() as bool?);
	}

	[Fact]
	public void IsNull_JObjectWithJTokenTypeOfString_ReturnsFalse()
	{
		var theObject = new Exception("A message");
		var jObject = JObject.FromObject(theObject);
		var expression = new ExtendedExpression($"isNull({nameof(jObject)})");
		expression.Parameters.Add(nameof(jObject), jObject["Message"]);
		Assert.False(expression.Evaluate() as bool?);
	}
}
