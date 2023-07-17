namespace PanoramicData.NCalcExtensions.Test;
public class ToLowerTests
{
	[Theory]
	[InlineData("ABC", "abc")]
	[InlineData("abc", "abc")]
	[InlineData("AbC", "abc")]
	[InlineData("123ABC", "123abc")]
	public void ToLower_Succeeds(string parameter, string expectedValue)
		=> new ExtendedExpression($"toLower('{parameter}')").Evaluate().Should().Be(expectedValue);
}
