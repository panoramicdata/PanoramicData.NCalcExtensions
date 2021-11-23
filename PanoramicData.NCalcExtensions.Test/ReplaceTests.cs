namespace PanoramicData.NCalcExtensions.Test;

public class ReplaceTests
{
	[Fact]
	public void Replace_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("replace('abcdefg', 'cde', 'CDE')");
		Assert.Equal("abCDEfg", expression.Evaluate() as string);
	}

	[Fact]
	public void Replace_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("replace('abcdefg', 'cde', '')");
		Assert.Equal("abfg", expression.Evaluate() as string);
	}
}
