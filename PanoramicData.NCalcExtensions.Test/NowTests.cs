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
}
