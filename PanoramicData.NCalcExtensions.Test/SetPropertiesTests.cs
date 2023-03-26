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
		result["a"].Should().BeOfType<JValue>();
		result["a"].Should().BeEquivalentTo(JToken.FromObject(1));
		result["b"].Should().BeOfType<JValue>();
		result["b"].Should().BeEquivalentTo(JValue.CreateNull());
		result["c"].Should().BeEquivalentTo(JToken.FromObject("X"));
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
		result["a"].Should().BeOfType<JValue>();
		result["a"].Should().BeEquivalentTo(JToken.FromObject(1));
		result["b"].Should().BeOfType<JValue>();
		result["b"].Should().BeEquivalentTo(JValue.CreateNull());
		result["c"].Should().BeEquivalentTo(JToken.FromObject("X"));
	}
}
