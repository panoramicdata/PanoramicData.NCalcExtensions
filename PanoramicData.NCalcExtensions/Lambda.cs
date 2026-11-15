namespace PanoramicData.NCalcExtensions;
public class Lambda
{
	private readonly string _predicate;
	private readonly string _nCalcString;
	private readonly SimpleExtendedExpression _expression;

	public Lambda(string predicate, string nCalcString, IDictionary<string, object?> parameters)
		: this(predicate, nCalcString, parameters, ExpressionSettings.From(parameters))
	{
	}

	/// <summary>Creates a lambda that evaluates with the options, culture and event handlers of the calling expression.</summary>
	public Lambda(string predicate, string nCalcString, FunctionEventArgs functionArgs)
		: this(predicate, nCalcString, (functionArgs ?? throw new ArgumentNullException(nameof(functionArgs))).Context.Parameters, functionArgs)
	{
	}

	internal Lambda(string predicate, string nCalcString, IDictionary<string, object?> parameters, FunctionEventArgs functionArgs)
		: this(predicate, nCalcString, parameters, ExpressionSettings.From(functionArgs.Context.Parameters))
	{
		var context = functionArgs.Context;

		// The lambda registers its own built-in functions, so only names it does not know go to the caller's handler.
		_expression.EvaluateFunction += (name, args) =>
		{
			if (!BaseExtendedExpression.IsBuiltInFunction(name))
			{
				context.EvaluateFunctionHandler?.Invoke(name, args);
			}
		};
		_expression.EvaluateParameter += (name, args) => context.EvaluateParameterHandler?.Invoke(name, args);
	}

	private Lambda(
		string predicate,
		string nCalcString,
		IDictionary<string, object?> parameters,
		ExpressionSettings settings)
	{
		_predicate = predicate;
		_nCalcString = nCalcString;

		var document = ExtendedExpressionDocumentParser.Parse(nCalcString);
		_expression = new SimpleExtendedExpression(document.TidiedExpression, document, settings.Options, settings.CultureInfo);
		foreach (var (key, parameter) in parameters)
		{
			_expression.Parameters[key] = parameter;
		}
	}

	public object? Evaluate<T>(T value)
	{
		_expression.Parameters[_predicate] = UnwrapValue(value);
		return _expression.Evaluate();
	}

	public TResult EvaluateTo<TValue, TResult>(TValue value)
	{
		_expression.Parameters[_predicate] = UnwrapValue(value);

		var resultObject = _expression.Evaluate();

		if (resultObject is TResult result)
		{
			return result;
		}
		else
		{
			// If the result is null and TResult is a nullable type, return default
			if (resultObject == null && (!typeof(TResult).IsValueType || Nullable.GetUnderlyingType(typeof(TResult)) != null))
			{
				return default!;
			}

			// Try casting it
			if (resultObject is IConvertible)
			{
				return (TResult)Convert.ChangeType(resultObject, typeof(TResult), CultureInfo.InvariantCulture);
			}

			throw new InvalidCastException($"Could not cast result of expression '{_nCalcString}' to type {typeof(TResult).FullName}.");
		}
	}

	private static object? UnwrapValue<T>(T value) => value is JValue jValue ? jValue.Value : value;
}
