﻿using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class First
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var enumerable = functionArgs.Parameters[0].Evaluate() as IList
			?? throw new FormatException($"First {ExtensionFunction.First} parameter must be an IEnumerable.");

		// If there is only 1 parameter, return the first element of the enumerable
		if (functionArgs.Parameters.Length == 1)
		{
			functionArgs.Result = enumerable[0];
			return;
		}

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.First} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.First} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);

		foreach (var value in enumerable)
		{
			if (lambda.Evaluate(value) as bool? != true)
			{
				continue;
			}

			functionArgs.Result = value;
			return;
		}

		throw new FormatException($"No matching element found.");
	}
}
