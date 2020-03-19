using NCalc;
using System;
using System.Globalization;

namespace PanoramicData.NCalcExtensions.Extensions
{
	internal static class ToDateTime
	{
		internal static void Evaluate(FunctionArgs functionArgs)
		{
			var argument1 = functionArgs.Parameters.Length >= 1 ? functionArgs.Parameters[0]?.Evaluate() : null;
			var argument2 = functionArgs.Parameters.Length >= 2 ? functionArgs.Parameters[1]?.Evaluate() : null;
			var argument3 = functionArgs.Parameters.Length >= 3 ? functionArgs.Parameters[2]?.Evaluate() : null;

			functionArgs.Result = (argument1, argument2, argument3) switch
			{
				// Only one argument
				(_, null, null) => throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected between 2 and 3 arguments"),

				// String and format, no timezone
				(string dateTimeString, string format, null) => DateTime.TryParseExact(dateTimeString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime)
					? parsedDateTime
					: throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Input string did not match expected format."),

				// String, format and timezone
				(string dateTimeString, string format, string timeZoneName) => GetFromString(dateTimeString, format, timeZoneName),

				// DateTime, and timezone, no third argument
				(DateTime dateTime, string timeZoneName, null) => GetFromDateTime(dateTime, timeZoneName),

				// Null first parameter should return null
				(null, _, _) => null,

				// Non-string format
				(string _, _, _) => throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected second argument to be a string."),
				(DateTime _, _, _) => throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected second argument to be a string."),

				_ => throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Expected first argument to be a string or DateTime.")
			};
		}

		private static object GetFromDateTime(DateTime dateTime, string timeZoneName)
		{
			var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);
			var dateTimeOffset = new DateTimeOffset(dateTime, timeZoneInfo.GetUtcOffset(dateTime));
			return dateTimeOffset.UtcDateTime;
		}

		private static object GetFromString(string dateTimeString, string format, string timeZoneName)
		{
			if (!DateTime.TryParseExact(dateTimeString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
			{
				throw new ArgumentException($"{ExtensionFunction.ToDateTime} function - Input string did not match expected format.");
			}
			return ConvertTimeZone(parsedDateTime, timeZoneName, "UTC");
		}

		internal static object ConvertTimeZone(DateTime parsedDateTime, string sourceTimeZoneName, string destinationTimeZoneName)
		{
			var sourceTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZoneName);
			var destinationTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZoneName);
			return TimeZoneInfo.ConvertTime(parsedDateTime, sourceTimeZoneInfo, destinationTimeZoneInfo);
		}
	}
}