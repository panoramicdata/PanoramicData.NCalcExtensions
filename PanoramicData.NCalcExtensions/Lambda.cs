using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions;
public class Lambda(
	string predicate,
	string nCalcString,
	Dictionary<string, object?> parameters)
{
	public object? Evaluate<T>(T value)
	{
		parameters.Remove(predicate);
		parameters.Add(predicate, value!);
		var ncalc = new ExtendedExpression(nCalcString)
		{
			Parameters = parameters
		};

		return ncalc.Evaluate();
	}
}