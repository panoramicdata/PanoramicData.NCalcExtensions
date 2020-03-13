using NCalc;
using System;
using System.Globalization;

namespace PanoramicData.NCalcExtensions.Extensions
{
	internal static class Format
	{
		internal static void Evaluate(FunctionArgs functionArgs)
		{
			const int min = 2;
			const int max = 3;
			if (functionArgs.Parameters.Length < min || functionArgs.Parameters.Length > max)
			{
				throw new ArgumentException($"{ExtensionFunction.Format} function - expected between {min} and {max} arguments");
			}

			var inputObject = functionArgs.Parameters[0].Evaluate();
			if (!(functionArgs.Parameters[1].Evaluate() is string formatFormat))
			{
				throw new ArgumentException($"{ExtensionFunction.Format} function - expected second argument to be a format string");
			}

			functionArgs.Result = (inputObject, functionArgs.Parameters.Length) switch
			{
				(int inputInt, 2) => inputInt.ToString(formatFormat, CultureInfo.InvariantCulture),
				(double inputDouble, 2) => inputDouble.ToString(formatFormat, CultureInfo.InvariantCulture),
				(DateTime dateTime, 2) => dateTime.ToString(formatFormat, CultureInfo.InvariantCulture),
				(DateTime dateTime, 3) => ToDateTimeInTargetTimeZone(dateTime, formatFormat, functionArgs.Parameters[2].Evaluate() as string ?? throw new ArgumentException($"{ExtensionFunction.Format} function - expected third argument to be a TimeZone string")),
				(string inputString, 2) => GetThing(inputString, formatFormat),
				_ => throw new NotSupportedException($"Unsupported input type {inputObject.GetType().Name} or incorrect number of parameters.")
			};
		}

		private static string ToDateTimeInTargetTimeZone(DateTime dateTime, string formatFormat, string timeZoneString)
		{
			var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneString);
			var dateTimeOffset = new DateTimeOffset(dateTime, -timeZoneInfo.GetUtcOffset(dateTime));
			return dateTimeOffset.UtcDateTime.ToString(formatFormat, CultureInfo.InvariantCulture);
		}

		private static object GetThing(string inputString, string formatFormat)
		{
			// Assume this is a number
			if (long.TryParse(inputString, out var longValue))
			{
				return longValue.ToString(formatFormat, CultureInfo.InvariantCulture);
			}
			if (double.TryParse(inputString, out var doubleValue))
			{
				return doubleValue.ToString(formatFormat, CultureInfo.InvariantCulture);
			}
			if (DateTimeOffset.TryParse(
				inputString,
				CultureInfo.InvariantCulture.DateTimeFormat,
				DateTimeStyles.AssumeUniversal,
				out var dateTimeOffsetValue))
			{
				return dateTimeOffsetValue.ToString(formatFormat, CultureInfo.InvariantCulture);
			}
			throw new FormatException($"Could not parse '{inputString}' as a number or date.");
		}
	}
}