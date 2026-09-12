namespace PanoramicData.NCalcExtensions.Test;

public class NowTests : NCalcTest
{
	[Theory]
	[InlineData("now()")]
	[InlineData("now('UTC')")]
	public void Now_InUtc_ReturnsCurrentDateAndTime(string expressionText)
	{
		var result = Test(expressionText);
		var desiredDateTime = DateTime.UtcNow;
		result.Should().BeOfType<DateTime>();

		(desiredDateTime - (DateTime)result!).Should().BeLessThan(TimeSpan.FromMilliseconds(1));
	}

	[Fact]
	public void Now_NoParameters_ReturnsValueOfKindUtc()
		=> KindOf(Test("now()")).Should().Be(DateTimeKind.Utc);

	[Theory]
	[InlineData("Central European Standard Time")]
	[InlineData("Pacific Standard Time")]
	[InlineData("Tokyo Standard Time")]
	public void Now_InNamedTimeZone_ReturnsValueOfKindUnspecified(string timeZone)
		=> KindOf(Test($"now('{timeZone}')")).Should().Be(DateTimeKind.Unspecified);

	[Fact]
	public void Now_EasternTimeZone_ReturnsCorrectTime()
	{
		var result = Test("now('Eastern Standard Time')");
		result.Should().BeOfType<DateTime>();

		// Eastern is UTC-5, or UTC-4 under daylight saving.
		Math.Abs((DateTime.UtcNow - (DateTime)result!).TotalHours).Should().BeInRange(0, 7);
	}

	[Fact]
	public void Now_UtcOffset_IsReasonable()
	{
		var result = Test("now()");
		result.Should().NotBeNull();

		Math.Abs((DateTime.UtcNow - (DateTime)result!).TotalSeconds).Should().BeLessThan(1);
	}

	[Fact]
	public void Now_RepeatedCalls_ReturnsIncreasingTime()
	{
		var first = Test("now()");
		System.Threading.Thread.Sleep(10);
		var second = Test("now()");

		first.Should().BeOfType<DateTime>();
		second.Should().BeOfType<DateTime>();
		((DateTime)second!).Should().BeAfter((DateTime)first!);
	}

	[Theory]
	[InlineData("now('Invalid/Timezone')", "*")]
	[InlineData("now('')", "*")]
	[InlineData("now(123)", "*first argument should be a string*")]
	public void Now_WithInvalidArguments_ThrowsFormatException(string expressionText, string messagePattern)
		=> TestShouldThrow<FormatException>(expressionText, messagePattern);

	/// <summary>
	/// The kind of a now() result, which must be a DateTime.
	/// </summary>
	private static DateTimeKind KindOf(object? result)
	{
		result.Should().BeOfType<DateTime>();
		return ((DateTime)result!).Kind;
	}
}
