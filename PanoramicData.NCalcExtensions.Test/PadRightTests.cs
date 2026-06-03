namespace PanoramicData.NCalcExtensions.Test;

public class PadRightTests
{
	[Fact]
	public void PadRight_EmptyString_SucceedsWithPaddedString()
	{
		var expression = new ExtendedExpression("padRight('', 1, '0')");
		(expression.Evaluate() as string).Should().Be("0");
	}

	[Fact]
	public void PadRight_DesiredLengthGreaterThanInput_SucceedsWithPaddedString()
	{
		var expression = new ExtendedExpression("padRight('12', 5, '0')");
		(expression.Evaluate() as string).Should().Be("12000");
	}

	[Fact]
	public void PadRight_DesiredLengthEqualToInput_SucceedsWithOriginalString()
	{
		var expression = new ExtendedExpression("padRight('12345', 5, '0')");
		(expression.Evaluate() as string).Should().Be("12345");
	}

	[Fact]
	public void PadRight_DesiredLengthLessThanInput_SucceedsWithOriginalString()
	{
		var expression = new ExtendedExpression("padRight('12345', 2, '0')");
		(expression.Evaluate() as string).Should().Be("12345");
	}

	[Fact]
	public void PadRight_DesiredStringLengthTooLow_FailsAsExpected()
		=> new ExtendedExpression("padRight('12345', 0, '0')")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<NCalcExtensionsException>()
			.WithMessage("padRight() requires a DesiredStringLength for parameter 2 that is >= 1.");

	[Fact]
	public void PadRight_PaddingStringTooLong_FailsAsExpected()
		=> new ExtendedExpression("padRight('12345', 5, '00')")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<NCalcExtensionsException>()
			.WithMessage("padRight() requires a single character string for parameter 3.");

	[Fact]
	public void PadRight_PaddingStringEmpty_FailsAsExpected()
		=> new ExtendedExpression("padRight('12345', 5, '')")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<NCalcExtensionsException>()
			.WithMessage("padRight() requires a single character string for parameter 3.");

	[Fact]
	public void PadRight_NullInput_ThrowsException()
		=> new ExtendedExpression("padRight(null, 5, '0')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*string Input*integer DesiredStringLength*");

	[Fact]
	public void PadRight_NonIntegerLength_ThrowsException()
		=> new ExtendedExpression("padRight('test', 'abc', '0')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*integer DesiredStringLength*");

	[Fact]
	public void PadRight_NullPaddingCharacter_ThrowsException()
		=> new ExtendedExpression("padRight('test', 5, null)")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<NCalcExtensionsException>()
			.WithMessage("*parameter 3 be a string*");
}
