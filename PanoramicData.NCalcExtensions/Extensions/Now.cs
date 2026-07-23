namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("now")]
	[Description("Return the current date and time as a DateTime, with an optional timezone correction.")]
	DateTime Now(
		[Description("TimeZone.")]
		string timeZoneName
	);
}

internal static class Now
{
	internal static void Evaluate(FunctionEventArgs functionArgs, TimeProvider timeProvider)
	{
		var destinationTimeZoneInfo = TimeZoneInfo.Utc;

		if (functionArgs.Parameters.Count > 0)
		{
			// Time Zone
			if (functionArgs.Parameters.Evaluate(0) is not string timeZoneName)
			{
				throw new FormatException($"{ExtensionFunction.Now} function - The first argument should be a string, e.g. 'UTC'");
			}

			if (!TZConvert.TryGetTimeZoneInfo(timeZoneName, out destinationTimeZoneInfo))
			{
				throw new FormatException($"{ExtensionFunction.Now} function - The requested timezone was not a recognized");
			}
		}
		// Time zone has been determined

		functionArgs.Result = TimeZoneInfo.ConvertTime(timeProvider.GetUtcNow().UtcDateTime, destinationTimeZoneInfo);
	}
}
