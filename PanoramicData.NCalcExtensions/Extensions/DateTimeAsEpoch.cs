﻿namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("dateTimeAsEpoch")]
	[Description("Parses the input DateTime and outputs as seconds since the Epoch (1970-01-01T00:00Z).")]
	long DateTimeAsEpoch(
		[Description("Input date string, example: '20190702T000000'.")]
		string dateString,
		[Description("Format of the given input date string, example: 'yyyyMMddTHHmmssK'.")]
		string format
	);
}

internal static class DateTimeAsEpoch
{
	internal static void Evaluate(FunctionArgs functionArgs, CultureInfo cultureInfo)
	{
		var input = functionArgs.Parameters[0].Evaluate() as string
			?? throw new FormatException($"{ExtensionFunction.DateTimeAsEpoch}() requires two string parameters.");
		var format = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"{ExtensionFunction.DateTimeAsEpoch}() requires two string parameters.");

		var dateTimeOffset = DateTimeOffset.ParseExact(
			input,
			format,
			cultureInfo.DateTimeFormat,
			DateTimeStyles.AssumeUniversal);
		
		functionArgs.Result = dateTimeOffset.ToUnixTimeSeconds();
	}
}