namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Format
{
	internal static void Evaluate(FunctionArgs functionArgs, CultureInfo cultureInfo)
	{
		const int min = 2;
		const int max = 3;
		if (functionArgs.Parameters.Length < min || functionArgs.Parameters.Length > max)
		{
			throw new ArgumentException($"{ExtensionFunction.Format} function - expected between {min} and {max} arguments");
		}

		var inputObject = functionArgs.Parameters[0].Evaluate();
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

	private static string GetThing(string inputString, string formatFormat, CultureInfo cultureInfo)
	{
		// Assume this is a number
		if (long.TryParse(inputString, out var longValue))
		{
			return longValue.ToString(formatFormat, cultureInfo);
		}

		if (double.TryParse(inputString, out var doubleValue))
		{
			return doubleValue.ToString(formatFormat, cultureInfo);
		}

		if (DateTimeOffset.TryParse(
			inputString,
			cultureInfo.DateTimeFormat,
			DateTimeStyles.AssumeUniversal,
			out var dateTimeOffsetValue))
		{
			return dateTimeOffsetValue.UtcDateTime.BetterToString(formatFormat, cultureInfo);
		}

		throw new FormatException($"Could not parse '{inputString}' as a number or date.");
	}
}
