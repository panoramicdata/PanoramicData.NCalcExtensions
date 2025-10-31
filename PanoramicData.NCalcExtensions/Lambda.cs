namespace PanoramicData.NCalcExtensions;
public class Lambda(
	string predicate,
	string nCalcString,
	IDictionary<string, object?> parameters)
{
	public object? Evaluate<T>(T value)
	{
		parameters.Remove(predicate);
		parameters.Add(predicate, value);
		var ncalc = new ExtendedExpression(nCalcString)
		{
			Parameters = parameters
		};

		return ncalc.Evaluate();
	}

	public TResult EvaluateTo<TValue, TResult>(TValue value)
	{
		parameters.Remove(predicate);
		parameters.Add(predicate, value);
		var ncalc = new ExtendedExpression(nCalcString)
		{
			Parameters = parameters
		};

		var resultObject = ncalc.Evaluate();

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

			throw new InvalidCastException($"Could not cast result of expression '{nCalcString}' to type {typeof(TResult).FullName}.");
		}
	}
}