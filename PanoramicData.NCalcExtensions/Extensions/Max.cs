using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Max
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var originalList = functionArgs.Parameters[0].Evaluate();

		if (functionArgs.Parameters.Length == 1)
		{
			functionArgs.Result = originalList switch
			{
				null => null,
				IEnumerable<byte> list => list.Cast<int>().Max(),
				IEnumerable<byte?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<short> list => list.Cast<int>().Max(),
				IEnumerable<short?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<int> list => list.Max(),
				IEnumerable<int?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<long> list => list.Max(),
				IEnumerable<long?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<float> list => list.Max(),
				IEnumerable<float?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<double> list => list.Max(),
				IEnumerable<double?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<decimal> list => list.Max(),
				IEnumerable<decimal?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<string?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<object?> list when list.All(x => x is string or null) => list.DefaultIfEmpty(null).Max(x => x as string),
				IEnumerable<object?> list => GetMax(list),
				_ => throw new FormatException($"First {ExtensionFunction.Max} parameter must be an IEnumerable of a numeric or string type if only on parameter is present.")
			};

			return;
		}

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			 ?? throw new FormatException($"Second {ExtensionFunction.Max} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			 ?? throw new FormatException($"Third {ExtensionFunction.Max} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);

		functionArgs.Result = originalList switch
		{
			IEnumerable<byte> list => list.Cast<int>().Max(value => (int?)lambda.Evaluate(value)),
			IEnumerable<byte?> list => list.Cast<int>().Max(value => (int?)lambda.Evaluate(value)),
			IEnumerable<short> list => list.Cast<int>().Max(value => (int?)lambda.Evaluate(value)),
			IEnumerable<short?> list => list.Cast<int>().Max(value => (int?)lambda.Evaluate(value)),
			IEnumerable<int> list => list.Max(value => (int?)lambda.Evaluate(value)),
			IEnumerable<int?> list => list.Max(value => (int?)lambda.Evaluate(value)),
			IEnumerable<long> list => list.Max(value => (long?)lambda.Evaluate(value)),
			IEnumerable<long?> list => list.Max(value => (long?)lambda.Evaluate(value)),
			IEnumerable<float> list => list.Max(value => (float?)lambda.Evaluate(value)),
			IEnumerable<float?> list => list.Max(value => (float?)lambda.Evaluate(value)),
			IEnumerable<double> list => list.Max(value => (double?)lambda.Evaluate(value)),
			IEnumerable<double?> list => list.Max(value => (double?)lambda.Evaluate(value)),
			IEnumerable<decimal> list => list.Max(value => (decimal?)lambda.Evaluate(value)),
			IEnumerable<decimal?> list => list.Max(value => (decimal?)lambda.Evaluate(value)),
			IEnumerable<string?> list => list.Max(value => (string?)lambda.Evaluate(value)),
			_ => throw new FormatException($"First {ExtensionFunction.Max} parameter must be an IEnumerable of a numeric type.")
		};

	}

	private static double GetMax(IEnumerable<object?> objectList)
	{
		double max = 0;
		foreach (var item in objectList)
		{
			var thisOne = item switch
			{
				byte value => value,
				short value => value,
				int value => value,
				long value => value,
				float value => value,
				double value => value,
				decimal value => (double)value,
				JValue jValue => jValue.Type switch
				{
					JTokenType.Float => jValue.Value<float>(),
					JTokenType.Integer => jValue.Value<int>(),
					_ => throw new FormatException($"Found unsupported JToken type '{jValue.Type}' when completing sum.")
				},
				null => 0,
				_ => throw new FormatException($"Found unsupported type '{item?.GetType().Name}' when completing sum.")
			};
			if (thisOne < max)
			{
				max = thisOne;
			}
		}

		return max;
	}
}
