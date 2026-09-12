namespace PanoramicData.NCalcExtensions.Test;

public class JArrayTests : NCalcTest
{
	[Fact]
	public void JArray_CreatesJArray()
	{
		var result = EvaluateToJArray("jArray(jObject('a', 1, 'b', null), jObject('a', 2, 'b', 'woo'), null)", 3);
		result[0]["a"].Should().BeOfType<JValue>();
		result[0]["a"].Should().BeEquivalentTo(JToken.FromObject(1));
		result[0]["b"].Should().BeOfType<JValue>();
		result[0]["b"].Should().BeEquivalentTo(JValue.CreateNull());
		result[1]["a"].Should().BeOfType<JValue>();
		result[1]["a"].Should().BeEquivalentTo(JToken.FromObject(2));
		result[1]["b"].Should().BeOfType<JValue>();
		result[1]["b"].Should().BeEquivalentTo(JToken.FromObject("woo"));
		result[2].Should().BeEquivalentTo(JValue.CreateNull());
	}

	[Fact]
	public void JArray_StringParameters_CreatesJArray()
	{
		var result = EvaluateToJArray("jArray('test1', 'test2')", 2);
		result[0].Should().BeOfType<JValue>();
		result[0].Should().BeEquivalentTo(JToken.FromObject("test1"));
		result[1].Should().BeOfType<JValue>();
		result[1].Should().BeEquivalentTo(JToken.FromObject("test2"));
	}

	/// <summary>
	/// Evaluates <paramref name="expressionText"/> to a JArray of <paramref name="expectedCount"/>
	/// entries.
	/// </summary>
	private static JArray EvaluateToJArray(string expressionText, int expectedCount)
	{
		var result = Test(expressionText) as JArray;
		result.Should().BeOfType<JArray>();
		result.Should().NotBeNull();
		result.Should().HaveCount(expectedCount);
		return result!;
	}
}
