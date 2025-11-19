namespace PanoramicData.NCalcExtensions.Test;

public class CapitalizeTests : NCalcTest
{
	[Theory]
	[InlineData("hello", "Hello")]
	[InlineData("Test", "Test")]
	[InlineData("new year", "New year")]
	[InlineData("", "")]
	[InlineData("a", "A")]
	public void Capitalize_UsingInlineData_MatchesExpectedResult(string input, string expected)
	{
		var expression = new ExtendedExpression($"capitalize('{input}')");

		var result = expression.Evaluate();

		result.Should().Be(expected);
	}

	[Fact]
	public void Capitalize_NullParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("capitalize(null)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}

	[Fact]
	public void Capitalize_NonStringParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("capitalize(123)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}
}


