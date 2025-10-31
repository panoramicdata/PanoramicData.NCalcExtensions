namespace PanoramicData.NCalcExtensions.Test;

public class DateTimeIsInPastTests : NCalcTest
{
	[Fact]
	public void Evaluate_UtcDate100MillisecondsAgo_ReturnsTrue()
	{
		var expression = new ExtendedExpression("dateTimeIsInPast(valueUnderTest)");
		expression.Parameters.Add("valueUnderTest", DateTime.UtcNow.AddMilliseconds(-100));
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeTrue();
	}

	[Fact]
	public void Evaluate_UtcDateIn100Milliseconds_ReturnsFalse()
	{
		var expression = new ExtendedExpression("dateTimeIsInPast(valueUnderTest)");
		expression.Parameters.Add("valueUnderTest", DateTime.UtcNow.AddMilliseconds(100));
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeFalse();
	}

	[Fact]
	public void Evaluate_WestAfricaTime100MillisecondsAgo_ReturnsTrue()
	{
		// West Africa Time does not observe Daylight Saving, so its offset from UTC is constant
		var expression = new ExtendedExpression("dateTimeIsInPast(valueUnderTest, 'Africa/Luanda')");
		expression.Parameters.Add("valueUnderTest", DateTime.UtcNow.AddHours(1).AddMilliseconds(-100));
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeTrue();
	}

	[Fact]
	public void Evaluate_WestAfricaTimeIn100Milliseconds_ReturnsFalse()
	{
		// West Africa Time does not observe Daylight Saving, so its offset from UTC is constant
		var expression = new ExtendedExpression("dateTimeIsInPast(valueUnderTest, 'Africa/Luanda')");
		expression.Parameters.Add("valueUnderTest", DateTime.UtcNow.AddHours(1).AddMilliseconds(100));
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeFalse();
	}

	[Fact]
	public void Evaluate_UtcNowInComparisonToWestAfricaTime_ReturnsTrue()
	{
		// West Africa Time does not observe Daylight Saving, so its offset from UTC is constant
		var expression = new ExtendedExpression("dateTimeIsInPast(now(), 'Africa/Luanda')");
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeTrue();
	}

	[Fact]
	public void Evaluate_CentralEuropeanTime_ReturnsTrue()
	{
		// CET is always ahead of UTC, so UtcNow masquerading as CET is always in the past
		var expression = new ExtendedExpression("dateTimeIsInPast(valueUnderTest, 'Central European Standard Time')");
		expression.Parameters.Add("valueUnderTest", DateTime.UtcNow);
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeTrue();
	}

	[Fact]
	public void Evaluate_1stJan2001_ReturnsTrue()
	{
		var expression = new ExtendedExpression("dateTimeIsInPast(toDateTime('2001-01-01T00:00:00', 'yyyy-MM-ddTHH:mm:ss'))");
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeTrue();
	}

	[Fact]
	public void Evaluate_1stJan2201_ReturnsFalse()
	{
		var expression = new ExtendedExpression("dateTimeIsInPast(toDateTime('2201-01-01T00:00:00', 'yyyy-MM-ddTHH:mm:ss'))");
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeFalse();
	}

	// New edge case tests

	[Theory]
	[InlineData("dateTimeIsInPast()")]
	[InlineData("dateTimeIsInPast(null)")]
	public void DateTimeIsInPast_InvalidParameters_ThrowsException(string expression)
	{
		new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();
	}

	[Fact]
	public void DateTimeIsInPast_VeryOldDate_ReturnsTrue()
	{
		var expression = new ExtendedExpression("dateTimeIsInPast(valueUnderTest)");
		expression.Parameters.Add("valueUnderTest", new DateTime(1900, 1, 1));
		var result = expression.Evaluate();
		result.Should().NotBeNull();
		((bool)result!).Should().BeTrue();
	}

	[Fact]
	public void DateTimeIsInPast_DateTimeMin_ReturnsTrue()
	{
		var expression = new ExtendedExpression("dateTimeIsInPast(valueUnderTest)");
		expression.Parameters.Add("valueUnderTest", DateTime.MinValue);
		var result = expression.Evaluate();
		result.Should().NotBeNull();
		((bool)result!).Should().BeTrue();
	}

	[Fact]
	public void DateTimeIsInPast_DateTimeMax_ReturnsFalse()
	{
		var expression = new ExtendedExpression("dateTimeIsInPast(valueUnderTest)");
		expression.Parameters.Add("valueUnderTest", DateTime.MaxValue);
		var result = expression.Evaluate();
		result.Should().NotBeNull();
		((bool)result!).Should().BeFalse();
	}
}
