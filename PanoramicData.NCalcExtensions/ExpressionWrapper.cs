using NCalc.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PanoramicData.NCalcExtensions;

internal class ExpressionWrapper : IExpression
{
	readonly Expression _expression;

	public ExpressionWrapper(Expression expression)
	{
		_expression = expression;
	}

	public LogicalExpression? LogicalExpression { get => _expression.LogicalExpression; }

	public IDictionary<string, object?> Parameters { get => _expression.Parameters; }
	public object? Evaluate() => _expression.Evaluate();
	public ValueTask<object?> EvaluateAsync() => new ValueTask<object?>(Evaluate());
}
