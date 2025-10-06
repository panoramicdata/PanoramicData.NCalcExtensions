namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("dateAdd")]
	[Description("Add a specified period to a DateTime. The following units are supported: Years/Months/Days/Hours/Minutes/Seconds/Milliseconds")]
	DateTime DateAddMethods(
		[Description("A DateTime to which to add the period specified.")]
		DateTime dateTime,
		[Description("The integer number of the units to be added.")]
		int quantity,
		[Description("A string representing the units used to specify the period to be added, example: 'hours'")]
		string units
	);
}

/// <summary>
/// Methods for dates and time maths
/// </summary>
internal static class DateAddMethods
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length < 3)
		{
			throw new FormatException($"{ExtensionFunction.DateTime} function - you must pass 3 parameters.");
		}

		// The date and time
		if (functionArgs.Parameters[0].Evaluate() is not DateTime initialDateTime)
		{
			throw new FormatException($"{ExtensionFunction.DateAdd} function - The first argument should be a DateTime");
		}

		// The quantity
		if (functionArgs.Parameters[1].Evaluate() is not int quantity)
		{
			throw new FormatException($"{ExtensionFunction.DateAdd} function - The second argument should be an integer");
		}

		// The units
		if (functionArgs.Parameters[2].Evaluate() is not string unitsString)
		{
			throw new FormatException($"{ExtensionFunction.DateAdd} function - The third argument should be a string, e.g. 'days'");
		}

		if (!Enum.TryParse(unitsString, true, out DateTimeUnit units))
		{
			throw new FormatException($"{ExtensionFunction.DateAdd} function - The third argument is not a recognised unit, e.g. 'days'");
		}

		functionArgs.Result = Add(initialDateTime, quantity, units);
	}

	/// <summary>
	/// Perform the addition, returning the sum of the DateTime and the period
	/// </summary>
	/// <param name="initialDateTime">The date and time that acts as the basis for the sum</param>
	/// <param name="quantity">The quantity of the units to be added</param>
	/// <param name="units">The units used to define the period to be added</param>
	/// <returns>The original DateTime plus the duration specified</returns>
	/// <exception cref="FormatException">The requested interval was not recognised</exception>
	private static DateTime Add(DateTime initialDateTime, int quantity, DateTimeUnit units)
		=> units switch
		{
			DateTimeUnit.Milliseconds => initialDateTime.AddMilliseconds(quantity),
			DateTimeUnit.Seconds => initialDateTime.AddSeconds(quantity),
			DateTimeUnit.Minutes => initialDateTime.AddMinutes(quantity),
			DateTimeUnit.Hours => initialDateTime.AddHours(quantity),
			DateTimeUnit.Days => initialDateTime.AddDays(quantity),
			DateTimeUnit.Months => initialDateTime.AddMonths(quantity),
			DateTimeUnit.Years => initialDateTime.AddYears(quantity),
			_ => throw new FormatException($"{ExtensionFunction.DateAdd} function - The requested units were not recognised")
		};
}
