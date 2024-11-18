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
		result["a"]!.Value<int>().Should().Be(1);
		result["b"]!.Value<int?>().Should().BeNull();
		result["c"]!.Value<int>().Should().Be(5);
	}

	[Fact]
	public void Extend_WithNull_Succeeds()
	{
		var expression = new ExtendedExpression("extend(jObject('a', 1, 'b', null), list('c', null))");
		var result = expression.Evaluate() as JObject;

		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result["a"]!.Value<int>().Should().Be(1);
		result["b"]!.Value<int?>().Should().BeNull();
		result["c"]!.Value<string?>().Should().Be(null);
	}
}