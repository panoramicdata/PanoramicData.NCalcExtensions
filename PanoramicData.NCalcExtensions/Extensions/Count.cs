using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Count
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var value = functionArgs.Parameters[0].Evaluate() as IList
			?? throw new FormatException($"{ExtensionFunction.Length}() requires IList parameter.");
		functionArgs.Result = value.Count;
	}
}
