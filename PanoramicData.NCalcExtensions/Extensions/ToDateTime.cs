using NCalc;
using System;
using System.Globalization;

namespace PanoramicData.NCalcExtensions.Extensions
{
	internal static class ToDateTime
	{
		internal static void Evaluate(FunctionArgs functionArgs)
		{
			{
				const int toDateTimeParameterCount = 2;
				if (functionArgs.Parameters.Length != toDateTimeParameterCount)
				{
					throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected {toDateTimeParameterCount} arguments");
				}
				if (!(functionArgs.Parameters[0].Evaluate() is string inputString))
				{
					throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected first argument to be a string.");
				}
				if (!(functionArgs.Parameters[1].Evaluate() is string formatString))
				{
					throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected second argument to be a string.");
				}
				if (!System.DateTime.TryParseExact(inputString, formatString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var outputDateTime))
				{
					throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Input string did not match expected format.");
				}
				functionArgs.Result = outputDateTime;
				return;
			}
		}
	}
}