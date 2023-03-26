using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Count
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var list = functionArgs.Parameters[0].Evaluate() as IEnumerable<object?>
			?? throw new FormatException($"{ExtensionFunction.Count}() requires IEnumerable parameter.");

		if (functionArgs.Parameters.Length == 1)
		{
			functionArgs.Result = list.Count();
			return;
		}

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.Count} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.Count} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, new());

		functionArgs.Result = list
			.Count(value =>
			{
				var result = lambda.Evaluate(value) as bool?;
				return result == true;
			});
	}
}