namespace PanoramicData.NCalcExtensions.Test;

public class Sha256Tests
{
	[Theory]
	[InlineData("Hello", "185f8db32271fe25f561a6fc938b2e264306ec304eda518007d1764826381969")]
	[InlineData("World", "78ae647dc5544d227130a0682a51e30bc7777fbb6d8a8f17007463a3ecd1d524")]
	[InlineData("", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")]
	public void Sha256_String_ReturnsNull(string input, string expectedOutput)
	{
		var expression = new ExtendedExpression("sha256(x)");
		expression.Parameters.Add("x", input);
		expression.Evaluate().Should().Be(expectedOutput);
	}

	[Fact]
	public void Sha256_NullParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("sha256(null)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*string parameter*");
	}

	[Fact]
	public void Sha256_NonStringParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("sha256(123)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*string parameter*");
	}
}
