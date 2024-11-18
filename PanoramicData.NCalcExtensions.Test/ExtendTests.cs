namespace PanoramicData.NCalcExtensions.Test;

public class ExtendTests
{
	[Fact]
	public void Extend_WithInt_Succeeds()
	{
		var expression = new ExtendedExpression("extend(jObject('a', 1, 'b', null), list('c', 5))");
		var result = expression.Evaluate() as JObject;

		result.Should().NotBeNull();
		result.Should().HaveCount(3);

		var a = result["a"];
		a.Should().NotBeNull();
		a.Value<int>().Should().Be(1);

		var b = result["b"];
		b.Should().NotBeNull();
		b.Value<int?>().Should().BeNull();

		var c = result["c"];
		c.Should().NotBeNull();
		c.Value<int>().Should().Be(5);
	}

	[Fact]
	public void Extend_WithNull_Succeeds()
	{
		var expression = new ExtendedExpression("extend(jObject('a', 1, 'b', null), list('c', null))");
		var result = expression.Evaluate() as JObject;

		result.Should().NotBeNull();
		result.Should().HaveCount(3);

		var a = result["a"];
		a.Should().NotBeNull();
		a.Value<int>().Should().Be(1);

		var b = result["b"];
		b.Should().NotBeNull();
		b.Value<int?>().Should().BeNull();

		var c = result["c"];
		c.Should().NotBeNull();
		c.Value<string?>().Should().Be(null);
	}
}