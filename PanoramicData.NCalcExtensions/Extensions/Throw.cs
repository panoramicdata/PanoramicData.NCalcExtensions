using NCalc;
using PanoramicData.NCalcExtensions.Exceptions;
using System;

namespace PanoramicData.NCalcExtensions.Extensions
{
	internal static class Throw
	{
		internal static Exception Evaluate(FunctionArgs functionArgs)
		{
			switch (functionArgs.Parameters.Length)
			{
				case 0:
					return new NCalcExtensionsException();
				case 1:
					if (!(functionArgs.Parameters[0].Evaluate() is string exceptionMessageText))
					{
						return new FormatException($"{ExtensionFunction.Throw} function - parameter must be a string.");
					}
					return new NCalcExtensionsException(exceptionMessageText);

				default:
					return new FormatException($"{ExtensionFunction.Throw} function - takes zero or one parameters.");
			}
		}
	}
}