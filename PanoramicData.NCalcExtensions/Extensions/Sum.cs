using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("sum")]
	[Description("Sums numeric items. Optionally, perform a lambda on each one first.")]
	object? Sum(
		[Description("The original list.")]
		IList list,
		[Description("(Optional) a string to represent the value to be evaluated")]
		string? predicate = null,
		[Description("(Optional, but must be provided if predicate is) the string to evaluate")]
		string? exprStr = null
	);
}

internal static class Sum
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		var originalList = functionArgs.Parameters[0].Evaluate();

		if (functionArgs.Parameters.Length == 1)
		{
			functionArgs.Result = originalList switch
			{
				IEnumerable<byte> list => (byte)list.Select(l => (int)l).Sum(),
				IEnumerable<short> list => (short)list.Select(l => (int)l).Sum(),
				IEnumerable<int> list => list.Sum(),
				IEnumerable<long> list => list.Sum(),
				IEnumerable<float> list => list.Sum(),
				IEnumerable<double> list => list.Sum(),
				IEnumerable<decimal> list => list.Sum(),
				IEnumerable<object?> list => GetSum(list),
				_ => throw new FormatException($"First {ExtensionFunction.Sum} parameter must be an IEnumerable of a numeric type if only one parameter is present. Received a {originalList.GetType().Name}.")
			};
			return;
		}

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.Sum} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.Sum} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);

		functionArgs.Result = originalList switch
		{
			IEnumerable<byte> byteList => byteList.Select(l => (int)l).Sum(value => (int?)lambda.Evaluate(value)),
			IEnumerable<short> shortList => shortList.Select(l => (int)l).Sum(value => (int?)lambda.Evaluate(value)),
			IEnumerable<int> intList => intList.Sum(value => (int?)lambda.Evaluate(value)),
			IEnumerable<long> longList => longList.Sum(value => (long?)lambda.Evaluate(value)),
			IEnumerable<float> floatList => floatList.Sum(value => (float?)lambda.Evaluate(value)),
			IEnumerable<double> doubleList => doubleList.Sum(value => (double?)lambda.Evaluate(value)),
			IEnumerable<decimal> decimalList => decimalList.Sum(value => (decimal?)lambda.Evaluate(value)),
			IEnumerable<object?> list => GetSum(list),
			_ => throw new FormatException($"First {ExtensionFunction.Sum} parameter must be an IEnumerable of a numeric type.   Received a {originalList.GetType().Name}<{string.Join(", ", originalList.GetType().GetGenericArguments().Select(t => t.Name))}>.")
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