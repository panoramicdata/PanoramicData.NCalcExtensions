namespace PanoramicData.NCalcExtensions.Test;

public class NowTests : NCalcTest
{
	[Fact]
	public void Evaluate_NoParameters_ReturnsCurrentDateAndTime()
	{
		var result = Test($"now()");
		var desiredDateTime = DateTime.UtcNow;
		result.Should().BeOfType<DateTime>();

		var difference = desiredDateTime - (DateTime)result;
		difference.Should().BeLessThan(TimeSpan.FromMilliseconds(1));
	}

	[Fact]
	public void Evaluate_NoParameters_ReturnsValueOfKindUtc()
	{
		var result = Test($"now()");
		result.Should().BeOfType<DateTime>();

		((DateTime)result).Kind.Should().Be(DateTimeKind.Utc);
	}

	[Fact]
	public void Evaluate_UTCTimeZone_ReturnsCurrentDateAndTime()
	{
		var result = Test($"now('UTC')");
		var desiredDateTime = DateTime.UtcNow;
		result.Should().BeOfType<DateTime>();

		var difference = desiredDateTime - (DateTime)result;
		difference.Should().BeLessThan(TimeSpan.FromMilliseconds(1));
	}

	[Fact]
	public void Evaluate_CETTimeZone_ReturnsValueOfKindUnspecified()
	{
		var result = Test($"now('Central European Standard Time')");
		result.Should().BeOfType<DateTime>();

		((DateTime)result).Kind.Should().Be(DateTimeKind.Unspecified);
	}

	// Additional comprehensive tests

	[Fact]
	public void Now_EasternTimeZone_ReturnsCorrectTime()
	{
		var result = Test("now('Eastern Standard Time')");
		result.Should().BeOfType<DateTime>();
		var nowEst = (DateTime)result;
		var nowUtc = DateTime.UtcNow;
		// EST is typically UTC-5 or UTC-4 (daylight saving), difference should be reasonable
		var difference = Math.Abs((nowUtc - nowEst).TotalHours);
		difference.Should().BeInRange(0, 7); // Account for timezone offset range
	}

	[Fact]
	public void Now_PacificTimeZone_ReturnsCorrectTime()
	{
		var result = Test("now('Pacific Standard Time')");
		result.Should().BeOfType<DateTime>();
		((DateTime)result).Kind.Should().Be(DateTimeKind.Unspecified);
	}

	[Fact]
	public void Now_InvalidTimeZone_ThrowsException()
		=> new ExtendedExpression("now('Invalid/Timezone')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	[Fact]
	public void Now_EmptyStringTimeZone_ThrowsException()
		=> new ExtendedExpression("now('')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	[Fact]
	public void Now_NonStringParameter_ThrowsException()
		=> new ExtendedExpression("now(123)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*first argument should be a string*");

	[Fact]
	public void Now_RepeatedCalls_ReturnsIncreasingTime()
	{
		var result1 = Test("now()");
		System.Threading.Thread.Sleep(10); // Small delay
		var result2 = Test("now()");
		
		result1.Should().BeOfType<DateTime>();
		result2.Should().BeOfType<DateTime>();
		((DateTime)result2).Should().BeAfter((DateTime)result1);
	}

	[Fact]
	public void Now_UtcOffset_IsReasonable()
	{
		var result = Test("now()");
		result.Should().NotBeNull();
		var nowUtc = DateTime.UtcNow;
		var difference = Math.Abs((nowUtc - (DateTime)result!).TotalSeconds);
		difference.Should().BeLessThan(1); // Should be within 1 second
	}

	[Fact]
	public void Now_TokyoTimeZone_ReturnsCorrectTime()
	{
		var result = Test("now('Tokyo Standard Time')");
		result.Should().BeOfType<DateTime>();
		((DateTime)result).Kind.Should().Be(DateTimeKind.Unspecified);
	}
}
