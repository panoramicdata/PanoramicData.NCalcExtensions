namespace PanoramicData.NCalcExtensions.Test;

public class JArrayTests : NCalcTest
{
	[Fact]
	public void JArray_CreatesJArray()
	{
		var result = EvaluateToJArray("jArray(jObject('a', 1, 'b', null), jObject('a', 2, 'b', 'woo'), null)", 3);

		var fields = result.Take(2).SelectMany(item => new[] { item["a"], item["b"] }).ToList();
		fields.Should().AllBeOfType<JValue>();
		fields.Should().Equal(
			[JToken.FromObject(1), JValue.CreateNull(), JToken.FromObject(2), JToken.FromObject("woo")],
			JToken.DeepEquals);
		result[2].Should().BeEquivalentTo(JValue.CreateNull());
	}

	[Fact]
	public void JArray_StringParameters_CreatesJArray()
	{
		var result = EvaluateToJArray("jArray('test1', 'test2')", 2);

		result.Should().AllBeOfType<JValue>();
		result.Should().Equal([JToken.FromObject("test1"), JToken.FromObject("test2")], JToken.DeepEquals);
	}

	/// <summary>
	/// Evaluates <paramref name="expressionText"/> to a JArray of <paramref name="expectedCount"/>
	/// entries.
	/// </summary>
	private static JArray EvaluateToJArray(string expressionText, int expectedCount)
	{
		var result = Test(expressionText).Should().BeOfType<JArray>().Subject;
		result.Should().HaveCount(expectedCount);
		return result;
	}
}
