namespace PanoramicData.NCalcExtensions.Test;

public class DateAddTests : NCalcTest
{
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
		var recognised = DateTime.TryParse(initialDateAndTime, out var initialDateTime);
		recognised.Should().BeTrue();

		recognised = DateTime.TryParse(expectedDateAndTime, out var expectedDateTime);
		recognised.Should().BeTrue();

		var expression = new ExtendedExpression("dateAdd(initialDateTime, quantity, units)");
		expression.Parameters.Add("units", units);
		expression.Parameters.Add("quantity", quantity);
		expression.Parameters.Add("initialDateTime", initialDateTime);

		var result = expression.Evaluate();
		result.Should().BeOfType<DateTime>();
		result.Should().Be(expectedDateTime);
	}

	[Theory]
	[InlineData("2023-12-05T05:00:01Z", 250, "aa")]
	[InlineData("2023-12-05T05:00:01Z", 1, "nanoseconds")]
	[InlineData("2023-12-05T05:00:01Z", 1, "weeks")]
	public void DateAdd_UnknownUnits_ThrowsFormatException(string initialDateAndTime, int quantity, string units)
	{
		var recognised = DateTime.TryParse(initialDateAndTime, out var initialDateTime);
		recognised.Should().BeTrue();

		var expression = new ExtendedExpression("dateAdd(initialDateTime, quantity, units)");
		expression.Parameters.Add("units", units);
		expression.Parameters.Add("quantity", quantity);
		expression.Parameters.Add("initialDateTime", initialDateTime);

		var action = expression.Evaluate;
		action.Should().Throw<FormatException>();
	}

	[Fact]
	public void DateAdd_SubtractionBeyondMinDateTime_ThrowsArgumentOutOfRangeException()
	{
		var units = "Years";
		var quantity = -1000000;
		var initialDateTime = new DateTime(2023, 12, 05, 05, 00, 01);

		var expression = new ExtendedExpression("dateAdd(initialDateTime, quantity, units)");
		expression.Parameters.Add("units", units);
		expression.Parameters.Add("quantity", quantity);
		expression.Parameters.Add("initialDateTime", initialDateTime);

		var action = expression.Evaluate;
		action.Should().Throw<ArgumentOutOfRangeException>();
	}

	[Fact]
	public void DateAdd_IncorrectUnitsDataType_ThrowsFormatException()
	{
		var units = 1;
		var quantity = 1;
		var initialDateTime = new DateTime(2023, 12, 05, 05, 00, 01);

		var expression = new ExtendedExpression("dateAdd(initialDateTime, quantity, units)");
		expression.Parameters.Add("units", units);
		expression.Parameters.Add("quantity", quantity);
		expression.Parameters.Add("initialDateTime", initialDateTime);

		var action = expression.Evaluate;
		action.Should().Throw<FormatException>();
	}

	[Fact]
	public void DateAdd_IncorrectQuantityDataType_ThrowsFormatException()
	{
		var units = "Hours";
		var quantity = "Hours";
		var initialDateTime = new DateTime(2023, 12, 05, 05, 00, 01);

		var expression = new ExtendedExpression("dateAdd(initialDateTime, quantity, units)");
		expression.Parameters.Add("units", units);
		expression.Parameters.Add("quantity", quantity);
		expression.Parameters.Add("initialDateTime", initialDateTime);

		var action = expression.Evaluate;
		action.Should().Throw<FormatException>();
	}

	[Fact]
	public void DateAdd_IncorrectDateTimeDataType_ThrowsFormatException()
	{
		var units = "Hours";
		var quantity = 1;
		var initialDateTime = new DateTime(2023, 12, 05, 05, 00, 01).ToString(CultureInfo.InvariantCulture);

		var expression = new ExtendedExpression("dateAdd(initialDateTime, quantity, units)");
		expression.Parameters.Add("units", units);
		expression.Parameters.Add("quantity", quantity);
		expression.Parameters.Add("initialDateTime", initialDateTime);

		var action = expression.Evaluate;
		action.Should().Throw<FormatException>();
	}
}
