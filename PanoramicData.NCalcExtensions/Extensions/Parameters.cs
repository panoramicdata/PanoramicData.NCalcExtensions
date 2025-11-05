using System.Runtime.CompilerServices;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Parameters
{

	#region Parameters

	internal static T1 GetParameter<T1>(
		FunctionArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(1, 1, args.Parameters, callerName);

		return (T1?)args.Parameters[0].Evaluate() ?? throw new FormatException($"{callerName} argument should be a {typeof(T1).Name}.");
	}

	internal static Tuple<T1, T2> GetParameters<T1, T2>(
		FunctionArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(2, 2, args.Parameters, callerName);

		return new Tuple<T1, T2>(
			(T1?)args.Parameters[0].Evaluate() ?? throw new FormatException($"{callerName} first argument should be a {typeof(T1).Name}."),
			(T2?)args.Parameters[1].Evaluate() ?? throw new FormatException($"{callerName} second argument should be a {typeof(T2).Name}.")
			);
	}

	internal static Tuple<T1, T2, T3> GetParameters<T1, T2, T3>(
		FunctionArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(3, 3, args.Parameters, callerName);

		return new Tuple<T1, T2, T3>(
			(T1?)args.Parameters[0].Evaluate() ?? throw new FormatException($"{callerName} first argument should be a {typeof(T1).Name}."),
			(T2?)args.Parameters[1].Evaluate() ?? throw new FormatException($"{callerName} second argument should be a {typeof(T2).Name}."),
			(T3?)args.Parameters[2].Evaluate() ?? throw new FormatException($"{callerName} third argument should be a {typeof(T3).Name}.")
			);
	}

	internal static Tuple<T1, T2, T3, T4> GetParameters<T1, T2, T3, T4>(
		FunctionArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(4, 4, args.Parameters, callerName);

		return new Tuple<T1, T2, T3, T4>(
			(T1?)args.Parameters[0].Evaluate() ?? throw new FormatException($"{callerName} first argument should be a {typeof(T1).Name}."),
			(T2?)args.Parameters[1].Evaluate() ?? throw new FormatException($"{callerName} second argument should be a {typeof(T2).Name}."),
			(T3?)args.Parameters[2].Evaluate() ?? throw new FormatException($"{callerName} third argument should be a {typeof(T3).Name}."),
			(T4?)args.Parameters[3].Evaluate() ?? throw new FormatException($"{callerName} fourth argument should be a {typeof(T4).Name}.")
			);
	}

	internal static Tuple<T1, T2, T3, T4, T5> GetParameters<T1, T2, T3, T4, T5>(
		FunctionArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(5, 5, args.Parameters, callerName);

		return new Tuple<T1, T2, T3, T4, T5>(
			(T1?)args.Parameters[0].Evaluate() ?? throw new FormatException($"{callerName} first argument should be a {typeof(T1).Name}."),
			(T2?)args.Parameters[1].Evaluate() ?? throw new FormatException($"{callerName} second argument should be a {typeof(T2).Name}."),
			(T3?)args.Parameters[2].Evaluate() ?? throw new FormatException($"{callerName} third argument should be a {typeof(T3).Name}."),
			(T4?)args.Parameters[3].Evaluate() ?? throw new FormatException($"{callerName} fourth argument should be a {typeof(T4).Name}."),
			(T5?)args.Parameters[4].Evaluate() ?? throw new FormatException($"{callerName} fifth argument should be a {typeof(T5).Name}.")
			);
	}

	internal static Tuple<T1, T2, T3, T4, T5, T6> GetParameters<T1, T2, T3, T4, T5, T6>(
		FunctionArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(6, 6, args.Parameters, callerName);

		return new Tuple<T1, T2, T3, T4, T5, T6>(
			(T1?)args.Parameters[0].Evaluate() ?? throw new FormatException($"{callerName} first argument should be a {typeof(T1).Name}."),
			(T2?)args.Parameters[1].Evaluate() ?? throw new FormatException($"{callerName} second argument should be a {typeof(T2).Name}."),
			(T3?)args.Parameters[2].Evaluate() ?? throw new FormatException($"{callerName} third argument should be a {typeof(T3).Name}."),
			(T4?)args.Parameters[3].Evaluate() ?? throw new FormatException($"{callerName} fourth argument should be a {typeof(T4).Name}."),
			(T5?)args.Parameters[4].Evaluate() ?? throw new FormatException($"{callerName} fifth argument should be a {typeof(T5).Name}."),
			(T6?)args.Parameters[5].Evaluate() ?? throw new FormatException($"{callerName} sixth argument should be a {typeof(T6).Name}.")
			);
	}

	internal static Tuple<T1, T2, T3, T4, T5, T6, T7> GetParameters<T1, T2, T3, T4, T5, T6, T7>(
		FunctionArgs args,
		[CallerMemberName] string callerName = "")
	{
		CheckParameterCount(7, 7, args.Parameters, callerName);

		return new Tuple<T1, T2, T3, T4, T5, T6, T7>(
			(T1?)args.Parameters[0].Evaluate() ?? throw new FormatException($"{callerName} first argument should be a {typeof(T1).Name}."),
			(T2?)args.Parameters[1].Evaluate() ?? throw new FormatException($"{callerName} second argument should be a {typeof(T2).Name}."),
			(T3?)args.Parameters[2].Evaluate() ?? throw new FormatException($"{callerName} third argument should be a {typeof(T3).Name}."),
			(T4?)args.Parameters[3].Evaluate() ?? throw new FormatException($"{callerName} fourth argument should be a {typeof(T4).Name}."),
			(T5?)args.Parameters[4].Evaluate() ?? throw new FormatException($"{callerName} fifth argument should be a {typeof(T5).Name}."),
			(T6?)args.Parameters[5].Evaluate() ?? throw new FormatException($"{callerName} sixth argument should be a {typeof(T6).Name}."),
			(T7?)args.Parameters[6].Evaluate() ?? throw new FormatException($"{callerName} seventh argument should be a {typeof(T7).Name}.")
			);
	}

	internal static void CheckParameterCount(
		int? minCount,
		int? maxCount,
		Expression[] parameters,
		[CallerMemberName] string callerName = "")
	{
		if (minCount.HasValue && parameters.Length < minCount.Value)
		{
			throw new FormatException($"{callerName} requires at least {minCount} parameters.");
		}

		if (maxCount.HasValue && parameters.Length > maxCount.Value)
		{
			throw new FormatException($"{callerName} requires at most {maxCount} parameters.");
		}
	}
	#endregion
}