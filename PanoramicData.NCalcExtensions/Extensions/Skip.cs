using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Skip
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var list = (List<object?>)functionArgs.Parameters[0].Evaluate();
		var numberToSkip = (int)functionArgs.Parameters[1].Evaluate();
		functionArgs.Result = list.Skip(numberToSkip).ToList();
	}
}
