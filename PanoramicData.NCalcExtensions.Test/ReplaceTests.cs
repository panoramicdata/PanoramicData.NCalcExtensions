namespace PanoramicData.NCalcExtensions.Test;

public class ReplaceTests
{
	[Fact]
	public void Replace_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("replace('abcdefg', 'cde', 'CDE')");
		(expression.Evaluate() as string).Should().Be("abCDEfg");
	}

	[Fact]
	public void Replace_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("replace('abcdefg', 'cde', '')");
		(expression.Evaluate() as string).Should().Be("abfg");
	}

	[Fact]
	public void Replace_Example3_Succeeds()
	{
		var expression = new ExtendedExpression("replace('abcdefg', 'a', '1', 'bc', '23')");
		(expression.Evaluate() as string).Should().Be("123defg");
	}
}
