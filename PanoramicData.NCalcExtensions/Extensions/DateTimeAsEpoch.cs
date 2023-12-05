namespace PanoramicData.NCalcExtensions.Extensions;

internal static class DateTimeAsEpoch
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var dateTimeOffset = DateTimeOffset.ParseExact(
			functionArgs.Parameters[0].Evaluate() as string, // Input date as string
			functionArgs.Parameters[1].Evaluate() as string,
			CultureInfo.InvariantCulture.DateTimeFormat,
			DateTimeStyles.AssumeUniversal);
		functionArgs.Result = dateTimeOffset.ToUnixTimeSeconds();
	}
}