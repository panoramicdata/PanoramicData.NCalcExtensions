namespace PanoramicData.NCalcExtensions.Test;

public class PadLeftTests
{
	[Fact]
	public void PadLeft_EmptyString_SucceedsWithPaddedString()
	{
		var expression = new ExtendedExpression("padLeft('', 1, '0')");
		(expression.Evaluate() as string).Should().Be("0");
	}

	[Fact]
	public void PadLeft_DesiredLengthGreaterThanInput_SucceedsWithPaddedString()
	{
		var expression = new ExtendedExpression("padLeft('12', 5, '0')");
		(expression.Evaluate() as string).Should().Be("00012");
	}

	[Fact]
	public void PadLeft_DesiredLengthEqualToInput_SucceedsWithOriginalString()
	{
		var expression = new ExtendedExpression("padLeft('12345', 5, '0')");
		(expression.Evaluate() as string).Should().Be("12345");
	}

	[Fact]
	public void PadLeft_DesiredLengthLessThanInput_SucceedsWithOriginalString()
	{
		var expression = new ExtendedExpression("padLeft('12345', 2, '0')");
		(expression.Evaluate() as string).Should().Be("12345");
	}

	[Fact]
	public void PadLeft_DesiredStringLengthTooLow_FailsAsExpected()
		=> new ExtendedExpression("padLeft('12345', 0, '0')")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<NCalcExtensionsException>()
			.WithMessage("padLeft() requires a DesiredStringLength for parameter 2 that is >= 1.");

	[Fact]
	public void PadLeft_PaddingStringTooLong_FailsAsExpected()
		=> new ExtendedExpression("padLeft('12345', 5, '00')")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<NCalcExtensionsException>()
			.WithMessage("padLeft() requires a single character string for parameter 3.");

	[Fact]
	public void PadLeft_PaddingStringEmpty_FailsAsExpected()
		=> new ExtendedExpression("padLeft('12345', 5, '')")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<NCalcExtensionsException>()
			.WithMessage("padLeft() requires a single character string for parameter 3.");

	[Fact]
	public void PadLeft_NullInput_ThrowsException()
		=> new ExtendedExpression("padLeft(null, 5, '0')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*string Input*integer DesiredStringLength*");

	[Fact]
	public void PadLeft_NonIntegerLength_ThrowsException()
		=> new ExtendedExpression("padLeft('test', 'abc', '0')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*integer DesiredStringLength*");

	[Fact]
	public void PadLeft_NullPaddingCharacter_ThrowsException()
		=> new ExtendedExpression("padLeft('test', 5, null)")
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<NCalcExtensionsException>()
			.WithMessage("*parameter 3 be a string*");
}
