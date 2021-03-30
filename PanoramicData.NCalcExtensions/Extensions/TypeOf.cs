using NCalc;
using System;

namespace PanoramicData.NCalcExtensions.Extensions
{
	internal static class TypeOf
	{
		internal static void Evaluate(FunctionArgs functionArgs)
		{
			var parameter1 = functionArgs.Parameters.Length == 1
				? functionArgs.Parameters[0].Evaluate()
				: throw new FormatException($"{ExtensionFunction.TypeOf} function -  requires one parameter.");

			functionArgs.Result = parameter1 switch
			{
				null => null,
				object @object => @object.GetType().Name
			};
		}
	}
}