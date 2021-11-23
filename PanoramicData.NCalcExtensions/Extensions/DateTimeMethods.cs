namespace PanoramicData.NCalcExtensions.Extensions;

internal static class DateTimeMethods
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length > 0)
		{
			// Time Zone
			if (functionArgs.Parameters[0].Evaluate() is not string timeZone)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - The first argument should be a string, e.g. 'UTC'");
			}
			// TODO - support more than just UTC
			if (timeZone != "UTC")
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Only UTC timeZone is currently supported.");
			}
		}
		// Time zone has been determined

		// Format
		var format = functionArgs.Parameters.Length > 1
			? functionArgs.Parameters[1].Evaluate() as string
			: "YYYY-MM-dd HH:mm:ss";
		// Format has been determined

		// Days to add
		double daysToAdd = 0;
		if (functionArgs.Parameters.Length > 2)
		{
			var daysToAddNullable = GetNullableDouble(functionArgs.Parameters[2]);
			if (!daysToAddNullable.HasValue)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Days to add must be a number.");
			}
			daysToAdd = daysToAddNullable.Value;
		}

		// Hours to add
		double hoursToAdd = 0;
		if (functionArgs.Parameters.Length > 3)
		{
			var hoursToAddNullable = GetNullableDouble(functionArgs.Parameters[3]);
			if (!hoursToAddNullable.HasValue)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Hours to add must be a number.");
			}
			hoursToAdd = hoursToAddNullable.Value;
		}

		// Minutes to add
		double minutesToAdd = 0;
		if (functionArgs.Parameters.Length > 4)
		{
			var minutesToAddNullable = GetNullableDouble(functionArgs.Parameters[4]);
			if (!minutesToAddNullable.HasValue)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Minutes to add must be a number.");
			}
			minutesToAdd = minutesToAddNullable.Value;
		}

		// Seconds to add
		double secondsToAdd = 0;
		if (functionArgs.Parameters.Length > 5)
		{
			var secondsToAddNullable = GetNullableDouble(functionArgs.Parameters[5]);
			if (!secondsToAddNullable.HasValue)
			{
				throw new FormatException($"{ExtensionFunction.DateTime} function - Seconds to add must be a number.");
			}
			secondsToAdd = secondsToAddNullable.Value;
		}

		functionArgs.Result = DateTimeOffset
			.UtcNow
			.AddDays(daysToAdd)
			.AddHours(hoursToAdd)
			.AddMinutes(minutesToAdd)
			.AddSeconds(secondsToAdd)
			.ToString(format, CultureInfo.InvariantCulture);
	}

	private static double? GetNullableDouble(Expression expression)
		=> (expression.Evaluate()) switch
		{
			double doubleResult => doubleResult,
			int intResult => intResult,
			_ => null,
		};
}
