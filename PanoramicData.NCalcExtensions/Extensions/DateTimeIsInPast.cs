namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("dateTimeIsInPast")]
	[Description("Returns whether a date and time is in the past, with an optional timezone correction.")]
	bool DateTimeIsInPast(
		[Description("Date and time being tested.")]
		DateTime valueUnderTest,
		[Description("TimeZone.")]
		string timeZoneName
	);
}

internal static class DateTimeIsInPast
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var destinationTimeZoneInfo = TimeZoneInfo.Utc;

		if (functionArgs.Parameters.Length < 1)
		{
			throw new FormatException($"{ExtensionFunction.DateTimeIsInPast} function - The first argument must be a DateTime");
		}

		var parameter1Value = functionArgs.Parameters[0].Evaluate();
		if (parameter1Value is not DateTime)
		{
			throw new FormatException($"{ExtensionFunction.DateTimeIsInPast} function - The first argument must be a DateTime");
		}
		var dateTimeUnderTest = (DateTime)parameter1Value;

		if (functionArgs.Parameters.Length > 1)
		{
			// Time Zone
			if (functionArgs.Parameters[1].Evaluate() is not string timeZoneName)
			{
				throw new FormatException($"{ExtensionFunction.DateTimeIsInPast} function - The second argument should be a string, e.g. 'UTC'");
			}

			if (!TZConvert.TryGetTimeZoneInfo(timeZoneName, out destinationTimeZoneInfo))
			{
				throw new FormatException($"{ExtensionFunction.DateTimeIsInPast} function - The requested timezone was not a recognized");
			}
		}
		// Time zone has been determined

		var currentDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, destinationTimeZoneInfo);
		functionArgs.Result = currentDateTime > dateTimeUnderTest;
	}
}
