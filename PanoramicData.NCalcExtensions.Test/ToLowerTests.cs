namespace PanoramicData.NCalcExtensions.Test;
public class ToLowerTests
{
	[Theory]
	[InlineData("ABC", "abc")]
	[InlineData("abc", "abc")]
	[InlineData("AbC", "abc")]
	[InlineData("123ABC", "123abc")]
	public void ToLower_UsingInlineData_ResultMatchExpectedValue(string parameter, string expectedValue)
	{
		var expression = new ExtendedExpression($"toLower('{parameter}')");
		var result = expression.Evaluate();
		result.Should().Be(expectedValue);
	}

	[Fact]
	public void ToLower_NullParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("toLower(null)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*requires one string parameter*");
	}

	[Fact]
	public void ToLower_NonStringParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("toLower(123)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*requires one string parameter*");
	}
}
