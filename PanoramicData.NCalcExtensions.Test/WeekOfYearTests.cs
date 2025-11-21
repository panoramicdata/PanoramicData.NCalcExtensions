namespace PanoramicData.NCalcExtensions.Test;

public class WeekOfYearTests : NCalcTest
{
	[Theory]
	[InlineData("2024-01-01", 1)]   // Monday, Week 1
	[InlineData("2024-01-07", 2)]   // Sunday, Week 2
	[InlineData("2024-12-31", 53)]  // Tuesday, Week 53
	[InlineData("2023-01-01", 1)]   // Sunday, Week 1
	[InlineData("2023-12-31", 53)]  // Sunday, Week 53
	[InlineData("2021-01-01", 53)]  // Friday, Week 53 (of 2020 in ISO)
	[InlineData("2021-12-31", 52)]  // Friday, Week 52
	public void Format_WeekOfYear_ReturnsExpected(string dateString, int expectedWeek)
	{
		var expression = new ExtendedExpression($"format('{dateString}', 'weekOfYear')");
		var result = expression.Evaluate();
		result.Should().Be(expectedWeek.ToString(CultureInfo.InvariantCulture));
	}

	[Theory]
	[InlineData("2024-01-01", 1)]   // Monday is start of ISO week 1
	[InlineData("2024-01-07", 1)]   // Sunday is end of ISO week 1
	[InlineData("2024-12-30", 1)]   // Monday starts ISO week 1 of 2025
	[InlineData("2023-01-01", 52)]  // Sunday is in ISO week 52 of 2022
	[InlineData("2023-01-02", 1)]   // Monday starts ISO week 1 of 2023
	[InlineData("2021-01-01", 53)]  // Friday in ISO week 53 of 2020
	[InlineData("2021-01-04", 1)]   // Monday starts ISO week 1 of 2021
	public void Format_IsoWeekOfYear_ReturnsExpected(string dateString, int expectedWeek)
	{
		var expression = new ExtendedExpression($"format('{dateString}', 'isoWeekOfYear')");
		var result = expression.Evaluate();
		result.Should().Be(expectedWeek.ToString(CultureInfo.InvariantCulture));
	}

	[Fact]
	public void Format_WeekOfYear_WithDateTime_Succeeds()
	{
		var expression = new ExtendedExpression("format(theDateTime, 'weekOfYear')");
		expression.Parameters["theDateTime"] = new DateTime(2024, 6, 15);
		var result = expression.Evaluate();
		result.Should().NotBeNull();
		var weekNumber = int.Parse(result.ToString()!, CultureInfo.InvariantCulture);
		weekNumber.Should().BeGreaterThan(0).And.BeLessThanOrEqualTo(53);
	}

	[Fact]
	public void Format_IsoWeekOfYear_WithDateTime_Succeeds()
	{
		var expression = new ExtendedExpression("format(theDateTime, 'isoWeekOfYear')");
		expression.Parameters["theDateTime"] = new DateTime(2024, 6, 15);
		var result = expression.Evaluate();
		result.Should().NotBeNull();
		var weekNumber = int.Parse(result.ToString()!, CultureInfo.InvariantCulture);
		weekNumber.Should().BeGreaterThan(0).And.BeLessThanOrEqualTo(53);
	}

	[Theory]
	[InlineData("2024-W01", "2024-01-01")]
	[InlineData("2024-W26", "2024-06-24")]
	[InlineData("2024-W52", "2024-12-23")]
	public void Format_IsoWeekOfYear_MatchesExpectedWeek(string expectedIsoWeek, string dateString)
	{
		ArgumentNullException.ThrowIfNull(expectedIsoWeek);
		
		var expression = new ExtendedExpression($"format('{dateString}', 'isoWeekOfYear')");
		var result = expression.Evaluate();
		result.Should().NotBeNull();
		var weekNumber = int.Parse(result.ToString()!, CultureInfo.InvariantCulture);
		var expectedWeekNumber = int.Parse(expectedIsoWeek.Split('-')[1][1..], CultureInfo.InvariantCulture);
		weekNumber.Should().Be(expectedWeekNumber);
	}
}
