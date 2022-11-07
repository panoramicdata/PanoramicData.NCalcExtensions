namespace PanoramicData.NCalcExtensions.Test;

public abstract class NCalcTest
{
	protected static object Test(string expressionText)
	{
		var expression = new Expression(expressionText);
		expression.EvaluateFunction += NCalcExtensions.Extend;
		return expression.Evaluate();
	}
}
