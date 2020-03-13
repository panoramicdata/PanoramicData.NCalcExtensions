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
				const int minToDateTimeParameterCount = 2;
				const int maxToDateTimeParameterCount = 3;
				var parameterIndex = -1;
				if (functionArgs.Parameters.Length < minToDateTimeParameterCount || functionArgs.Parameters.Length > maxToDateTimeParameterCount)
				{
					throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected between {minToDateTimeParameterCount} and {maxToDateTimeParameterCount} arguments");
				}
				if (!(functionArgs.Parameters[++parameterIndex].Evaluate() is string inputString))
				{
					throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected first argument to be a string.");
				}
				if (!(functionArgs.Parameters[++parameterIndex].Evaluate() is string formatString))
				{
					throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected second argument to be a string.");
				}

				if (!System.DateTime.TryParseExact(inputString, formatString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var outputDateTime))
				{
					throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Input string did not match expected format.");
				}

				if (functionArgs.Parameters.Length == 3)
				{
					if (!(functionArgs.Parameters[++parameterIndex].Evaluate() is string timeZoneString))
					{
						throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected second argument to be a string.");
					}
					var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneString);
					var dateTimeOffset = new DateTimeOffset(outputDateTime, timeZoneInfo.GetUtcOffset(outputDateTime));
					functionArgs.Result = dateTimeOffset.UtcDateTime;
				}
				else
				{
					functionArgs.Result = outputDateTime;
				}
			}
		}
	}
}