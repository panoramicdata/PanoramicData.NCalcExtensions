using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Select
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var enumerable = functionArgs.Parameters[0].Evaluate() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.Where} parameter must be an IEnumerable.");

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException("Second {ExtensionFunction.Where} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			?? throw new FormatException("Second {ExtensionFunction.Where} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, new());

		functionArgs.Result = enumerable
			.Select(value =>
			{
				var result = lambda.Evaluate(value);
				return result;
			}).ToList();
	}
}
