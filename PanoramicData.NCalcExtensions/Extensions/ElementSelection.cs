using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// The shared implementation of first(), firstOrDefault(), last() and lastOrDefault().
/// </summary>
/// <remarks>
/// The four functions differ only in which end of the list they take from and what they do when
/// there is no element to take, so they are one implementation with those two choices as
/// parameters rather than four near-identical copies.
/// </remarks>
internal static class ElementSelection
{
	/// <param name="functionName">The calling function's name, for failure messages.</param>
	/// <param name="fromEnd">Whether to search from the end of the list rather than the start.</param>
	/// <param name="allowMissing">
	/// Whether a missing element yields null. When false, the single-parameter form indexes the
	/// list directly — so an empty list throws from the indexer — and the lambda form throws a
	/// FormatException if nothing matches.
	/// </param>
	internal static void Evaluate(
		FunctionEventArgs functionArgs,
		string functionName,
		bool fromEnd,
		bool allowMissing)
	{
		var enumerable = functionArgs.Parameters.Evaluate(0) as IList
			?? throw new FormatException($"First {functionName} parameter must be an IEnumerable.");

		// With only one parameter, take the element from the relevant end of the list.
		if (functionArgs.Parameters.Count == 1)
		{
			functionArgs.Result = allowMissing && enumerable.Count == 0
				? null
				: JValueHelper.UnwrapJValue(enumerable[fromEnd ? enumerable.Count - 1 : 0]);
			return;
		}

		var predicate = functionArgs.Parameters.Evaluate(1) as string
			?? throw new FormatException($"Second {functionName} parameter must be a string.");

		var lambdaString = functionArgs.Parameters.Evaluate(2) as string
			?? throw new FormatException($"Third {functionName} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Context.StaticParameters);

		var values = enumerable.Cast<object?>();
		if (fromEnd)
		{
			values = values.Reverse();
		}

		foreach (var value in values)
		{
			if (lambda.Evaluate(value) as bool? == true)
			{
				functionArgs.Result = JValueHelper.UnwrapJValue(value);
				return;
			}
		}

		if (!allowMissing)
		{
			throw new FormatException("No matching element found.");
		}

		functionArgs.Result = null;
	}
}
