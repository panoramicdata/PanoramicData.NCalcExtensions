using System;
using System.Globalization;

namespace PanoramicData.NCalcExtensions.Extensions
{
	internal static class DateTimeAsEpochMs
	{
		internal static void Evaluate(NCalc.FunctionArgs functionArgs)
		{
			var dateTimeOffset = DateTimeOffset.ParseExact(
				functionArgs.Parameters[0].Evaluate() as string, // Input date as string
				functionArgs.Parameters[1].Evaluate() as string,
				CultureInfo.InvariantCulture.DateTimeFormat,
				DateTimeStyles.AssumeUniversal);
			functionArgs.Result = dateTimeOffset.ToUnixTimeMilliseconds();
		}
	}
}