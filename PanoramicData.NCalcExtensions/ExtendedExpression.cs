namespace PanoramicData.NCalcExtensions;

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
		if (Parameters.ContainsKey("True"))
		{
			throw new InvalidOperationException("You may not set a parameter called 'True', as it is a reserved keyword.");
		}
		Parameters["True"] = true;
		if (Parameters.ContainsKey("False"))
		{
			throw new InvalidOperationException("You may not set a parameter called 'False', as it is a reserved keyword.");
		}
		Parameters["False"] = false;
	}
}
