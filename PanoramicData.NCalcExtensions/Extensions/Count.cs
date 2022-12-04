using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Count
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var value = functionArgs.Parameters[0].Evaluate() as IEnumerable
			?? throw new FormatException($"{ExtensionFunction.Count}() requires IList parameter.");
		functionArgs.Result = value.Cast<object>().Count();
	}
}
