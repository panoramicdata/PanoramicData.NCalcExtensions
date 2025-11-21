namespace PanoramicData.NCalcExtensions.Test;

public class SetPropertiesTests
{
	[Fact]
	public void SetProperties_OnJObject_CreatesJObject()
	{
		var expression = new ExtendedExpression("setProperties(jObject('a', 1, 'b', null), 'c', 'X')");
		var result = expression.Evaluate() as JObject;
		result.Should().BeOfType<JObject>();
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result!["a"].Should().BeOfType<JValue>();
		result!["a"].Should().BeEquivalentTo(JToken.FromObject(1));
		result!["b"].Should().BeOfType<JValue>();
		result!["b"].Should().BeEquivalentTo(JValue.CreateNull());
		result!["c"].Should().BeEquivalentTo(JToken.FromObject("X"));
	}

	[Fact]
	public void SetProperties_OnAnonymous_CreatesJObject()
	{
		var expression = new ExtendedExpression("setProperties(anon, 'c', 'X')");
		expression.Parameters["anon"] = new { a = 1, b = (string?)null };
		var result = expression.Evaluate() as JObject;
		result.Should().BeOfType<JObject>();
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result!["a"].Should().BeOfType<JValue>();
		result!["a"].Should().BeEquivalentTo(JToken.FromObject(1));
		result!["b"].Should().BeOfType<JValue>();
		result!["b"].Should().BeEquivalentTo(JValue.CreateNull());
		result!["c"].Should().BeEquivalentTo(JToken.FromObject("X"));
	}

	[Fact]
	public void SetProperties_MultipleProperties_Succeeds()
	{
		var expression = new ExtendedExpression("setProperties(jObject('a', 1), 'b', 2, 'c', 3)");
		var result = expression.Evaluate() as JObject;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result!["a"]!.Value<int>().Should().Be(1);
		result!["b"]!.Value<int>().Should().Be(2);
		result!["c"]!.Value<int>().Should().Be(3);
	}

	// Error cases
	[Fact]
	public void SetProperties_EvenNumberOfParameters_ThrowsException()
	{
		var expression = new ExtendedExpression("setProperties(jObject('a', 1), 'b')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*odd number of parameters*");
	}

	[Fact]
	public void SetProperties_NullFirstParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("setProperties(null, 'key', 'value')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*first parameter cannot be null*");
	}

	[Fact]
	public void SetProperties_NonStringKey_ThrowsException()
	{
		var expression = new ExtendedExpression("setProperties(jObject('a', 1), 123, 'value')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*requires a string key*");
	}

	[Fact]
	public void SetProperties_DuplicateKey_ThrowsException()
	{
		var expression = new ExtendedExpression("setProperties(jObject('a', 1), 'a', 2)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*can only define property a once*");
	}
}
