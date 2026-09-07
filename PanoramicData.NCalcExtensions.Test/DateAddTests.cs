namespace PanoramicData.NCalcExtensions.Test;

public class DateAddTests : NCalcTest
{
	/// <summary>
	/// A valid initial DateTime, for the tests that vary one of the other arguments.
	/// </summary>
	private static readonly DateTime InitialDateTime = new(2023, 12, 05, 05, 00, 01);

	/// <summary>
	/// Evaluates dateAdd() with the given arguments supplied as parameters.
	/// </summary>
	private static object? DateAdd(object initialDateTime, object quantity, object units)
	{
		var expression = new ExtendedExpression("dateAdd(initialDateTime, quantity, units)");
		expression.Parameters.Add("units", units);
		expression.Parameters.Add("quantity", quantity);
		expression.Parameters.Add("initialDateTime", initialDateTime);

		return expression.Evaluate();
	}

	[Theory]
	[InlineData("2023-12-05T05:00:01Z", 250, "milliseconds", "2023-12-05T05:00:01.250Z")]
	[InlineData("2023-12-05T05:00:01Z", 250, "Milliseconds", "2023-12-05T05:00:01.250Z")]
	[InlineData("2023-12-05T05:00:01Z", 1, "seconds", "2023-12-05T05:00:02Z")]
	[InlineData("2023-12-05T05:00:01Z", 1, "SECONDS", "2023-12-05T05:00:02Z")]
	[InlineData("2023-12-05T05:00:01Z", 1, "minutes", "2023-12-05T05:01:01Z")]
	[InlineData("2023-12-05T05:00:01Z", 1, "mInUtEs", "2023-12-05T05:01:01Z")]
	[InlineData("2023-12-05T05:00:01Z", 1, "hours", "2023-12-05T06:00:01Z")]
	[InlineData("2023-12-05T05:00:01Z", 1, "days", "2023-12-06T05:00:01Z")]
	[InlineData("2023-12-05T05:00:01Z", 1, "months", "2024-01-05T05:00:01Z")]
	[InlineData("2023-12-05T05:00:01Z", 1, "years", "2024-12-05T05:00:01Z")]
	public void DateAdd_ParameterizedInput_GivesExpectedOutput(string initialDateAndTime, int quantity, string units, string expectedDateAndTime)
	{
		DateTime.TryParse(initialDateAndTime, out var initialDateTime).Should().BeTrue();
		DateTime.TryParse(expectedDateAndTime, out var expectedDateTime).Should().BeTrue();

		var result = DateAdd(initialDateTime, quantity, units);

		result.Should().BeOfType<DateTime>();
		result.Should().Be(expectedDateTime);
	}

	[Theory]
	[InlineData("2023-12-05T05:00:01Z", 250, "aa")]
	[InlineData("2023-12-05T05:00:01Z", 1, "nanoseconds")]
	[InlineData("2023-12-05T05:00:01Z", 1, "weeks")]
	public void DateAdd_UnknownUnits_ThrowsFormatException(string initialDateAndTime, int quantity, string units)
	{
		DateTime.TryParse(initialDateAndTime, out var initialDateTime).Should().BeTrue();

		var action = () => DateAdd(initialDateTime, quantity, units);

		action.Should().Throw<FormatException>();
	}

	[Fact]
	public void DateAdd_SubtractionBeyondMinDateTime_ThrowsArgumentOutOfRangeException()
	{
		var action = () => DateAdd(InitialDateTime, -1000000, "Years");

		action.Should().Throw<ArgumentOutOfRangeException>();
	}

	[Fact]
	public void DateAdd_IncorrectUnitsDataType_ThrowsFormatException()
	{
		var action = () => DateAdd(InitialDateTime, 1, 1);

		action.Should().Throw<FormatException>();
	}

	[Fact]
	public void DateAdd_IncorrectQuantityDataType_ThrowsFormatException()
	{
		var action = () => DateAdd(InitialDateTime, "Hours", "Hours");

		action.Should().Throw<FormatException>();
	}

	[Fact]
	public void DateAdd_IncorrectDateTimeDataType_ThrowsFormatException()
	{
		var action = () => DateAdd(InitialDateTime.ToString(CultureInfo.InvariantCulture), 1, "Hours");

		action.Should().Throw<FormatException>();
	}
}
