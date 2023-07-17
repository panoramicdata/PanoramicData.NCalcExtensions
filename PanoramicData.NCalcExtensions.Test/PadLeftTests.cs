namespace PanoramicData.NCalcExtensions.Test;

public class PadLeftTests
{
	[Fact]
	public void PadLeft_EmptyString_SucceedsWithPaddedString()
	{
		var expression = new ExtendedExpression("padLeft('', 1, '0')");
		Assert.Equal("0", expression.Evaluate() as string);
	}

	[Fact]
	public void PadLeft_DesiredLengthGreaterThanInput_SucceedsWithPaddedString()
	{
		var expression = new ExtendedExpression("padLeft('12', 5, '0')");
		Assert.Equal("00012", expression.Evaluate() as string);
	}

	[Fact]
	public void PadLeft_DesiredLengthEqualToInput_SucceedsWithOriginalString()
	{
		var expression = new ExtendedExpression("padLeft('12345', 5, '0')");
		Assert.Equal("12345", expression.Evaluate() as string);
	}

	[Fact]
	public void PadLeft_DesiredLengthLessThanInput_SucceedsWithOriginalString()
	{
		var expression = new ExtendedExpression("padLeft('12345', 2, '0')");
		Assert.Equal("12345", expression.Evaluate() as string);
	}

	[Fact]
	public void PadLeft_DesiredStringLengthTooLow_FailsAsExpected()
	{
		var expression = new ExtendedExpression("padLeft('12345', 0, '0')");
		var exception = Assert.Throws<NCalcExtensionsException>(expression.Evaluate);
		exception.Message.Should().Be("padLeft() requires a DesiredStringLength for parameter 2 that is >= 1.");
	}

	[Fact]
	public void PadLeft_PaddingStringTooLong_FailsAsExpected()
	{
		var expression = new ExtendedExpression("padLeft('12345', 5, '00')");
		var exception = Assert.Throws<NCalcExtensionsException>(expression.Evaluate);
		exception.Message.Should().Be("padLeft() requires a single character string for parameter 3.");
	}

	[Fact]
	public void PadLeft_PaddingStringEmpty_FailsAsExpected()
	{
		var expression = new ExtendedExpression("padLeft('12345', 5, '')");
		var exception = Assert.Throws<NCalcExtensionsException>(expression.Evaluate);
		exception.Message.Should().Be("padLeft() requires a single character string for parameter 3.");
	}
}
