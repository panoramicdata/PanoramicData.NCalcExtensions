namespace PanoramicData.NCalcExtensions;
public class Lambda
{
	private readonly string _predicate;
	private readonly string _nCalcString;
	private readonly IDictionary<string, object?> _parameters;
	private readonly SimpleExtendedExpression _expression;

	public Lambda(string predicate, string nCalcString, IDictionary<string, object?> parameters)
	{
		_predicate = predicate;
		_nCalcString = nCalcString;
		_parameters = parameters;

		var document = ExtendedExpressionDocumentParser.Parse(nCalcString);
		_expression = new SimpleExtendedExpression(document.TidiedExpression, document);
		_expression.Parameters = _parameters;
	}

	public object? Evaluate<T>(T value)
	{
		_parameters[_predicate] = UnwrapValue(value);
		return _expression.Evaluate();
	}

	public TResult EvaluateTo<TValue, TResult>(TValue value)
	{
		_parameters[_predicate] = UnwrapValue(value);

		var resultObject = _expression.Evaluate();

		if (resultObject is TResult result)
		{
			return result;
		}
		else
		{
			// If the result is null and TResult is a nullable type, return default
			if (resultObject == null && (default(TResult) == null || Nullable.GetUnderlyingType(typeof(TResult)) != null))
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

	private static object? UnwrapValue<T>(T value) => value switch
	{
		JValue jValue => jValue.Value,
		_ => value
	};
}