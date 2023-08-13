using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions;
public class Lambda
{
	private readonly string predicate;
	private readonly string nCalcString;
	private readonly Dictionary<string, object?> parameters;

	public Lambda(
		string predicate,
		string nCalcString,
		Dictionary<string, object?> parameters)
	{
		this.predicate = predicate;
		this.nCalcString = nCalcString;
		this.parameters = parameters;
	}

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