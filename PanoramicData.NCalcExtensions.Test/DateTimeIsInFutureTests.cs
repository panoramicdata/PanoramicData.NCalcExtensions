namespace PanoramicData.NCalcExtensions.Test;

public class DateTimeIsInFutureTests : NCalcTest
{
	[Fact]
	public void Evaluate_UtcDateIn100Milliseconds_ReturnsTrue()
	{
		var expression = new ExtendedExpression("dateTimeIsInFuture(valueUnderTest)");
		expression.Parameters.Add("valueUnderTest", DateTime.UtcNow.AddMilliseconds(2000));
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeTrue();
	}

	[Fact]
	public void Evaluate_UtcDate100MillisecondsAgo_ReturnsFalse()
	{
		var expression = new ExtendedExpression("dateTimeIsInFuture(valueUnderTest)");
		expression.Parameters.Add("valueUnderTest", DateTime.UtcNow.AddMilliseconds(-2000));
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeFalse();
	}

	[Fact]
	public void Evaluate_WestAfricaTimeIn100Milliseconds_ReturnsTrue()
	{
		// West Africa Time does not observe Daylight Saving, so its offset from UTC is constant
		var expression = new ExtendedExpression("dateTimeIsInFuture(valueUnderTest, 'Africa/Luanda')");
		expression.Parameters.Add("valueUnderTest", DateTime.UtcNow.AddHours(1).AddMilliseconds(2000));
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeTrue();
	}

	[Fact]
	public void Evaluate_WestAfricaTime100MillisecondsAgo_ReturnsFalse()
	{
		// West Africa Time does not observe Daylight Saving, so its offset from UTC is constant
		var expression = new ExtendedExpression("dateTimeIsInFuture(valueUnderTest, 'Africa/Luanda')");
		expression.Parameters.Add("valueUnderTest", DateTime.UtcNow.AddHours(1).AddMilliseconds(-2000));
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeFalse();
	}

	[Fact]
	public void Evaluate_UtcNowInComparisonToWestAfricaTime_ReturnsFalse()
	{
		// West Africa Time does not observe Daylight Saving, so its offset from UTC is constant
		var expression = new ExtendedExpression("dateTimeIsInFuture(now(), 'Africa/Luanda')");
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeFalse();
	}

	[Fact]
	public void Evaluate_CentralEuropeanTime_ReturnsFalse()
	{
		// CET is always ahead of UTC, so UtcNow masquerading as CET is always in the past
		var expression = new ExtendedExpression("dateTimeIsInFuture(valueUnderTest, 'Central European Standard Time')");
		expression.Parameters.Add("valueUnderTest", DateTime.UtcNow);
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeFalse();
	}

	[Fact]
	public void Evaluate_1stJan2001_ReturnsFalse()
	{
		var expression = new ExtendedExpression("dateTimeIsInFuture(toDateTime('2001-01-01T00:00:00', 'yyyy-MM-ddTHH:mm:ss'))");
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeFalse();
	}

	[Fact]
	public void Evaluate_1stJan2201_ReturnsTrue()
	{
		var expression = new ExtendedExpression("dateTimeIsInFuture(toDateTime('2201-01-01T00:00:00', 'yyyy-MM-ddTHH:mm:ss'))");
		var result = expression.Evaluate();
		result.Should().BeOfType<bool>();

		((bool)result).Should().BeTrue();
	}

	// New edge case tests

	[Theory]
	[InlineData("dateTimeIsInFuture()")]
	[InlineData("dateTimeIsInFuture(null)")]
	public void DateTimeIsInFuture_InvalidParameters_ThrowsException(string expression) => new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	[Fact]
	public void DateTimeIsInFuture_NonStringTimezone_ThrowsException()
	{
		var expression = new ExtendedExpression("dateTimeIsInFuture(valueUnderTest, 123)");
		expression.Parameters.Add("valueUnderTest", DateTime.UtcNow);
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*second argument should be a string*");
	}

	[Fact]
	public void DateTimeIsInFuture_InvalidTimezone_ThrowsException()
	{
		var expression = new ExtendedExpression("dateTimeIsInFuture(valueUnderTest, 'Invalid/Timezone')");
		expression.Parameters.Add("valueUnderTest", DateTime.UtcNow);
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*timezone was not a recognized*");
	}

	[Fact]
	public void DateTimeIsInFuture_VeryOldDate_ReturnsFalse()
	{
		var expression = new ExtendedExpression("dateTimeIsInFuture(valueUnderTest)");
		expression.Parameters.Add("valueUnderTest", new DateTime(1900, 1, 1));
		var result = expression.Evaluate();
		result.Should().NotBeNull();
		((bool)result!).Should().BeFalse();
	}

	[Fact]
	public void DateTimeIsInFuture_DateTimeMax_ReturnsTrue()
	{
		var expression = new ExtendedExpression("dateTimeIsInFuture(valueUnderTest)");
		expression.Parameters.Add("valueUnderTest", DateTime.MaxValue);
		var result = expression.Evaluate();
		result.Should().NotBeNull();
		((bool)result!).Should().BeTrue();
	}
}
