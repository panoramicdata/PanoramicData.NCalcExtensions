using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Skip
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var list = (IList)functionArgs.Parameters[0].Evaluate();
		var numberToSkip = (int)functionArgs.Parameters[1].Evaluate();
		functionArgs.Result = list.Cast<object?>().Skip(numberToSkip).ToList();
	}
}
