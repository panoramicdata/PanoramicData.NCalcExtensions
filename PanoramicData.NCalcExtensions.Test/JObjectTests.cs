namespace PanoramicData.NCalcExtensions.Test;

public class JObjectTests : NCalcTest
{
	[Fact]
	public void JObject_CreatesJObject()
	{
		var result = Test("jObject('a', 1, 'b', null)") as JObject;
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
		var result = Test("jObject()") as JObject;
		result.Should().BeOfType<JObject>();
		result.Should().NotBeNull();
		result.Should().HaveCount(0);
	}

	[Theory]
	[InlineData("jObject('a', 1, 'b')", "*even number of parameters*")]
	[InlineData("jObject(123, 'value')", "*requires a string key*")]
	[InlineData("jObject('a', 1, 'a', 2)", "*can only define property a once*")]
	public void JObject_WithInvalidParameters_ThrowsFormatException(string expressionText, string messagePattern)
		=> TestShouldThrowExactly<FormatException>(expressionText, messagePattern);
}
