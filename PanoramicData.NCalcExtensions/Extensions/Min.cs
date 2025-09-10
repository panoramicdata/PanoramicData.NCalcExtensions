using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("min")]
	[Description("Emits the minimum value, ignoring nulls.")]
	object? Min(
		[Description("The list of values")]
		IEnumerable<object?> list,
		[Description("(Optional) a string to represent the value to be evaluated")]
		string? predicate = null,
		[Description("(Optional, but must be provided if predicate is) the string to evaluate")]
		string? exprStr = null
	);
}

internal static class Min
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var originalListUntyped = functionArgs.Parameters[0].Evaluate();

		if (originalListUntyped is null)
		{
			functionArgs.Result = null;
			return;
		}

		var originalList = originalListUntyped as IEnumerable ?? throw new FormatException($"First {ExtensionFunction.Min} parameter must be an IEnumerable.");

		if (functionArgs.Parameters.Length == 1)
		{
			functionArgs.Result = originalList switch
			{
				null => null,
				IEnumerable<byte> list => list.Min(),
				IEnumerable<byte?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<short> list => list.Min(),
				IEnumerable<short?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<int> list => list.Min(),
				IEnumerable<int?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<long> list => list.Min(),
				IEnumerable<long?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<float> list => list.Min(),
				IEnumerable<float?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<double> list => list.Min(),
				IEnumerable<double?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<decimal> list => list.Min(),
				IEnumerable<decimal?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<string?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<object?> list when list.All(x => x is string or null) => list.DefaultIfEmpty(null).Min(x => x as string),
				IEnumerable<object?> list => GetMin(list),
				_ => throw new FormatException($"First {ExtensionFunction.Min} parameter must be an IEnumerable of a numeric or string type if only one parameter is present.")
			};

			return;
		}

		// It's in lambda form

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			 ?? throw new FormatException($"Second {ExtensionFunction.Min} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			 ?? throw new FormatException($"Third {ExtensionFunction.Min} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);

		functionArgs.Result = originalList switch
		{
			IEnumerable<byte> list => list.Min(lambda.EvaluateTo<byte, byte>),
			IEnumerable<byte?> list => list.Min(lambda.EvaluateTo<byte?, byte?>),
			IEnumerable<short> list => list.Min(lambda.EvaluateTo<short, short>),
			IEnumerable<short?> list => list.Min(lambda.EvaluateTo<short?, short?>),
			IEnumerable<int> list => list.Min(lambda.EvaluateTo<int, int>),
			IEnumerable<int?> list => list.Min(lambda.EvaluateTo<int?, int?>),
			IEnumerable<long> list => list.Min(lambda.EvaluateTo<long, long>),
			IEnumerable<long?> list => list.Min(lambda.EvaluateTo<long?, long?>),
			IEnumerable<float> list => list.Min(lambda.EvaluateTo<float, float>),
			IEnumerable<float?> list => list.Min(lambda.EvaluateTo<float?, float?>),
			IEnumerable<double> list => list.Min(lambda.EvaluateTo<double, double>),
			IEnumerable<double?> list => list.Min(lambda.EvaluateTo<double?, double?>),
			IEnumerable<decimal> list => list.Min(lambda.EvaluateTo<decimal, decimal>),
			IEnumerable<decimal?> list => list.Min(lambda.EvaluateTo<decimal?, decimal?>),
			IEnumerable<string?> list => list.Min(lambda.EvaluateTo<string?, string?>),
			IEnumerable<object?> list => GetMin(list.Select(value => lambda.Evaluate(value))),
			_ => throw new FormatException($"First {ExtensionFunction.Min} parameter must be an IEnumerable of a string or numeric type when processing as a lambda.")
		};

	}

	private static double GetMin(IEnumerable<object?> objectList)
	{
		var min = double.PositiveInfinity;
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
					_ => throw new FormatException($"Found unsupported JToken type '{jValue.Type}' when completing Min.")
				},
				null => 0,
				_ => throw new FormatException($"Found unsupported type '{item?.GetType().Name}' when completing Min.")
			};
			if (thisOne < min)
			{
				min = thisOne;
			}
		}

		return min;
	}
}
