namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("humanize")]
	[Description("Humanizes the value text.")]
	string Humanize(
		[Description("The value to be humanized - must be a floating-point number.")]
		object value,
		[Description("Time unit that the value represents, example: 'seconds'")]
		string timeUnit,
		[Description("(Optional) the time unit to round the output to, example: 'hours'. Nothing finer than this unit is shown.")]
		[Optional] string? resolution
	);
}

internal static class Humanize
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		try
		{
			var param1Obj = functionArgs.Parameters.Evaluate(0)
				?? throw new FormatException($"{ExtensionFunction.Humanize}() first parameter cannot be null.");

			if (!double.TryParse(param1Obj.ToString(), out var param1Double))
			{
				throw new FormatException($"{ExtensionFunction.Humanize}() first parameter must be a number.");
			}

			var timeUnit = ReadTimeUnit(functionArgs, 1, "second");
			var resolution = functionArgs.Parameters.Count > 2
				? ReadTimeUnit(functionArgs, 2, "third")
				: (TimeUnit?)null;

			functionArgs.Result = Humanise(param1Double, timeUnit, resolution);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"{ExtensionFunction.Humanize} function - The first number should be a valid floating-point number and the second should be a time unit ({string.Join(", ", Enum.GetNames<TimeUnit>())}).");
		}
	}

	private static TimeUnit ReadTimeUnit(FunctionEventArgs functionArgs, int index, string ordinal)
	{
		var text = functionArgs.Parameters.Evaluate(index) as string
			?? throw new FormatException($"{ExtensionFunction.Humanize}() {ordinal} parameter must be a string.");

		return Enum.TryParse<TimeUnit>(text, true, out var timeUnit)
			? timeUnit
			: throw new FormatException($"{ExtensionFunction.Humanize} function - Parameter {index + 1} must be a time unit - one of {string.Join(", ", Enum.GetNames<TimeUnit>().Select(n => $"'{n}'"))}.");
	}

	private static string Humanise(double param1Double, TimeUnit timeUnit, TimeUnit? resolution)
	{
		try
		{
			return TimeSpanHumanizer.Humanize(param1Double, timeUnit, resolution);
		}
		catch (OverflowException)
		{
			throw new FormatException("The value is too big to use humanize. It must be a double (a 64-bit, floating point number)");
		}
	}
}
