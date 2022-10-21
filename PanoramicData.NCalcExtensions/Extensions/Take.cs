using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Take
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var list = (List<object?>)functionArgs.Parameters[0].Evaluate();
		var numberToTake = (int)functionArgs.Parameters[1].Evaluate();
		functionArgs.Result = list.Take(numberToTake).ToList();
	}
}
