using System.Collections;
using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Select
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var enumerable = functionArgs.Parameters[0].Evaluate() as IList
			?? throw new FormatException($"First {ExtensionFunction.Select} parameter must be an IEnumerable.");

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.Select} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.Select} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);

		var result = new List<object?>();
		foreach (var value in enumerable)
		{
			result.Add(lambda.Evaluate(value));
		}

		functionArgs.Result = result;
	}
}
