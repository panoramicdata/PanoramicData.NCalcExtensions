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

	[Fact]
	public void Contains_NullHaystack_ThrowsException()
	{
		var expression = new ExtendedExpression("contains(null, 'needle')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}

	[Fact]
	public void Contains_NullNeedle_ThrowsException()
	{
		var expression = new ExtendedExpression("contains('haystack', null)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}

	[Fact]
	public void Contains_NonStringParameters_ThrowsException()
	{
		var expression = new ExtendedExpression("contains(123, 456)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}
}