using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions;

public class AsyncExtendedExpression : AsyncExpression
{
	private readonly Dictionary<string, object?> _storageDictionary = [];

	public AsyncExtendedExpression(string expression) :
		this(expression, ExpressionOptions.None | ExpressionOptions.NoCache, CultureInfo.InvariantCulture)
	{ }

	public AsyncExtendedExpression(string expression, ExpressionOptions expressionOptions) :
		this(expression, expressionOptions, CultureInfo.InvariantCulture)
	{ }

	public AsyncExtendedExpression(
		string expression,
		ExpressionOptions expressionOptions,
		CultureInfo cultureInfo) : base(expression.TidyExpression(), expressionOptions, cultureInfo)
	{
		ExpressionHelper.Configure(this.Parameters, _storageDictionary);
		EvaluateFunctionAsync += (fn, args) => ExpressionHelper.Extend(fn, args, _storageDictionary, cultureInfo);
	}

	public AsyncExtendedExpression(
		string expression,
		AsyncExpressionContext context) : base(expression.TidyExpression(), context)
	{
		ExpressionHelper.Configure(this.Parameters, _storageDictionary);
		EvaluateFunctionAsync += (fn, args) => ExpressionHelper.Extend(fn, args, _storageDictionary, context.CultureInfo);
	}

	public object? Evaluate()
	{
		// TODO : this is a hack to make the existing functions still work
		return this.EvaluateAsync().AsTask().GetAwaiter().GetResult();
	}
}
