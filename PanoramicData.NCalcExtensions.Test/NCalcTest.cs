namespace PanoramicData.NCalcExtensions.Test;

public abstract class NCalcTest
{
	protected static object Test(string expressionText)
	{
		var expression = new ExtendedExpression(expressionText);
		return expression.Evaluate();
	}
}
