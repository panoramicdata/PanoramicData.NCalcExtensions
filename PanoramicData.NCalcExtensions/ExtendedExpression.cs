using NCalc;
namespace PanoramicData.NCalcExtensions
{
	public class ExtendedExpression : Expression
	{
		public ExtendedExpression(string expression) : base(expression)
		{
			EvaluateFunction += NCalcExtensions.Extend;
			CacheEnabled = false;
		}
	}
}
