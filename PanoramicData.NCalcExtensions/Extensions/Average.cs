using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("average")]
	[Description("Calculates the average of numeric items in a list.")]
	double Average(
		[Description("The list of numeric values.")]
		IList list
	);
}

internal static class AverageFunction
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		var originalList = functionArgs.Parameters.Evaluate(0)
			?? throw new FormatException($"First {ExtensionFunction.Average} parameter cannot be null.");

		functionArgs.Result = originalList switch
		{
			IEnumerable<byte> list => list.Select(l => (double)l).DefaultIfEmpty(double.NaN).Average(),
			IEnumerable<short> list => list.Select(l => (double)l).DefaultIfEmpty(double.NaN).Average(),
			IEnumerable<int> list => list.Select(l => (double)l).DefaultIfEmpty(double.NaN).Average(),
			IEnumerable<long> list => list.Select(l => (double)l).DefaultIfEmpty(double.NaN).Average(),
			IEnumerable<float> list => list.Select(l => (double)l).DefaultIfEmpty(double.NaN).Average(),
			IEnumerable<double> list => list.DefaultIfEmpty(double.NaN).Average(),
			IEnumerable<decimal> list => (double)list.DefaultIfEmpty(0).Average(),
			IEnumerable<object?> list => GetAverage(list),
			_ => throw new FormatException($"First {ExtensionFunction.Average} parameter must be an IEnumerable of a numeric type. Received a {originalList.GetType().Name}.")
		};
	}

	private static double GetAverage(IEnumerable<object?> objectList)
	{
		double sum = 0;
		var count = 0;
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
				null => 0,
				_ => throw new FormatException($"Found unsupported type '{item.GetType().Name}' when completing average.")
			};
			count++;
		}

		return count == 0 ? double.NaN : sum / count;
	}
}
