namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("dateTimeIsInFuture")]
	[Description("Returns whether a date and time is in the future, with an optional timezone correction.")]
	bool DateTimeIsInFuture(
		[Description("Date and time being tested.")]
		DateTime valueUnderTest,
		[Description("TimeZone of the date and time being tested.")]
		string timeZoneName
	);
}

internal static class DateTimeIsInFuture
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var destinationTimeZoneInfo = TimeZoneInfo.Utc;
		if (functionArgs.Parameters.Length < 1)
		{
			throw new FormatException($"{ExtensionFunction.DateTimeIsInFuture} function - The first argument must be a DateTime");
		}

		var parameter1Value = functionArgs.Parameters[0].Evaluate();
		if (parameter1Value is not DateTime)
		{
			throw new FormatException($"{ExtensionFunction.DateTimeIsInFuture} function - The first argument must be a DateTime");
		}

		var dateTimeUnderTest = (DateTime)parameter1Value;

		if (functionArgs.Parameters.Length > 1)
		{
			// Time Zone
			if (functionArgs.Parameters[1].Evaluate() is not string timeZoneName)
			{
				throw new FormatException($"{ExtensionFunction.DateTimeIsInFuture} function - The second argument should be a string, e.g. 'UTC'");
			}

			if (!TZConvert.TryGetTimeZoneInfo(timeZoneName, out destinationTimeZoneInfo))
			{
				throw new FormatException($"{ExtensionFunction.DateTimeIsInFuture} function - The requested timezone was not a recognized");
			}
		}
		// Time zone has been determined

		var currentDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, destinationTimeZoneInfo);
		functionArgs.Result = dateTimeUnderTest > currentDateTime;
	}
}
