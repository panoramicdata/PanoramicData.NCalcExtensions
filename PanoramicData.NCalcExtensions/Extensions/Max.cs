using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("max")]
	[Description("Emits the maximum value, ignoring nulls.")]
	object? Max(
		[Description("The list of values")]
		IEnumerable<object?> list,
		[Description("(Optional) a string to represent the value to be evaluated")]
		string? predicate = null,
		[Description("(Optional, but must be provided if predicate is) the string to evaluate")]
		string? exprStr = null
	);
}

internal static class Max
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var originalListUntyped = functionArgs.Parameters[0].Evaluate();

		if (originalListUntyped is null)
		{
			functionArgs.Result = null;
			return;
		}

		var originalList = originalListUntyped as IEnumerable ?? throw new FormatException($"First {ExtensionFunction.Max} parameter must be an IEnumerable.");

		if (functionArgs.Parameters.Length == 1)
		{
			functionArgs.Result = originalList switch
			{
				null => null,
				IEnumerable<sbyte> list => list.Max(),
				IEnumerable<sbyte?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<byte> list => list.Max(),
				IEnumerable<byte?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<short> list => list.Max(),
				IEnumerable<short?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<ushort> list => list.Max(),
				IEnumerable<ushort?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<int> list => list.Max(),
				IEnumerable<int?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<uint> list => list.Max(),
				IEnumerable<uint?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<long> list => list.Max(),
				IEnumerable<long?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<ulong> list => list.Max(),
				IEnumerable<ulong?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<float> list => list.Max(),
				IEnumerable<float?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<double> list => list.Max(),
				IEnumerable<double?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<decimal> list => list.Max(),
				IEnumerable<decimal?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<string?> list => list.DefaultIfEmpty(null).Max(),
				IEnumerable<object?> list when list.All(x => x is string or null) => list.DefaultIfEmpty(null).Max(x => x as string),
				IEnumerable<object?> list => GetMax(list),
				_ => throw new FormatException($"First {ExtensionFunction.Max} parameter must be an IEnumerable of a numeric or string type if only one parameter is present.")
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
			IEnumerable<sbyte> list => list.Max(lambda.EvaluateTo<sbyte, sbyte>),
			IEnumerable<sbyte?> list => list.Max(lambda.EvaluateTo<sbyte?, sbyte?>),
			IEnumerable<byte> list => list.Max(lambda.EvaluateTo<byte, byte>),
			IEnumerable<byte?> list => list.Max(lambda.EvaluateTo<byte?, byte?>),
			IEnumerable<short> list => list.Max(lambda.EvaluateTo<short, short>),
			IEnumerable<short?> list => list.Max(lambda.EvaluateTo<short?, short?>),
			IEnumerable<ushort> list => list.Max(lambda.EvaluateTo<ushort, ushort>),
			IEnumerable<ushort?> list => list.Max(lambda.EvaluateTo<ushort?, ushort?>),
			IEnumerable<int> list => list.Max(lambda.EvaluateTo<int, int>),
			IEnumerable<int?> list => list.Max(lambda.EvaluateTo<int?, int?>),
			IEnumerable<uint> list => list.Max(lambda.EvaluateTo<uint, uint>),
			IEnumerable<uint?> list => list.Max(lambda.EvaluateTo<uint?, uint?>),
			IEnumerable<long> list => list.Max(lambda.EvaluateTo<long, long>),
			IEnumerable<long?> list => list.Max(lambda.EvaluateTo<long?, long?>),
			IEnumerable<ulong> list => list.Max(lambda.EvaluateTo<ulong, ulong>),
			IEnumerable<ulong?> list => list.Max(lambda.EvaluateTo<ulong?, ulong?>),
			IEnumerable<float> list => list.Max(lambda.EvaluateTo<float, float>),
			IEnumerable<float?> list => list.Max(lambda.EvaluateTo<float?, float?>),
			IEnumerable<double> list => list.Max(lambda.EvaluateTo<double, double>),
			IEnumerable<double?> list => list.Max(lambda.EvaluateTo<double?, double?>),
			IEnumerable<decimal> list => list.Max(lambda.EvaluateTo<decimal, decimal>),
			IEnumerable<decimal?> list => list.Max(lambda.EvaluateTo<decimal?, decimal?>),
			IEnumerable<string?> list => list.Max(lambda.EvaluateTo<string?, string?>),
			IEnumerable<object?> list => GetMax(list.Select(value => lambda.Evaluate(value))),
			_ => throw new FormatException($"First {ExtensionFunction.Max} parameter must be an IEnumerable of a string or numeric type when processing as a lambda.")
		};

	}

	private static IComparable GetMax(IEnumerable<object?> objectList)
	{
		IComparable? max = null;
		foreach (var item in objectList)
		{
			var thisOne = item switch
			{
				sbyte value => value,
				byte value => value,
				short value => value,
				ushort value => value,
				int value => value,
				uint value => value,
				long value => value,
				ulong value => value,
				float value => value,
				double value => value,
				decimal value => value,
				JValue jValue => jValue.Type switch
				{
					JTokenType.Float => jValue.Value<float>()!,
					JTokenType.Integer => (IComparable)jValue.Value<int>(),
					JTokenType.String => jValue.Value<string>()!,
					_ => throw new FormatException($"Found unsupported JToken type '{jValue.Type}' when completing max.")
				},
				string value => value,
				null => null,
				_ => throw new FormatException($"Found unsupported type '{item?.GetType().Name}' when completing max.")
			};
			if (thisOne != null && (max == null || thisOne.CompareTo(max) > 0))
			{
				max = thisOne;
			}
		}

		return max!;
	}
}
