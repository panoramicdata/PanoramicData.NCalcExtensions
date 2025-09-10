namespace PanoramicData.NCalcExtensions.Test;

public class ExtendTests
{
	[Fact]
	public void Extend_WithInt_Succeeds()
	{
		var expression = new ExtendedExpression("extend(jObject('first', 1, 'second', null), list('third', 5))");
		var result = expression.Evaluate() as JObject;

		result.Should().NotBeNull();
		result.Should().HaveCount(3);

		var a = result["first"];
		a.Should().NotBeNull();
		a.Value<int>().Should().Be(1);

		var b = result["second"];
		b.Should().NotBeNull();
		b.Value<int?>().Should().BeNull();

		var c = result["third"];
		c.Should().NotBeNull();
		c.Value<int>().Should().Be(5);
	}

	[Theory]
	[InlineData("extend(jObject('First', 1, 'Second', 2), list('Third', null))")]
	[InlineData("extend(jObject('First', 1), list('Second', 2, 'Third', null))")]
	public void Extend_WithNull_Succeeds(string expressionString)
	{
		var expression = new ExtendedExpression(expressionString);
		var result = expression.Evaluate() as JObject;

		result.Should().NotBeNull();
		result.Should().HaveCount(3);

		var first = result["First"];
		first.Should().NotBeNull();
		first.Value<int>().Should().Be(1);

		var second = result["Second"];
		second.Should().NotBeNull();
		second.Value<int>().Should().Be(2);

		var third = result["Third"];
		third.Should().NotBeNull();
		third.Value<string?>().Should().Be(null);
	}
}