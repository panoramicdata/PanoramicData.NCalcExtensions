namespace PanoramicData.NCalcExtensions.Test;

public class ParameterTests
{
	[Fact]
	public void SquareBracketParameter_Succeeds()
	{
		var expression = new ExtendedExpression("[a.b]");
		expression.Parameters["a.b"] = "AAAA";
		(expression.Evaluate() as string).Should().Be("AAAA");
	}
}
