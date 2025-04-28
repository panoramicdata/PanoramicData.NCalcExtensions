using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions;
public class Lambda(
	string predicate,
	string nCalcString,
	IDictionary<string, object?> parameters)
{
	public object? Evaluate(object value)
	{
		parameters.Remove(predicate);
		parameters.Add(predicate, value!);
		var ncalc = new ExtendedExpression(nCalcString)
		{
			Parameters = parameters
		};

		return ncalc.Evaluate();
	}

	public TOut? Evaluate<T, TOut>(T value)
	{
		parameters.Remove(predicate);
		parameters.Add(predicate, value!);
		var ncalc = new ExtendedExpression(nCalcString)
		{
			Parameters = parameters
		};

		var val = ncalc.Evaluate();

		return (TOut)Convert.ChangeType(val, typeof(TOut), CultureInfo.InvariantCulture);
	}

	public TOut? Evaluate<TOut>(object value)
	{
		parameters.Remove(predicate);
		parameters.Add(predicate, value!);
		var ncalc = new ExtendedExpression(nCalcString, typeof(TOut) == typeof(string) ? (ExpressionOptions.StringConcat) : (ExpressionOptions.NoCache))
		{
			Parameters = parameters
		};

		var val = ncalc.Evaluate();

		return (TOut)Convert.ChangeType(val, typeof(TOut), CultureInfo.InvariantCulture);
	}
}