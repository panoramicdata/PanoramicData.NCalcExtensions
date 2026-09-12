using System.Runtime.CompilerServices;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Parameters
{
	#region Parameters

	/// <summary>
	/// The argument names used in failure messages, indexed by argument position.
	/// </summary>
	private static readonly string[] ArgumentOrdinals =
		["first", "second", "third", "fourth", "fifth", "sixth", "seventh"];

	/// <summary>
	/// Evaluates the argument at <paramref name="index"/>, requiring it to be a
	/// <typeparamref name="T"/>.
	/// </summary>
	private static T Argument<T>(FunctionEventArgs args, int index, string callerName)
		=> (T?)args.Parameters.Evaluate(index)
			?? throw new FormatException($"{callerName} {ArgumentOrdinals[index]} argument should be a {typeof(T).Name}.");

	internal static T1 GetParameter<T1>(
		FunctionEventArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(1, 1, args.Parameters.Count, callerName);

		return (T1?)args.Parameters.Evaluate(0) ?? throw new FormatException($"{callerName} argument should be a {typeof(T1).Name}.");
	}

	internal static Tuple<T1, T2> GetParameters<T1, T2>(
		FunctionEventArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(2, 2, args.Parameters.Count, callerName);

		return new Tuple<T1, T2>(
			Argument<T1>(args, 0, callerName),
			Argument<T2>(args, 1, callerName));
	}

	internal static Tuple<T1, T2, T3> GetParameters<T1, T2, T3>(
		FunctionEventArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(3, 3, args.Parameters.Count, callerName);

		return new Tuple<T1, T2, T3>(
			Argument<T1>(args, 0, callerName),
			Argument<T2>(args, 1, callerName),
			Argument<T3>(args, 2, callerName));
	}

	internal static Tuple<T1, T2, T3, T4> GetParameters<T1, T2, T3, T4>(
		FunctionEventArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(4, 4, args.Parameters.Count, callerName);

		return new Tuple<T1, T2, T3, T4>(
			Argument<T1>(args, 0, callerName),
			Argument<T2>(args, 1, callerName),
			Argument<T3>(args, 2, callerName),
			Argument<T4>(args, 3, callerName));
	}

	internal static Tuple<T1, T2, T3, T4, T5> GetParameters<T1, T2, T3, T4, T5>(
		FunctionEventArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(5, 5, args.Parameters.Count, callerName);

		return new Tuple<T1, T2, T3, T4, T5>(
			Argument<T1>(args, 0, callerName),
			Argument<T2>(args, 1, callerName),
			Argument<T3>(args, 2, callerName),
			Argument<T4>(args, 3, callerName),
			Argument<T5>(args, 4, callerName));
	}

	internal static Tuple<T1, T2, T3, T4, T5, T6> GetParameters<T1, T2, T3, T4, T5, T6>(
		FunctionEventArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(6, 6, args.Parameters.Count, callerName);

		return new Tuple<T1, T2, T3, T4, T5, T6>(
			Argument<T1>(args, 0, callerName),
			Argument<T2>(args, 1, callerName),
			Argument<T3>(args, 2, callerName),
			Argument<T4>(args, 3, callerName),
			Argument<T5>(args, 4, callerName),
			Argument<T6>(args, 5, callerName));
	}

	internal static Tuple<T1, T2, T3, T4, T5, T6, T7> GetParameters<T1, T2, T3, T4, T5, T6, T7>(
		FunctionEventArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(7, 7, args.Parameters.Count, callerName);

		return new Tuple<T1, T2, T3, T4, T5, T6, T7>(
			Argument<T1>(args, 0, callerName),
			Argument<T2>(args, 1, callerName),
			Argument<T3>(args, 2, callerName),
			Argument<T4>(args, 3, callerName),
			Argument<T5>(args, 4, callerName),
			Argument<T6>(args, 5, callerName),
			Argument<T7>(args, 6, callerName));
	}

	/// <summary>
	/// Builds the lambda from a function's predicate and expression arguments, which by
	/// convention are its second and third.
	/// </summary>
	internal static Lambda GetLambda(FunctionEventArgs args, string function)
	{
		var predicate = args.Parameters.Evaluate(1) as string
			?? throw new FormatException($"Second {function} parameter must be a string.");

		var lambdaString = args.Parameters.Evaluate(2) as string
			?? throw new FormatException($"Third {function} parameter must be a string.");

		return new Lambda(predicate, lambdaString, args.Context.StaticParameters);
	}

	internal static void CheckParameterCount(
		int? minCount,
		int? maxCount,
		int actualCount,
		[CallerMemberName] string callerName = "")
	{
		if (minCount.HasValue && actualCount < minCount.Value)
		{
			throw new FormatException($"{callerName} requires at least {minCount} parameters.");
		}

		if (maxCount.HasValue && actualCount > maxCount.Value)
		{
			throw new FormatException($"{callerName} requires at most {maxCount} parameters.");
		}
	}
	#endregion
}