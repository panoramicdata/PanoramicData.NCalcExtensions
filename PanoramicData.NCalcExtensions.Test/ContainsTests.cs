namespace PanoramicData.NCalcExtensions.Test;

public class ContainsTests
{
	[Theory]
	[InlineData("'ABC'", "'A'", true)]
	[InlineData("'ABC'", "'B'", true)]
	[InlineData("'ABC'", "'//'", false)]
	[InlineData("'//TRN'", "'//'", true)]
	[InlineData("'ABC //TRN'", "'//'", true)]
	public void Contains_Succeeds(string haystack, string needle, bool expectedResult)
	{
		var expression = new ExtendedExpression($"contains({haystack}, {needle})");
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();
		result.Should().Be(expectedResult);
	}
}