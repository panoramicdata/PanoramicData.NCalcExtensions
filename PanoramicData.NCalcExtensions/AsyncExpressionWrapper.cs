using NCalc.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcExtensions;

internal class AsyncExpressionWrapper : IExpression
{
	AsyncExpression _expression;

	public AsyncExpressionWrapper(AsyncExpression expression)
	{
		_expression = expression;
	}

	public IDictionary<string, object?> Parameters { get => _expression.Parameters; }
	public LogicalExpression? LogicalExpression { get => _expression.LogicalExpression; }

	public object? Evaluate() => _expression.EvaluateAsync().GetAwaiter().GetResult();
	public ValueTask<object?> EvaluateAsync() => _expression.EvaluateAsync();
}
