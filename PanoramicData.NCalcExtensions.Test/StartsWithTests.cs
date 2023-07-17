namespace PanoramicData.NCalcExtensions.Test;
public class StartsWithTests
{
	[Theory]
	[InlineData("abc","a",true)]
	[InlineData("abc","b",false)]
	[InlineData("abc","c",false)]
	[InlineData("abc","ab",true)]
	[InlineData("abc","bc",false)]
	
	public void StartsWith_Succeeds(string parameter1, string parameter2, bool expectedValue)
		=> new ExtendedExpression($"startsWith('{parameter1}', '{parameter2}')").Evaluate().Should().Be(expectedValue);
}
