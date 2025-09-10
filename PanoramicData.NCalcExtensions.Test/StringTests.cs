namespace PanoramicData.NCalcExtensions.Test;

public class StringTests
{
	private const string x = "1";

	[Theory]
	[InlineData("'1' + '1'", "11")]
	[InlineData("x + x", "11")]
	[InlineData("'' + '1' + '1'", "11")]
	[InlineData("'' + x + x", "11")]
	[InlineData("x + x + ''", "11")]
	[InlineData("'1' + '2' + 'c'", "12c")]

	public void ConcatenatingStrings_Succeeds(string expressionString, object? expectedOutput)
	{
		var extendedExpression = new ExtendedExpression(expressionString);
		extendedExpression.Parameters["x"] = x;
		var result = extendedExpression.Evaluate();
		result.Should().BeEquivalentTo(expectedOutput);
	}
}