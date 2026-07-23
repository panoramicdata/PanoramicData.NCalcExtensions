using System.Runtime.CompilerServices;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Parameters
{
	#region Parameters

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
			(T1?)args.Parameters.Evaluate(0) ?? throw new FormatException($"{callerName} first argument should be a {typeof(T1).Name}."),
			(T2?)args.Parameters.Evaluate(1) ?? throw new FormatException($"{callerName} second argument should be a {typeof(T2).Name}.")
			);
	}

	internal static Tuple<T1, T2, T3> GetParameters<T1, T2, T3>(
		FunctionEventArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(3, 3, args.Parameters.Count, callerName);

		return new Tuple<T1, T2, T3>(
			(T1?)args.Parameters.Evaluate(0) ?? throw new FormatException($"{callerName} first argument should be a {typeof(T1).Name}."),
			(T2?)args.Parameters.Evaluate(1) ?? throw new FormatException($"{callerName} second argument should be a {typeof(T2).Name}."),
			(T3?)args.Parameters.Evaluate(2) ?? throw new FormatException($"{callerName} third argument should be a {typeof(T3).Name}.")
			);
	}

	internal static Tuple<T1, T2, T3, T4> GetParameters<T1, T2, T3, T4>(
		FunctionEventArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(4, 4, args.Parameters.Count, callerName);

		return new Tuple<T1, T2, T3, T4>(
			(T1?)args.Parameters.Evaluate(0) ?? throw new FormatException($"{callerName} first argument should be a {typeof(T1).Name}."),
			(T2?)args.Parameters.Evaluate(1) ?? throw new FormatException($"{callerName} second argument should be a {typeof(T2).Name}."),
			(T3?)args.Parameters.Evaluate(2) ?? throw new FormatException($"{callerName} third argument should be a {typeof(T3).Name}."),
			(T4?)args.Parameters.Evaluate(3) ?? throw new FormatException($"{callerName} fourth argument should be a {typeof(T4).Name}.")
			);
	}

	internal static Tuple<T1, T2, T3, T4, T5> GetParameters<T1, T2, T3, T4, T5>(
		FunctionEventArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(5, 5, args.Parameters.Count, callerName);

		return new Tuple<T1, T2, T3, T4, T5>(
			(T1?)args.Parameters.Evaluate(0) ?? throw new FormatException($"{callerName} first argument should be a {typeof(T1).Name}."),
			(T2?)args.Parameters.Evaluate(1) ?? throw new FormatException($"{callerName} second argument should be a {typeof(T2).Name}."),
			(T3?)args.Parameters.Evaluate(2) ?? throw new FormatException($"{callerName} third argument should be a {typeof(T3).Name}."),
			(T4?)args.Parameters.Evaluate(3) ?? throw new FormatException($"{callerName} fourth argument should be a {typeof(T4).Name}."),
			(T5?)args.Parameters.Evaluate(4) ?? throw new FormatException($"{callerName} fifth argument should be a {typeof(T5).Name}.")
			);
	}

	internal static Tuple<T1, T2, T3, T4, T5, T6> GetParameters<T1, T2, T3, T4, T5, T6>(
		FunctionEventArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(6, 6, args.Parameters.Count, callerName);

		return new Tuple<T1, T2, T3, T4, T5, T6>(
			(T1?)args.Parameters.Evaluate(0) ?? throw new FormatException($"{callerName} first argument should be a {typeof(T1).Name}."),
			(T2?)args.Parameters.Evaluate(1) ?? throw new FormatException($"{callerName} second argument should be a {typeof(T2).Name}."),
			(T3?)args.Parameters.Evaluate(2) ?? throw new FormatException($"{callerName} third argument should be a {typeof(T3).Name}."),
			(T4?)args.Parameters.Evaluate(3) ?? throw new FormatException($"{callerName} fourth argument should be a {typeof(T4).Name}."),
			(T5?)args.Parameters.Evaluate(4) ?? throw new FormatException($"{callerName} fifth argument should be a {typeof(T5).Name}."),
			(T6?)args.Parameters.Evaluate(5) ?? throw new FormatException($"{callerName} sixth argument should be a {typeof(T6).Name}.")
			);
	}

	internal static Tuple<T1, T2, T3, T4, T5, T6, T7> GetParameters<T1, T2, T3, T4, T5, T6, T7>(
		FunctionEventArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(7, 7, args.Parameters.Count, callerName);

		return new Tuple<T1, T2, T3, T4, T5, T6, T7>(
			(T1?)args.Parameters.Evaluate(0) ?? throw new FormatException($"{callerName} first argument should be a {typeof(T1).Name}."),
			(T2?)args.Parameters.Evaluate(1) ?? throw new FormatException($"{callerName} second argument should be a {typeof(T2).Name}."),
			(T3?)args.Parameters.Evaluate(2) ?? throw new FormatException($"{callerName} third argument should be a {typeof(T3).Name}."),
			(T4?)args.Parameters.Evaluate(3) ?? throw new FormatException($"{callerName} fourth argument should be a {typeof(T4).Name}."),
			(T5?)args.Parameters.Evaluate(4) ?? throw new FormatException($"{callerName} fifth argument should be a {typeof(T5).Name}."),
			(T6?)args.Parameters.Evaluate(5) ?? throw new FormatException($"{callerName} sixth argument should be a {typeof(T6).Name}."),
			(T7?)args.Parameters.Evaluate(6) ?? throw new FormatException($"{callerName} seventh argument should be a {typeof(T7).Name}.")
			);
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