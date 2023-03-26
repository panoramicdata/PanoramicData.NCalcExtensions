using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class OrderBy
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var parameterIndex = 0;
		var list = functionArgs.Parameters[parameterIndex++].Evaluate() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.Where} parameter must be an IEnumerable.");

		var predicate = functionArgs.Parameters[parameterIndex++].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.Where} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[parameterIndex++].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.Where} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, new());

		IOrderedEnumerable<object?> orderable = list
			.OrderBy(value =>
			{
				var result = lambda.Evaluate(value);
				return result;
			});

		var parameterCount = functionArgs.Parameters.Length;

		while (parameterIndex < parameterCount)
		{
			lambdaString = functionArgs.Parameters[parameterIndex++].Evaluate() as string
				?? throw new FormatException($"Third {ExtensionFunction.Where} parameter must be a string.");
			lambda = new Lambda(predicate, lambdaString, new());
			orderable = orderable
						.ThenBy(value =>
						{
							var result = lambda.Evaluate(value);
							return result;
						});
		}

		functionArgs.Result = orderable.ToList();
	}
}
