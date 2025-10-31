namespace PanoramicData.NCalcExtensions.Test;

public class StartsWithTests
{
	[Theory]
	[InlineData("abc", "a", true)]
	[InlineData("abc", "b", false)]
	[InlineData("abc", "ab", true)]
	[InlineData("abc", "bc", false)]
	public void StartsWith_UsingInlineData_ResultMatchExpectedValue(string parameter1, string parameter2, bool expectedValue)
	{
		var expression = new ExtendedExpression($"startsWith('{parameter1}','{parameter2}')");
		var result = expression.Evaluate();
		result.Should().Be(expectedValue);
	}

	// Additional comprehensive tests

	[Fact]
	public void StartsWith_EmptyStringPrefix_ReturnsTrue()
	{
		var expression = new ExtendedExpression("startsWith('test', '')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void StartsWith_EmptyString_EmptyPrefix_ReturnsTrue()
	{
		var expression = new ExtendedExpression("startsWith('', '')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void StartsWith_EmptyString_NonEmptyPrefix_ReturnsFalse()
	{
		var expression = new ExtendedExpression("startsWith('', 'a')");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void StartsWith_ExactMatch_ReturnsTrue()
	{
		var expression = new ExtendedExpression("startsWith('hello', 'hello')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void StartsWith_LongerPrefix_ReturnsFalse()
	{
		var expression = new ExtendedExpression("startsWith('hi', 'hello')");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void StartsWith_CaseSensitive_ReturnsFalse()
	{
		var expression = new ExtendedExpression("startsWith('Hello', 'hello')");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void StartsWith_WithSpaces_HandlesCorrectly()
	{
		var expression = new ExtendedExpression("startsWith('  hello', '  ')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void StartsWith_SingleCharacter_ReturnsTrue()
	{
		var expression = new ExtendedExpression("startsWith('x', 'x')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void StartsWith_UnicodeCharacters_HandlesCorrectly()
	{
		var expression = new ExtendedExpression("startsWith('世界Hello', '世界')");
		expression.Evaluate().Should().Be(true);
	}

	[Theory]
	[InlineData("startsWith()")]
	[InlineData("startsWith('test')")]
	[InlineData("startsWith('a', 'b', 'c')")]
	public void StartsWith_WrongParameterCount_ThrowsException(string expressionText)
		=> new ExtendedExpression(expressionText)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	[Fact]
	public void StartsWith_NullFirstParameter_ThrowsException()
		=> new ExtendedExpression("startsWith(null, 'test')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	[Fact]
	public void StartsWith_NullSecondParameter_ThrowsException()
		=> new ExtendedExpression("startsWith('test', null)")
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
