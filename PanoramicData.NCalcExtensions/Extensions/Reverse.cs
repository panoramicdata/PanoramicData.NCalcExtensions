using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Reverse
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var enumerable = functionArgs.Parameters[0].Evaluate() as IList
			?? throw new FormatException($"First {ExtensionFunction.Reverse} parameter must be an IEnumerable.");

		functionArgs.Result = enumerable.Cast<object?>().Reverse().ToList();
	}
}
