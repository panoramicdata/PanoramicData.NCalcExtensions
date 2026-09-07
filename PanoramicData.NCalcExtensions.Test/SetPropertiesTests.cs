namespace PanoramicData.NCalcExtensions.Test;

public class SetPropertiesTests : NCalcTest
{
	/// <summary>
	/// Evaluates <paramref name="expression"/>, requiring it to produce a JObject.
	/// </summary>
	private static JObject EvaluateToJObject(ExtendedExpression expression)
	{
		var result = expression.Evaluate();
		result.Should().BeOfType<JObject>();
		return (JObject)result!;
	}

	/// <summary>
	/// Asserts the result of setting 'c' to 'X' on an object that already had a = 1 and b = null.
	/// </summary>
	private static void ShouldHaveOriginalPropertiesAndC(JObject result)
	{
		result.Should().HaveCount(3);
		result["a"].Should().BeOfType<JValue>();
		result["a"].Should().BeEquivalentTo(JToken.FromObject(1));
		result["b"].Should().BeOfType<JValue>();
		result["b"].Should().BeEquivalentTo(JValue.CreateNull());
		result["c"].Should().BeEquivalentTo(JToken.FromObject("X"));
	}

	/// <summary>
	/// Asserts that <paramref name="expressionText"/> fails with a FormatException matching
	/// <paramref name="messagePattern"/>.
	/// </summary>
	private static void ShouldFailWith(string expressionText, string messagePattern)
		=> new ExtendedExpression(expressionText)
			.Invoking(expression => expression.Evaluate())
			.Should().ThrowExactly<FormatException>()
			.WithMessage(messagePattern);

	[Fact]
	public void SetProperties_OnJObject_CreatesJObject()
		=> ShouldHaveOriginalPropertiesAndC(
			EvaluateToJObject(new ExtendedExpression("setProperties(jObject('a', 1, 'b', null), 'c', 'X')")));

	[Fact]
	public void SetProperties_OnAnonymous_CreatesJObject()
	{
		var expression = new ExtendedExpression("setProperties(anon, 'c', 'X')");
		expression.Parameters["anon"] = new { a = 1, b = (string?)null };

		ShouldHaveOriginalPropertiesAndC(EvaluateToJObject(expression));
	}

	[Fact]
	public void SetProperties_MultipleProperties_Succeeds()
	{
		var result = EvaluateToJObject(new ExtendedExpression("setProperties(jObject('a', 1), 'b', 2, 'c', 3)"));

		result.Should().HaveCount(3);
		result["a"]!.Value<int>().Should().Be(1);
		result["b"]!.Value<int>().Should().Be(2);
		result["c"]!.Value<int>().Should().Be(3);
	}

	// Error cases
	[Fact]
	public void SetProperties_EvenNumberOfParameters_ThrowsException()
		=> ShouldFailWith("setProperties(jObject('a', 1), 'b')", "*odd number of parameters*");

	[Fact]
	public void SetProperties_NullFirstParameter_ThrowsException()
		=> ShouldFailWith("setProperties(null, 'key', 'value')", "*first parameter cannot be null*");

	[Fact]
	public void SetProperties_NonStringKey_ThrowsException()
		=> ShouldFailWith("setProperties(jObject('a', 1), 123, 'value')", "*requires a string key*");

	[Fact]
	public void SetProperties_DuplicateKey_ThrowsException()
		=> ShouldFailWith("setProperties(jObject('a', 1), 'a', 2)", "*can only define property a once*");
}
