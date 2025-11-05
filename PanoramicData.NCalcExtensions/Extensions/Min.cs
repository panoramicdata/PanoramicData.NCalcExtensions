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
				IEnumerable<sbyte> list => list.Min(),
				IEnumerable<sbyte?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<byte> list => list.Min(),
				IEnumerable<byte?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<short> list => list.Min(),
				IEnumerable<short?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<ushort> list => list.Min(),
				IEnumerable<ushort?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<int> list => list.Min(),
				IEnumerable<int?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<uint> list => list.Min(),
				IEnumerable<uint?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<long> list => list.Min(),
				IEnumerable<long?> list => list.DefaultIfEmpty(null).Min(),
				IEnumerable<ulong> list => list.Min(),
				IEnumerable<ulong?> list => list.DefaultIfEmpty(null).Min(),
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
			IEnumerable<sbyte> list => list.Min(lambda.EvaluateTo<sbyte, sbyte>),
			IEnumerable<sbyte?> list => list.Min(lambda.EvaluateTo<sbyte?, sbyte?>),
			IEnumerable<byte> list => list.Min(lambda.EvaluateTo<byte, byte>),
			IEnumerable<byte?> list => list.Min(lambda.EvaluateTo<byte?, byte?>),
			IEnumerable<short> list => list.Min(lambda.EvaluateTo<short, short>),
			IEnumerable<short?> list => list.Min(lambda.EvaluateTo<short?, short?>),
			IEnumerable<ushort> list => list.Min(lambda.EvaluateTo<ushort, ushort>),
			IEnumerable<ushort?> list => list.Min(lambda.EvaluateTo<ushort?, ushort?>),
			IEnumerable<int> list => list.Min(lambda.EvaluateTo<int, int>),
			IEnumerable<int?> list => list.Min(lambda.EvaluateTo<int?, int?>),
			IEnumerable<uint> list => list.Min(lambda.EvaluateTo<uint, uint>),
			IEnumerable<uint?> list => list.Min(lambda.EvaluateTo<uint?, uint?>),
			IEnumerable<long> list => list.Min(lambda.EvaluateTo<long, long>),
			IEnumerable<long?> list => list.Min(lambda.EvaluateTo<long?, long?>),
			IEnumerable<ulong> list => list.Min(lambda.EvaluateTo<ulong, ulong>),
			IEnumerable<ulong?> list => list.Min(lambda.EvaluateTo<ulong?, ulong?>),
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

	private static IComparable GetMin(IEnumerable<object?> objectList)
	{
		IComparable? min = null;
		foreach (var item in objectList)
		{
			var thisOne = item switch
			{
				sbyte value => (IComparable)value,
				byte value => (IComparable)value,
				short value => (IComparable)value,
				ushort value => (IComparable)value,
				int value => (IComparable)value,
				uint value => (IComparable)value,
				long value => (IComparable)value,
				ulong value => (IComparable)value,
				float value => (IComparable)value,
				double value => (IComparable)value,
				decimal value => (IComparable)value,
				JValue jValue => jValue.Type switch
				{
					JTokenType.Float => (IComparable)jValue.Value<float>()!,
					JTokenType.Integer => (IComparable)jValue.Value<int>(),
					_ => throw new FormatException($"Found unsupported JToken type '{jValue.Type}' when completing Min.")
				},
				null => null,
				_ => throw new FormatException($"Found unsupported type '{item?.GetType().Name}' when completing Min.")
			};
			if (thisOne != null && (min == null || thisOne.CompareTo(min) < 0))
			{
				min = thisOne;
			}
		}

		return min!;
	}
}
