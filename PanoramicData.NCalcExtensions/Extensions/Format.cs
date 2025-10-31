namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("format")]
	[Description("Formats strings and numbers as output strings with the specified format.")]
	string Format(
		[Description("The value to be formated (number or text).")]
		object value,
		[Description("The format to use:\r\n\r\n"
			+ "- See C# number and date/time formatting\r\n"
			+ "- 'weekOfMonth' is the numeric week of month as would be shown on a calendar with one row per week with weeks starting on a Sunday\r\n"
			+ "- 'weekOfMonthText' is the same as weekOfMonth, but translated: 1: 'first', 2: 'second', 3: 'third', 4: 'forth', 5: 'last'\r\n"
			+ "- 'weekDayOfMonth' is the number of times this weekday has occurred within the month so far, including this one\r\n"
			+ "- 'weekDayOfMonthText' is the same as weekDayOfMonth, but translated: 1: 'first', 2: 'second', 3: 'third', 4: 'forth', 5: 'last'"
		)]
		string formatString,
		[Description("(Optional) Timezone to use when formatting DateTimes.")]
		string? timezone
	);
}

internal static class Format
{
	internal static void Evaluate(FunctionArgs functionArgs, CultureInfo cultureInfo)
	{
		const int min = 2;
		const int max = 3;
		if (functionArgs.Parameters.Length is < min or > max)
		{
			throw new ArgumentException($"{ExtensionFunction.Format} function - expected between {min} and {max} arguments");
		}

		var inputObject = functionArgs.Parameters[0].Evaluate()
			?? throw new ArgumentException($"{ExtensionFunction.Format} function - first argument cannot be null");
		
		if (functionArgs.Parameters[1].Evaluate() is not string formatFormat)
		{
			throw new ArgumentException($"{ExtensionFunction.Format} function - expected second argument to be a format string");
		}

		functionArgs.Result = (inputObject, functionArgs.Parameters.Length) switch
		{
			(int inputInt, 2) => inputInt.ToString(formatFormat, cultureInfo),
			(double inputDouble, 2) => inputDouble.ToString(formatFormat, cultureInfo),
			(DateTime dateTime, 2) => dateTime.BetterToString(formatFormat, cultureInfo),
			(DateTime dateTime, 3) => dateTime.ToDateTimeInTargetTimeZone(formatFormat, functionArgs.Parameters[2].Evaluate() as string ?? throw new ArgumentException($"{ExtensionFunction.Format} function - expected third argument to be a TimeZone string"), cultureInfo),
			(string inputString, 2) => GetThing(inputString, formatFormat, cultureInfo),
			_ => throw new NotSupportedException($"Unsupported input type {inputObject.GetType().Name} or incorrect number of parameters.")
		};
	}

	private static string GetThing(string inputString, string formatFormat, CultureInfo cultureInfo) =>
		long.TryParse(inputString, out var longValue)
			? longValue.ToString(formatFormat, cultureInfo)
		: double.TryParse(inputString, out var doubleValue)
			? doubleValue.ToString(formatFormat, cultureInfo)
		: DateTimeOffset.TryParse(
			inputString,
			cultureInfo.DateTimeFormat,
			DateTimeStyles.AssumeUniversal,
			out var dateTimeOffsetValue)
			? dateTimeOffsetValue.UtcDateTime.BetterToString(formatFormat, cultureInfo)
		: throw new FormatException($"Could not parse '{inputString}' as a number or date.");
}
