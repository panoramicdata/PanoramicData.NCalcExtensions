using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// The shared implementation of max() and min(), which differ only in the direction of the
/// comparison.
/// </summary>
internal static class Extremum
{
	internal static void Evaluate(FunctionEventArgs functionArgs, string function, bool isMax)
	{
		var originalListUntyped = functionArgs.Parameters.Evaluate(0);

		if (originalListUntyped is null)
		{
			functionArgs.Result = null;
			return;
		}

		var originalList = originalListUntyped as IEnumerable
			?? throw new FormatException($"First {function} parameter must be an IEnumerable.");

		if (functionArgs.Parameters.Count == 1)
		{
			functionArgs.Result = ExtremumOf(originalList, null, function, isMax);
			return;
		}

		var lambda = Parameters.GetLambda(functionArgs, function);

		functionArgs.Result = ExtremumOf(originalList, lambda, function, isMax);
	}

	/// <summary>
	/// The extreme of the list's own values, or of <paramref name="lambda"/> applied to each of
	/// them when one is supplied.
	/// </summary>
	private static object? ExtremumOf(IEnumerable originalList, Lambda? lambda, string function, bool isMax)
		=> originalList switch
		{
			IEnumerable<sbyte> list => Pick(list, lambda, isMax),
			IEnumerable<sbyte?> list => Pick(list, lambda, isMax),
			IEnumerable<byte> list => Pick(list, lambda, isMax),
			IEnumerable<byte?> list => Pick(list, lambda, isMax),
			IEnumerable<short> list => Pick(list, lambda, isMax),
			IEnumerable<short?> list => Pick(list, lambda, isMax),
			IEnumerable<ushort> list => Pick(list, lambda, isMax),
			IEnumerable<ushort?> list => Pick(list, lambda, isMax),
			IEnumerable<int> list => Pick(list, lambda, isMax),
			IEnumerable<int?> list => Pick(list, lambda, isMax),
			IEnumerable<uint> list => Pick(list, lambda, isMax),
			IEnumerable<uint?> list => Pick(list, lambda, isMax),
			IEnumerable<long> list => Pick(list, lambda, isMax),
			IEnumerable<long?> list => Pick(list, lambda, isMax),
			IEnumerable<ulong> list => Pick(list, lambda, isMax),
			IEnumerable<ulong?> list => Pick(list, lambda, isMax),
			IEnumerable<float> list => Pick(list, lambda, isMax),
			IEnumerable<float?> list => Pick(list, lambda, isMax),
			IEnumerable<double> list => Pick(list, lambda, isMax),
			IEnumerable<double?> list => Pick(list, lambda, isMax),
			IEnumerable<decimal> list => Pick(list, lambda, isMax),
			IEnumerable<decimal?> list => Pick(list, lambda, isMax),
			IEnumerable<string?> list => Pick(list, lambda, isMax),
			// A list of boxed values that happen to all be strings is compared as strings.
			IEnumerable<object?> list when lambda is null && list.All(value => value is string or null)
				=> PickString(list, isMax),
			IEnumerable<object?> list
				=> ExtremeOfBoxed(lambda is null ? list : list.Select(lambda.Evaluate), function, isMax),
			_ => throw new FormatException(lambda is null
				? $"First {function} parameter must be an IEnumerable of a numeric or string type if only one parameter is present."
				: $"First {function} parameter must be an IEnumerable of a string or numeric type when processing as a lambda.")
		};

	/// <summary>
	/// The extreme of a strongly typed list, or of <paramref name="lambda"/> applied to each of
	/// its values when one is supplied.
	/// </summary>
	private static T? Pick<T>(IEnumerable<T> list, Lambda? lambda, bool isMax)
	{
		var values = lambda is null ? list : list.Select(lambda.EvaluateTo<T, T>);

		return isMax ? values.Max() : values.Min();
	}

	private static string? PickString(IEnumerable<object?> list, bool isMax)
	{
		var values = list.DefaultIfEmpty(null).Select(value => value as string);

		return isMax ? values.Max() : values.Min();
	}

	/// <summary>
	/// The extreme of a list of boxed values, which may mix numeric, string and JSON types.
	/// </summary>
	private static IComparable ExtremeOfBoxed(IEnumerable<object?> objectList, string function, bool isMax)
	{
		IComparable? extreme = null;
		foreach (var item in objectList)
		{
			var thisOne = AsComparable(item, function);
			if (thisOne is not null && (extreme is null || Beats(thisOne, extreme, isMax)))
			{
				extreme = thisOne;
			}
		}

		return extreme!;
	}

	private static bool Beats(IComparable candidate, IComparable incumbent, bool isMax)
		=> isMax ? candidate.CompareTo(incumbent) > 0 : candidate.CompareTo(incumbent) < 0;

	private static IComparable? AsComparable(object? item, string function) => item switch
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
			_ => throw new FormatException($"Found unsupported JToken type '{jValue.Type}' when completing {function}.")
		},
		string value => value,
		null => null,
		_ => throw new FormatException($"Found unsupported type '{item.GetType().Name}' when completing {function}.")
	};
}
