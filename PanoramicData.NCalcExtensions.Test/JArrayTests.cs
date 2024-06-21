namespace PanoramicData.NCalcExtensions.Test;

public class JArrayTests
{
	[Fact]
	public void JArray_CreatesJArray()
	{
		var expression = new ExtendedExpression("jArray(jObject('a', 1, 'b', null), jObject('a', 2, 'b', 'woo'), null)");
		var result = expression.Evaluate() as JArray;
		result.Should().BeOfType<JArray>();
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result![0]["a"].Should().BeOfType<JValue>();
		result![0]["a"].Should().BeEquivalentTo(JToken.FromObject(1));
		result![0]["b"].Should().BeOfType<JValue>();
		result![0]["b"].Should().BeEquivalentTo(JValue.CreateNull());
		result![1]["a"].Should().BeOfType<JValue>();
		result![1]["a"].Should().BeEquivalentTo(JToken.FromObject(2));
		result![1]["b"].Should().BeOfType<JValue>();
		result![1]["b"].Should().BeEquivalentTo(JToken.FromObject("woo"));
		result![2].Should().BeEquivalentTo(JValue.CreateNull());
	}
}
