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
		string? resolution = null
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

			var param2 = functionArgs.Parameters.Evaluate(1) as string
				?? throw new FormatException($"{ExtensionFunction.Humanize}() second parameter must be a string.");

			if (!Enum.TryParse<TimeUnit>(param2, true, out var param2TimeUnit))
			{
				throw new FormatException($"{ExtensionFunction.Humanize} function - Parameter 2 must be a time unit - one of {string.Join(", ", Enum.GetNames<TimeUnit>().Select(n => $"'{n}'"))}.");
			}

			TimeUnit? resolution = null;
			if (functionArgs.Parameters.Count > 2)
			{
				var param3 = functionArgs.Parameters.Evaluate(2) as string
					?? throw new FormatException($"{ExtensionFunction.Humanize}() third parameter must be a string.");

				if (!Enum.TryParse<TimeUnit>(param3, true, out var param3TimeUnit))
				{
					throw new FormatException($"{ExtensionFunction.Humanize} function - Parameter 3 must be a time unit - one of {string.Join(", ", Enum.GetNames<TimeUnit>().Select(n => $"'{n}'"))}.");
				}

				resolution = param3TimeUnit;
			}

			functionArgs.Result = Humanise(param1Double, param2TimeUnit, resolution);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"{ExtensionFunction.Humanize} function - The first number should be a valid floating-point number and the second should be a time unit ({string.Join(", ", Enum.GetNames<TimeUnit>())}).");
		}
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
