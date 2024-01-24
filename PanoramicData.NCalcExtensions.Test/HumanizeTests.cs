namespace PanoramicData.NCalcExtensions.Test;
public class HumanizeTests
{
	[Theory]
	[InlineData("3600000", "Milliseconds", "1 hour")]
	[InlineData("3600", "Seconds", "1 hour")]
	[InlineData("60", "Minutes", "1 hour")]
	[InlineData("1", "Hours", "1 hour")]
	[InlineData("1", "Days", "1 day")]
	[InlineData("1", "Weeks", "7 days")]
	public void Humanize_UsingInlineData_MatchesExpectedValue(string expressionText, string datatype, string expected)
	{
		var expression = new ExtendedExpression($"humanize({expressionText},'{datatype}')");
		var result = expression.Evaluate();
		result.Should().Be(expected);
	}
}
