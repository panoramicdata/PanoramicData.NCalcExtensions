using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Any
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		switch (functionArgs.Parameters.Length)
		{
			case 0:
				throw new FormatException($"At least one parameter must be provided for {ExtensionFunction.Any}.");
			case 1:
				var list = functionArgs.Parameters[0].Evaluate() as IEnumerable<object?>
					?? throw new FormatException($"First {ExtensionFunction.Any} parameter must be an IEnumerable.");

				functionArgs.Result = list.Any();
				return;
			case 3:
				list = functionArgs.Parameters[0].Evaluate() as IEnumerable<object?>
					?? throw new FormatException($"First {ExtensionFunction.Any} parameter must be an IEnumerable.");

				var predicate = functionArgs.Parameters[1].Evaluate() as string
					?? throw new FormatException($"Second {ExtensionFunction.Any} parameter must be a string.");

				var lambdaString = functionArgs.Parameters[2].Evaluate() as string
					?? throw new FormatException($"Third {ExtensionFunction.Any} parameter must be a string.");

				var lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);

				functionArgs.Result = list
					.Any(value =>
					{
						var result = lambda.Evaluate(value) as bool?;
						return result == true;
					});
				return;
			default:
				throw new FormatException($"{ExtensionFunction.Any} takes either 1 or 3 parameters.");
		}
	}
}
