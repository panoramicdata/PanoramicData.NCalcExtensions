namespace PanoramicData.NCalcExtensions.Test;

public class CountByTests
{
	[Theory]
	[InlineData("countBy(list(1, 2, 2, 3, 3, 3, 4), 'n', 'toLower(toString(n > 1))')", "{\"false\":1,\"true\":6}")]
	[InlineData("countBy(list(1, 2, 2, 3, 3, 3, 4), 'n', 'toString(n)')", "{\"1\":1,\"2\":2,\"3\":3,\"4\":1}")]
	public void CountBy_ReturnsExpectedResult(string expressionText, string expectedResult)
	{
		var expression = new ExtendedExpression(expressionText);

		var result = expression.Evaluate();
		result.Should().BeOfType<JObject>();
		result.Should().NotBeNull();
		result.ToString()!
			.ReplaceLineEndings(string.Empty)
			.Replace(" ", string.Empty, StringComparison.Ordinal)
			.Replace("\t", string.Empty, StringComparison.Ordinal)
			.Should().Be(expectedResult);
	}
}
