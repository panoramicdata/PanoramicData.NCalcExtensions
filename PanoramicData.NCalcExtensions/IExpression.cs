using NCalc.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcExtensions;

internal interface IExpression
{
	LogicalExpression? LogicalExpression { get; }
	IDictionary<string, object?> Parameters { get; }
	object? Evaluate();
	ValueTask<object?> EvaluateAsync();
}
