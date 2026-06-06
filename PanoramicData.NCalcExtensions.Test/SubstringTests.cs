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

	// AC-01: Default mode (no mode param) gives bounds-specific error for negative start
	[Fact]
	public void Substring_NegativeStart_DefaultMode_ThrowsBoundsError()
		=> new ExtendedExpression("substring('test', -1)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*start index*out of bounds*");

	// AC-01: Explicit 'Error' mode gives same bounds-specific error
	[Fact]
	public void Substring_NegativeStart_ErrorMode_ThrowsBoundsError()
		=> new ExtendedExpression("substring('test', -1, 2, 'Error')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*start index*out of bounds*");

	[Fact]
	public void Substring_NegativeLength_ThrowsException()
		=> new ExtendedExpression("substring('test', 0, -1)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	// AC-01: Default mode gives bounds-specific error for start beyond length
	[Fact]
	public void Substring_StartBeyondLength_DefaultMode_ThrowsBoundsError()
		=> new ExtendedExpression("substring('test', 10)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*start index*out of bounds*");

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

	// AC-03: Scratchpad regression cases
	[Theory]
	[InlineData(1, 3, "bcd")]
	[InlineData(2, 3, "cde")]
	[InlineData(3, 3, "de")]
	[InlineData(4, 3, "e")]
	[InlineData(5, 3, "")]
	public void Substring_ScatchpadInBoundsCases_Succeed(int start, int length, string expected)
	{
		var expression = new ExtendedExpression($"substring('abcde', {start}, {length})");
		(expression.Evaluate() as string).Should().Be(expected);
	}

	// AC-04: Positive out-of-range - each mode
	[Fact]
	public void Substring_StartBeyondLength_EmptyMode_ReturnsEmpty()
		=> (new ExtendedExpression("substring('abcde', 6, 3, 'Empty')").Evaluate() as string).Should().Be(string.Empty);

	[Fact]
	public void Substring_StartBeyondLength_NullMode_ReturnsNull()
		=> new ExtendedExpression("substring('abcde', 6, 3, 'Null')").Evaluate().Should().BeNull();

	[Fact]
	public void Substring_StartBeyondLength_ClipMode_ReturnsEmpty()
		=> (new ExtendedExpression("substring('abcde', 6, 3, 'Clip')").Evaluate() as string).Should().Be(string.Empty);

	// AC-05: Negative start - each mode
	[Theory]
	[InlineData(-1)]
	[InlineData(-2)]
	[InlineData(-100)]
	public void Substring_NegativeStart_EmptyMode_ReturnsEmpty(int start)
		=> (new ExtendedExpression($"substring('abcde', {start}, 3, 'Empty')").Evaluate() as string).Should().Be(string.Empty);

	[Theory]
	[InlineData(-1)]
	[InlineData(-2)]
	[InlineData(-100)]
	public void Substring_NegativeStart_NullMode_ReturnsNull(int start)
		=> new ExtendedExpression($"substring('abcde', {start}, 3, 'Null')").Evaluate().Should().BeNull();

	[Theory]
	[InlineData(-1)]
	[InlineData(-2)]
	[InlineData(-100)]
	public void Substring_NegativeStart_ClipMode_ClampsToZero(int start)
		=> (new ExtendedExpression($"substring('abcde', {start}, 3, 'Clip')").Evaluate() as string).Should().Be("abc");

	// AC-05: Very large negative start under Clip
	[Fact]
	public void Substring_VeryLargeNegativeStart_ClipMode_ClampsToZero()
		=> (new ExtendedExpression("substring('abcde', -99999, 3, 'Clip')").Evaluate() as string).Should().Be("abc");

	// AC-04: Very large positive start under each mode
	[Fact]
	public void Substring_VeryLargePositiveStart_EmptyMode_ReturnsEmpty()
		=> (new ExtendedExpression("substring('abcde', 99999, 3, 'Empty')").Evaluate() as string).Should().Be(string.Empty);

	[Fact]
	public void Substring_VeryLargePositiveStart_ClipMode_ReturnsEmpty()
		=> (new ExtendedExpression("substring('abcde', 99999, 3, 'Clip')").Evaluate() as string).Should().Be(string.Empty);

	// AC-02: Mode is case-insensitive
	[Theory]
	[InlineData("'empty'")]
	[InlineData("'EMPTY'")]
	[InlineData("'Empty'")]
	public void Substring_ModeIsCaseInsensitive(string modeParam)
		=> (new ExtendedExpression($"substring('abcde', 6, 3, {modeParam})").Evaluate() as string).Should().Be(string.Empty);

	// AC-08: Empty input string - each mode
	[Fact]
	public void Substring_EmptyInput_StartZero_ReturnsEmpty()
		=> (new ExtendedExpression("substring('', 0)").Evaluate() as string).Should().Be(string.Empty);

	[Fact]
	public void Substring_EmptyInput_StartBeyond_DefaultMode_ThrowsBoundsError()
		=> new ExtendedExpression("substring('', 1)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*start index*out of bounds*");

	[Fact]
	public void Substring_EmptyInput_StartBeyond_ClipMode_ReturnsEmpty()
		=> (new ExtendedExpression("substring('', 1, 3, 'Clip')").Evaluate() as string).Should().Be(string.Empty);

	// AC-08: Omitted length - with Clip mode, use a large length to get full remaining string
	[Fact]
	public void Substring_ClipMode_NegativeStart_FullLengthRequested_ReturnsFromStart()
		=> (new ExtendedExpression("substring('abcde', -3, 100, 'Clip')").Evaluate() as string).Should().Be("abcde");

	// Unrecognised mode falls through to Error behavior
	[Fact]
	public void Substring_UnrecognisedMode_OutOfBounds_ThrowsBoundsError()
		=> new ExtendedExpression("substring('abcde', 6, 3, 'unknown')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*start index*out of bounds*");
}
