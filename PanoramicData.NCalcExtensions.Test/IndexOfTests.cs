namespace PanoramicData.NCalcExtensions.Test;
public class IndexOfTests
{
	[Theory]
	[InlineData("abc", "a", 0)]
	[InlineData("abc", "b", 1)]
	[InlineData("abc", "c", 2)]
	[InlineData("abc", "d", -1)]
	public void IndexOf_InlineData_ResultsMatchExpectation(string list, string item, object expected)
	{
		var expr = new ExtendedExpression($"indexOf('{list}','{item}')");
		var result = expr.Evaluate();
		result.Should().Be(expected);
	}

	[Fact]
	public void IndexOf_NullFirstParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("indexOf(null, 'search')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}

	[Fact]
	public void IndexOf_NullSecondParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("indexOf('string', null)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}

	[Fact]
	public void IndexOf_NonStringParameters_ThrowsException()
	{
		var expression = new ExtendedExpression("indexOf(123, 456)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>();
	}
}
