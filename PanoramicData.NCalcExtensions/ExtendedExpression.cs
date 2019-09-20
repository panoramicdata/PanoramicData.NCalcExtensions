using NCalc;
using System;

namespace PanoramicData.NCalcExtensions
{
	public class ExtendedExpression : Expression
	{
		public ExtendedExpression(string expression) : base(expression)
		{
			EvaluateFunction += NCalcExtensions.Extend;
			CacheEnabled = false;
			if (Parameters.ContainsKey("null"))
			{
				throw new InvalidOperationException("You may not set a parameter called 'null', as it is a reserved keyword.");
			}
			Parameters["null"] = null;
		}
	}
}
