using NCalc.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcExtensions;

internal class AsyncExpressionWrapper : IExpression
{
	readonly AsyncExpression _expression;

	public AsyncExpressionWrapper(AsyncExpression expression)
	{
		_expression = expression;
	}

	public IDictionary<string, object?> Parameters { get => _expression.Parameters; }
	public LogicalExpression? LogicalExpression { get => _expression.LogicalExpression; }

	public object? Evaluate()
	{
		var valueTask = _expression.EvaluateAsync();

		if (valueTask.IsCompletedSuccessfully) 
		{
			return valueTask.Result;
		}
		else
		{
			return valueTask.AsTask().GetAwaiter().GetResult();
		}
	}

	public ValueTask<object?> EvaluateAsync() => _expression.EvaluateAsync();
}
