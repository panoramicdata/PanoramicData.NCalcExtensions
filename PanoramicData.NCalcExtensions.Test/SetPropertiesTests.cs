using System.Collections.Generic;
namespace PanoramicData.NCalcExtensions.Test;

public class SetPropertiesTests : NCalcTest
{
	/// <summary>
	/// Asserts the result of setting 'c' to 'X' on an object that already had a = 1 and b = null.
	/// </summary>
	private static void ShouldHaveOriginalPropertiesAndC(object? evaluated)
	{
		var result = evaluated.Should().BeOfType<JObject>().Subject;

		result.Should().HaveCount(3);
		result.Properties().Select(property => property.Value).Should().AllBeOfType<JValue>();
		result["a"].Should().BeEquivalentTo(JToken.FromObject(1));
		result["b"].Should().BeEquivalentTo(JValue.CreateNull());
		result["c"].Should().BeEquivalentTo(JToken.FromObject("X"));
	}

	[Fact]
	public void SetProperties_OnJObject_CreatesJObject()
		=> ShouldHaveOriginalPropertiesAndC(Test("setProperties(jObject('a', 1, 'b', null), 'c', 'X')"));

	[Fact]
	public void SetProperties_OnAnonymous_CreatesJObject()
		=> ShouldHaveOriginalPropertiesAndC(Test("setProperties(anon, 'c', 'X')", "anon", new { a = 1, b = (string?)null }));

	[Fact]
	public void SetProperties_MultipleProperties_Succeeds()
	{
		var result = Test("setProperties(jObject('a', 1), 'b', 2, 'c', 3)").Should().BeOfType<JObject>().Subject;

		result.Properties().ToDictionary(property => property.Name, property => property.Value.Value<int>())
			.Should().Equal(new Dictionary<string, int> { ["a"] = 1, ["b"] = 2, ["c"] = 3 });
	}

	[Theory]
	[InlineData("setProperties(jObject('a', 1), 'b')", "*odd number of parameters*")]
	[InlineData("setProperties(null, 'key', 'value')", "*first parameter cannot be null*")]
	[InlineData("setProperties(jObject('a', 1), 123, 'value')", "*requires a string key*")]
	[InlineData("setProperties(jObject('a', 1), 'a', 2)", "*can only define property a once*")]
	public void SetProperties_InvalidParameters_ThrowsException(string expressionText, string messagePattern)
		=> TestShouldThrowExactly<FormatException>(expressionText, messagePattern);
}
