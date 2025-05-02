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
		ExpressionHelper.Extend(_storageDictionary, Context);
	}

	public AsyncExtendedExpression(
		string expression,
		AsyncExpressionContext context) : base(expression.TidyExpression(), context)
	{
		ExpressionHelper.Configure(this.Parameters, _storageDictionary);
		ExpressionHelper.Extend(_storageDictionary, Context);
	}

	public object? Evaluate()
	{
		// TODO : this is a hack to make the existing functions still work
		var valueTask = EvaluateAsync();

		if (valueTask.IsCompletedSuccessfully)
		{
			return valueTask.Result;
		}
		else
		{
			return valueTask.AsTask().GetAwaiter().GetResult();
		}
	}
}
