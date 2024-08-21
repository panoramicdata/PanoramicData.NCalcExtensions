using PanoramicData.NCalcExtensions.Helpers;
using System.ComponentModel;

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
		string timeUnit
	);
}

internal static class Humanize
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		double param1Double;
		string param2;
		try
		{
			if (double.TryParse(functionArgs.Parameters[0].Evaluate().ToString(), out var result))
			{
				param1Double = result;
				param2 = (string)functionArgs.Parameters[1].Evaluate();
			}
			else
			{
				throw new FormatException($"{ExtensionFunction.Humanize}() first parameter must be a number.");
			}
		}
		catch (Exception e) when (e is not NCalcExtensionsException)
		{
			throw new FormatException($"{ExtensionFunction.Humanize} function - The first number should be a valid floating-point number and the second should be a time unit ({string.Join(", ", Enum.GetNames(typeof(TimeUnit)))}).");
		}

		if (!Enum.TryParse<TimeUnit>(param2, true, out var param2TimeUnit))
		{
			throw new FormatException($"{ExtensionFunction.Humanize} function - Parameter 2 must be a time unit - one of {string.Join(", ", Enum.GetNames(typeof(TimeUnit)).Select(n => $"'{n}'"))}.");
		}

		functionArgs.Result = Humanise(param1Double, param2TimeUnit);
	}

	private static string Humanise(double param1Double, TimeUnit timeUnit)
	{
		try
		{
			return timeUnit switch
			{
				TimeUnit.Milliseconds => System.TimeSpan.FromMilliseconds(param1Double).Humanise(),
				TimeUnit.Seconds => System.TimeSpan.FromSeconds(param1Double).Humanise(),
				TimeUnit.Minutes => System.TimeSpan.FromMinutes(param1Double).Humanise(),
				TimeUnit.Hours => System.TimeSpan.FromHours(param1Double).Humanise(),
				TimeUnit.Days => System.TimeSpan.FromDays(param1Double).Humanise(),
				TimeUnit.Weeks => System.TimeSpan.FromDays(param1Double * 7).Humanise(),
				TimeUnit.Years => System.TimeSpan.FromDays(param1Double * 365.25).Humanise(),
				_ => throw new FormatException($"{timeUnit} is not a supported time unit for humanization."),
			};
		}
		catch (OverflowException)
		{
			throw new FormatException("The value is too big to use humanize. It must be a double (a 64-bit, floating point number)");
		}
	}
}
