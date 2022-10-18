namespace PanoramicData.NCalcExtensions.Test;

public class SwitchTests : NCalcTest
{
	[Theory]
	[InlineData("switch()")]
	[InlineData("switch(1)")]
	[InlineData("switch(1, 2)")]
	public void Switch_InsufficientParameters_ThrowsException(string expression)
		=> Assert.Throws<FormatException>(() =>
		{
			var e = new ExtendedExpression(expression);
			e.Evaluate();
		});

	[Theory]
	[InlineData("switch('yes', 'yes', 1)", 1)]
	[InlineData("switch('yes', 'yes', 1, 'no', 2)", 1)]
	[InlineData("switch('yes', 'yes', '1', 'no', '2')", "1")]
	[InlineData("switch('no', 'yes', 1, 'no', 2)", 2)]
	[InlineData("switch('no', 'yes', '1', 'no', '2')", "2")]
	[InlineData("switch('blah', 'yes', 1, 'no', 2, 3)", 3)]
	[InlineData("switch('blah', 'yes', 1, 'no', 2, '3')", "3")]
	[InlineData("switch(1, 1, 'one', 2, 'two')", "one")]
	public void Switch_ReturnsExpected(string expression, object expectedOutput)
		=> Assert.Equal(expectedOutput, new ExtendedExpression(expression).Evaluate());

	[Theory]
	[InlineData("switch('blah', 'yes', 1, 'no', 2)")]
	public void Switch_MissingDefault_ThrowsException(string expression)
		=> Assert.Throws<FormatException>(() =>
		{
			var e = new ExtendedExpression(expression);
			e.Evaluate();
		});

	[Fact]
	public void Switch_ComparingIntegers_Works()
	{
		const string expression = "switch(incident_Priority, 4, 4, 1, 1, 21)";
		var e = new ExtendedExpression(expression);
		e.Parameters["incident_Priority"] = 1;
		var result = e.Evaluate();
		result.Should().Be(1);
	}

	[Fact]
	public void Switch_ComparingIntegersInsideIf_Works()
	{
		const string expression = "if(incident_exists, switch(incident_Priority, 4, 4, 1, 1, 21), 9)";
		var e = new ExtendedExpression(expression);
		e.Parameters["incident_exists"] = true;
		e.Parameters["incident_Priority"] = 4;
		var result = e.Evaluate();
		result.Should().Be(4);
	}

	[Fact]
	public void Switch_ComparingIntegersViaJObject_Works()
	{
		const string expression = "if(incident_exists, switch(incident_Priority, 4, 4, 1, 1, 21), 9)";
		var e = new ExtendedExpression(expression);
		JObject jobject = new JObject();
		jobject["incident_exists"] = true;
		jobject["incident_Priority"] = 4;
		foreach (var property in jobject.Properties())
		{
			e.Parameters[property.Name] = GetValue(property);
		}
		var result = e.Evaluate();
		result.Should().Be(4);
	}

	private static object? GetValue(JProperty jProperty) => jProperty.Value.Type switch
	{
		JTokenType.Null => (object?)null,
		JTokenType.Undefined => (object?)null,
		JTokenType.String => jProperty.Value.ToObject<string>(),
		JTokenType.Integer => jProperty.Value.ToObject<int>(),
		JTokenType.Float => jProperty.Value.ToObject<double>(),
		JTokenType.Boolean => jProperty.Value.ToObject<bool>(),
		_ => jProperty.Value
	};

}

