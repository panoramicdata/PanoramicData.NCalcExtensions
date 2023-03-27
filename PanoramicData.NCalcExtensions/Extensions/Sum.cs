using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Sum
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var list = functionArgs.Parameters[0].Evaluate();

		if (functionArgs.Parameters.Length == 1)
		{
			functionArgs.Result = list switch
			{
				IEnumerable<byte> byteList => byteList.Cast<int>().Sum(),
				IEnumerable<short> shortList => shortList.Cast<int>().Sum(),
				IEnumerable<int> intList => intList.Sum(),
				IEnumerable<long> longList => longList.Sum(),
				IEnumerable<float> floatList => floatList.Sum(),
				IEnumerable<double> doubleList => doubleList.Sum(),
				IEnumerable<decimal> decimalList => decimalList.Sum(),
				IEnumerable<object?> objectList => GetSum(objectList),
				_ => throw new FormatException($"First {ExtensionFunction.Sum} parameter must be an IEnumerable of a numeric type.")
			};
			return;
		}

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.Sum} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.Sum} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, new());

		functionArgs.Result = list switch
		{
			IEnumerable<byte> byteList => byteList.Cast<int>().Sum(value => (int?)lambda.Evaluate(value)),
			IEnumerable<short> shortList => shortList.Cast<int>().Sum(value => (int?)lambda.Evaluate(value)),
			IEnumerable<int> intList => intList.Sum(value => (int?)lambda.Evaluate(value)),
			IEnumerable<long> longList => longList.Sum(value => (long?)lambda.Evaluate(value)),
			IEnumerable<float> floatList => floatList.Sum(value => (float?)lambda.Evaluate(value)),
			IEnumerable<double> doubleList => doubleList.Sum(value => (double?)lambda.Evaluate(value)),
			IEnumerable<decimal> decimalList => decimalList.Sum(value => (decimal?)lambda.Evaluate(value)),
			_ => throw new FormatException($"First {ExtensionFunction.Sum} parameter must be an IEnumerable of a numeric type.")
		};
	}

	private static double GetSum(IEnumerable<object?> objectList)
	{
		double sum = 0;
		foreach (var item in objectList)
		{
			sum += item switch
			{
				byte byteValue => byteValue,
				short shortValue => shortValue,
				int intValue => intValue,
				long longValue => longValue,
				float floatValue => floatValue,
				double doubleValue => doubleValue,
				decimal decimalValue => (double)decimalValue,
				JValue jValue => jValue.Type switch
				{
					JTokenType.Float => jValue.Value<float>(),
					JTokenType.Integer => jValue.Value<int>(),
					_ => throw new FormatException($"Found unsupported JToken type '{jValue.Type}' when completing sum.")
				},
				null => 0,
				_ => throw new FormatException($"Found unsupported type '{item?.GetType().Name}' when completing sum.")
			};
		}

		return sum;
	}
}
