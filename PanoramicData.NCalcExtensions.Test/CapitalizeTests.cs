namespace PanoramicData.NCalcExtensions.Test;

public class CapitalizeTests : NCalcTest
{
	[Theory]
	[InlineData("hello", "Hello")]
	[InlineData("Test", "Test")]
	public void Capitalize_UsingInlinedata_MatchesExpectedResult(string input, string expected)
	{
		var expression = new ExtendedExpression($"capitalize('{input}')");

		var result = expression.Evaluate();

		Assert.Equal(expected, result);
	}
}


