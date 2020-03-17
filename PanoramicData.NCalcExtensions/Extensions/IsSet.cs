using NCalc;
using System;
using System.Linq;

namespace PanoramicData.NCalcExtensions.Extensions
{

	internal static class IsSet
	{
		internal static void Evaluate(FunctionArgs functionArgs)
		{
			if (functionArgs.Parameters.Length != 1)
			{
				throw new FormatException($"{ExtensionFunction.IsSet}() requires one parameter.");
			}
			functionArgs.Result = functionArgs.Parameters[0].Parameters.Keys.Any(p => p == functionArgs.Parameters[0].Evaluate() as string);
		}
	}
}