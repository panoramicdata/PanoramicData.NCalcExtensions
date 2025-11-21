namespace PanoramicData.NCalcExtensions.Test;

public class StartsWithTests
{
	[Theory]
	[InlineData("abc", "a", true)]
	[InlineData("abc", "b", false)]
	[InlineData("abc", "ab", true)]
	[InlineData("abc", "bc", false)]
	[InlineData("test", "", true)]
	[InlineData("", "", true)]
	[InlineData("", "a", false)]
	[InlineData("hello", "hello", true)]
	[InlineData("hi", "hello", false)]
	[InlineData("Hello", "hello", false)] // Case sensitive
	[InlineData("  hello", "  ", true)]
	[InlineData("x", "x", true)]
	[InlineData("世界Hello", "世界", true)]
	public void StartsWith_VariousInputs_ReturnsExpected(string text, string prefix, bool expected)
	{
		var expression = new ExtendedExpression($"startsWith('{text}','{prefix}')");
		expression.Evaluate().Should().Be(expected);
	}

	[Theory]
	[InlineData("startsWith()")]
	[InlineData("startsWith('test')")]
	[InlineData("startsWith('a', 'b', 'c')")]
	[InlineData("startsWith(null, 'test')")]
	[InlineData("startsWith('test', null)")]
	public void StartsWith_InvalidInput_ThrowsException(string expressionText)
		=> new ExtendedExpression(expressionText)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	[Fact]
	public void StartsWith_WithVariable_ReturnsTrue()
	{
		var expression = new ExtendedExpression("startsWith(text, 'Hello')");
		expression.Parameters["text"] = "Hello World";
		expression.Evaluate().Should().Be(true);
	}
}
