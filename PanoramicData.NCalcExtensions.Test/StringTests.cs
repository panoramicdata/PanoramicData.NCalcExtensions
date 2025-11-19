namespace PanoramicData.NCalcExtensions.Test;

public class StringTests
{
	private const string x = "1";

	[Theory]
	[InlineData("'1' + '1'", typeof(double), 2)]
	[InlineData("x + x", typeof(double), 2)]
	[InlineData("'' + '1' + '1'", typeof(double), 2)]
	[InlineData("'' + x + x", typeof(double), 2)]
	public void ConcatenatingStrings_Succeeds(
		string expressionString,
		Type type,
		object? expectedOutput)
	{
		var extendedExpression = new ExtendedExpression(expressionString);
		extendedExpression.Parameters["x"] = x;
		var result = extendedExpression.Evaluate();
		result.Should().BeOfType(type);
		result.Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("'1' + '2' + 'c'")]
	[InlineData("x + x + ''")]
	public void ConcatenatingStrings_Fails(string expressionString)
	{
		var expression = new ExtendedExpression(expressionString);
		expression.Parameters["x"] = x;
		((Action)(() => expression.Evaluate()))
			.Should()
			.Throw<FormatException>();
	}
}
