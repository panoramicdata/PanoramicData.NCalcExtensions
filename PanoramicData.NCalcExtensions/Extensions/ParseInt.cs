using NCalc;
using System;

namespace PanoramicData.NCalcExtensions.Extensions
{
	internal static class ParseInt
	{
		internal static void Evaluate(FunctionArgs functionArgs)
		{
			if (functionArgs.Parameters.Length != 1)
			{
				throw new FormatException($"{ExtensionFunction.ParseInt} function - requires one string parameter.");
			}
			var param1 = functionArgs.Parameters[0].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.ParseInt} function - requires one string parameter.");
			if (!int.TryParse(param1, out var result))
			{
				throw new FormatException($"{ExtensionFunction.ParseInt} function - parameter could not be parsed to an integer.");
			}
			functionArgs.Result = result;
		}
	}
}