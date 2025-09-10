namespace PanoramicData.NCalcExtensions.Test;

public class NullOperatorTests : NCalcTest
{
	[Theory]
	[InlineData("null")]
	[InlineData("null + 1")]
	[InlineData("null - 1")]
	[InlineData("null * 1")]
	[InlineData("null / 1")]
	[InlineData("null % 1")]
	public void NullOutputTest_Succeeds(string expressionThatShouldResultInNull)
		=> new ExtendedExpression(expressionThatShouldResultInNull).Evaluate().Should().BeNull();
}