namespace PanoramicData.NCalcExtensions.Test;

public class CapitalizeTests : NCalcTest
{
	[Theory]
	[InlineData("hello", "Hello")]
	[InlineData("test", "Test")]
	[InlineData("123", "123")]
	[InlineData("123test", "123test")]
	[InlineData("test123", "Test123")]
	[InlineData("Test", "Test")]


	public void Capitalize_Succeeds(string input, string expected)
		=> new ExtendedExpression($"capitalize('{input}')")
		.Evaluate()
		.Should()
		.Be(expected);
}


