using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Distinct
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var enumerable = functionArgs.Parameters[0].Evaluate() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.Distinct} parameter must be an IEnumerable.");

		functionArgs.Result = enumerable
			.Distinct()
			.ToList();
	}
}
