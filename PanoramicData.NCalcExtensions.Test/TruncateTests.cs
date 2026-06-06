namespace PanoramicData.NCalcExtensions.Test;

public class TruncateTests
{
	[Fact]
	public void Truncate_StringShorterThanMax_ReturnsOriginal()
	{
		var expression = new ExtendedExpression("truncate('hello', 10)");
		(expression.Evaluate() as string).Should().Be("hello");
	}

	[Fact]
	public void Truncate_StringExactlyMaxLength_ReturnsOriginal()
	{
		var expression = new ExtendedExpression("truncate('hello', 5)");
		(expression.Evaluate() as string).Should().Be("hello");
	}

	[Fact]
	public void Truncate_StringLongerThanMax_ReturnsTruncated()
	{
		var expression = new ExtendedExpression("truncate('hello world', 5)");
		(expression.Evaluate() as string).Should().Be("hello");
	}

	[Fact]
	public void Truncate_WithEllipsis_ReturnsTruncatedWithEllipsis()
	{
		var expression = new ExtendedExpression("truncate('hello world', 8, '...')");
		(expression.Evaluate() as string).Should().Be("hello...");
	}

	[Fact]
	public void Truncate_EllipsisLongerThanMax_ClipsWithoutEllipsis()
	{
		var expression = new ExtendedExpression("truncate('hello world', 2, '...')");
		(expression.Evaluate() as string).Should().Be("he");
	}

	[Fact]
	public void Truncate_MaxLengthZero_ReturnsEmpty()
	{
		var expression = new ExtendedExpression("truncate('hello', 0)");
		(expression.Evaluate() as string).Should().Be(string.Empty);
	}

	[Fact]
	public void Truncate_EmptyString_ReturnsEmpty()
	{
		var expression = new ExtendedExpression("truncate('', 5)");
		(expression.Evaluate() as string).Should().Be(string.Empty);
	}

	[Fact]
	public void Truncate_NegativeMaxLength_FailsAsExpected()
		=> new ExtendedExpression("truncate('hello', -1)")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<NCalcExtensionsException>()
			.WithMessage("truncate() requires a maxLength >= 0.");

	[Fact]
	public void Truncate_NullString_ThrowsException()
		=> new ExtendedExpression("truncate(null, 5)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*string parameter 1*integer parameter 2*");

	[Fact]
	public void Truncate_NonIntegerMaxLength_ThrowsException()
		=> new ExtendedExpression("truncate('hello', 'abc')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*string parameter 1*integer parameter 2*");
}
