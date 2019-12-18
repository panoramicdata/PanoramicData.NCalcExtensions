using NCalc;

namespace PanoramicData.NCalcExtensions.Test
{
	public class NCalcTest
	{
		protected static object Test(string expressionText)
		{
			var expression = new Expression(expressionText);
			expression.EvaluateFunction += NCalcExtensions.Extend;
			return expression.Evaluate();
		}
	}
}