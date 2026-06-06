namespace PanoramicData.NCalcExtensions.Test;

public class RepeatTests
{
	[Fact]
	public void Repeat_BasicString_SucceedsWithRepeatedString()
	{
		var expression = new ExtendedExpression("repeat('ab', 3)");
		(expression.Evaluate() as string).Should().Be("ababab");
	}

	[Fact]
	public void Repeat_CountOfOne_ReturnsSameString()
	{
		var expression = new ExtendedExpression("repeat('hello', 1)");
		(expression.Evaluate() as string).Should().Be("hello");
	}

	[Fact]
	public void Repeat_CountOfZero_ReturnsEmptyString()
	{
		var expression = new ExtendedExpression("repeat('hello', 0)");
		(expression.Evaluate() as string).Should().Be(string.Empty);
	}

	[Fact]
	public void Repeat_EmptyString_ReturnsEmptyString()
	{
		var expression = new ExtendedExpression("repeat('', 5)");
		(expression.Evaluate() as string).Should().Be(string.Empty);
	}

	[Fact]
	public void Repeat_NegativeCount_FailsAsExpected()
		=> new ExtendedExpression("repeat('hi', -1)")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<NCalcExtensionsException>()
			.WithMessage("repeat() requires a count >= 0.");

	[Fact]
	public void Repeat_NullString_ThrowsException()
		=> new ExtendedExpression("repeat(null, 3)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*string parameter 1*integer parameter 2*");

	[Fact]
	public void Repeat_NonIntegerCount_ThrowsException()
		=> new ExtendedExpression("repeat('hi', 'abc')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*string parameter 1*integer parameter 2*");
}
