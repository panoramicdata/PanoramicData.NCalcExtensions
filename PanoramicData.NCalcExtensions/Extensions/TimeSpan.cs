using NCalc;
using PanoramicData.NCalcExtensions.Exceptions;
using System;

namespace PanoramicData.NCalcExtensions.Extensions
{
	internal static class TimeSpan
	{
		internal static void Evaluate(FunctionArgs functionArgs)
		{
			if (functionArgs.Parameters.Length != 3)
			{
				throw new FormatException($"{ExtensionFunction.TimeSpan} function - requires three parameters.");
			}
			string fromString;
			string toString;
			string timeUnitString;
			try
			{
				fromString = functionArgs.Parameters[0].Evaluate().ToString();
				toString = functionArgs.Parameters[1].Evaluate().ToString();
				timeUnitString = functionArgs.Parameters[2].Evaluate().ToString();
			}
			catch (NCalcExtensionsException)
			{
				throw;
			}
			catch (Exception e)
			{
				throw new FormatException($"{ExtensionFunction.TimeSpan} function -  could not extract three parameters into strings: {e.Message}");
			}

			if (!System.DateTime.TryParse(fromString, out var fromDateTime))
			{
				throw new FormatException($"{ExtensionFunction.TimeSpan} function -  could not convert '{fromString}' to DateTime");
			}
			if (!System.DateTime.TryParse(toString, out var toDateTime))
			{
				throw new FormatException($"{ExtensionFunction.TimeSpan} function -  could not convert '{toString}' to DateTime");
			}
			if (!Enum.TryParse(timeUnitString, true, out TimeUnit timeUnit))
			{
				throw new FormatException($"{ExtensionFunction.TimeSpan} function -  could not convert '{timeUnitString}' to a supported time unit");
			}

			// Determine the timespan
			var timeSpan = toDateTime - fromDateTime;
			functionArgs.Result = GetUnits(timeSpan, timeUnit);
		}

		private static double GetUnits(System.TimeSpan timeSpan, TimeUnit timeUnit)
			=> timeUnit switch
			{
				TimeUnit.Milliseconds => timeSpan.TotalMilliseconds,
				TimeUnit.Seconds => timeSpan.TotalSeconds,
				TimeUnit.Minutes => timeSpan.TotalMinutes,
				TimeUnit.Hours => timeSpan.TotalHours,
				TimeUnit.Days => timeSpan.TotalDays,
				TimeUnit.Weeks => timeSpan.TotalDays / 7,
				TimeUnit.Years => timeSpan.TotalDays / 365.25,
				_ => throw new ArgumentOutOfRangeException($"Time unit not supported: '{timeUnit}'"),
			};
	}
}