namespace PanoramicData.NCalcExtensions.Test;

public class JObjectTests
{
	[Fact]
	public void JObject_CreatesJObject()
	{
		var expression = new ExtendedExpression("jObject('a', 1, 'b', null)");
		var result = expression.Evaluate() as JObject;
		result.Should().BeOfType<JObject>();
		result.Should().NotBeNull();
		result.Should().HaveCount(2);
		result!["a"].Should().BeOfType<JValue>();
		result!["a"].Should().BeEquivalentTo(JToken.FromObject(1));
		result!["b"].Should().BeOfType<JValue>();
		result!["b"].Should().BeEquivalentTo(JValue.CreateNull());
	}

	[Fact]
	public void JObject_EmptyJObject_Succeeds()
	{
		var expression = new ExtendedExpression("jObject()");
		var result = expression.Evaluate() as JObject;
		result.Should().BeOfType<JObject>();
		result.Should().NotBeNull();
		result.Should().HaveCount(0);
	}

	[Fact]
	public void JObject_OddNumberOfParameters_ThrowsException()
	{
		var expression = new ExtendedExpression("jObject('a', 1, 'b')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*even number of parameters*");
	}

	[Fact]
	public void JObject_NonStringKey_ThrowsException()
	{
		var expression = new ExtendedExpression("jObject(123, 'value')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*requires a string key*");
	}

	[Fact]
	public void JObject_DuplicateKey_ThrowsException()
	{
		var expression = new ExtendedExpression("jObject('a', 1, 'a', 2)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*can only define property a once*");
	}
}
