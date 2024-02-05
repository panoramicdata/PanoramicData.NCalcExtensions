using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class LastOrDefault
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var enumerable = functionArgs.Parameters[0].Evaluate() as IList
			?? throw new FormatException($"First {ExtensionFunction.LastOrDefault} parameter must be an IEnumerable.");

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.LastOrDefault} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.LastOrDefault} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);

		var reversedEnumerable = enumerable.Cast<object?>().Reverse();

		foreach (var value in reversedEnumerable)
		{
			if (lambda.Evaluate(value) as bool? == true)
			{
				functionArgs.Result = value;
				return;
			}
		}

		functionArgs.Result = null;
	}
}

