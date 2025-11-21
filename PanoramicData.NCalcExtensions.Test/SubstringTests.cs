namespace PanoramicData.NCalcExtensions.Test;

public class SubstringTests
{
	[Theory]
	[InlineData("substring('haystack', 3)", "stack")]
	[InlineData("substring('haystack', 0, 3)", "hay")]
	[InlineData("substring('haystack', 3, 100)", "stack")]
	[InlineData("substring('haystack', 0, 100)", "haystack")]
	[InlineData("substring('haystack', 0, 0)", "")]
	public void Substring_HelpExamples_Succeed(string expressionText, string expected)
	{
		var expression = new ExtendedExpression(expressionText);
		(expression.Evaluate() as string).Should().Be(expected);
	}

	[Fact]
	public void Substring_TwoParameters_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 1)");
		(expression.Evaluate() as string).Should().Be("BC");
	}

	[Fact]
	public void Substring_ThreeParameters_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 1, 1)");
		(expression.Evaluate() as string).Should().Be("B");
	}

	[Fact]
	public void Substring_ThreeParametersTruncate_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 0, 6)");
		(expression.Evaluate() as string).Should().Be("ABC");
	}

	[Fact]
	public void Substring_ThreeParametersTruncateMidString_Succeeds()
	{
		var expression = new ExtendedExpression("substring('ABC', 1, 6)");
		(expression.Evaluate() as string).Should().Be("BC");
	}

	// Additional comprehensive tests

	[Fact]
	public void Substring_EmptyString_ReturnsEmpty()
	{
		var expression = new ExtendedExpression("substring('', 0)");
		(expression.Evaluate() as string).Should().Be("");
	}

	[Fact]
	public void Substring_StartAtEnd_ReturnsEmpty()
	{
		var expression = new ExtendedExpression("substring('test', 4)");
		(expression.Evaluate() as string).Should().Be("");
	}

	[Fact]
	public void Substring_LengthZero_ReturnsEmpty()
	{
		var expression = new ExtendedExpression("substring('test', 1, 0)");
		(expression.Evaluate() as string).Should().Be("");
	}

	[Fact]
	public void Substring_SingleCharacter_ReturnsChar()
	{
		var expression = new ExtendedExpression("substring('A', 0, 1)");
		(expression.Evaluate() as string).Should().Be("A");
	}

	[Fact]
	public void Substring_LastCharacter_ReturnsChar()
	{
		var expression = new ExtendedExpression("substring('ABC', 2, 1)");
		(expression.Evaluate() as string).Should().Be("C");
	}

	[Fact]
	public void Substring_MiddleSection_ReturnsSection()
	{
		var expression = new ExtendedExpression("substring('ABCDEF', 2, 2)");
		(expression.Evaluate() as string).Should().Be("CD");
	}

	[Theory]
	[InlineData("substring()")]
	[InlineData("substring('test')")]
	public void Substring_WrongParameterCount_ThrowsException(string expressionText)
		=> new ExtendedExpression(expressionText)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	[Fact]
	public void Substring_ExtraParameters_IgnoresExtra()
	{
		// Substring accepts 2 or 3 parameters, extra ones are ignored
		var expression = new ExtendedExpression("substring('test', 0, 2, 999)");
		(expression.Evaluate() as string).Should().Be("te");
	}

	[Fact]
	public void Substring_NullString_ThrowsException()
		=> new ExtendedExpression("substring(null, 0)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	[Fact]
	public void Substring_NegativeStart_ThrowsException()
		=> new ExtendedExpression("substring('test', -1)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	[Fact]
	public void Substring_NegativeLength_ThrowsException()
		=> new ExtendedExpression("substring('test', 0, -1)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	[Fact]
	public void Substring_StartBeyondLength_ThrowsException()
		=> new ExtendedExpression("substring('test', 10)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	[Fact]
	public void Substring_NonIntegerStartIndex_ThrowsException()
		=> new ExtendedExpression("substring('test', 'abc')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*one or two numeric parameters*");

	[Fact]
	public void Substring_NonIntegerLength_ThrowsException()
		=> new ExtendedExpression("substring('test', 0, 'abc')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*one or two numeric parameters*");

	[Fact]
	public void Substring_WithSpaces_HandlesCorrectly()
	{
		var expression = new ExtendedExpression("substring('hello world', 6, 5)");
		(expression.Evaluate() as string).Should().Be("world");
	}

	[Fact]
	public void Substring_WholeString_ReturnsWholeString()
	{
		var expression = new ExtendedExpression("substring('complete', 0)");
		(expression.Evaluate() as string).Should().Be("complete");
	}
}
