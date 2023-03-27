using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Take
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var list = (IList)functionArgs.Parameters[0].Evaluate();
		var numberToTake = (int)functionArgs.Parameters[1].Evaluate();
		functionArgs.Result = list.Cast<object?>().Take(numberToTake).ToList();
	}
}
