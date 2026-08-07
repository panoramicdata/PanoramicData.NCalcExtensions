namespace PanoramicData.NCalcExtensions.Test;

public class DateTimeIsInWindowTests : NCalcTest
{
	// 2026-08-09 is a Sunday. The recurring test window is 02:00 for two hours.
	private static readonly DateTimeOffset SundayInsideWindowUtc = new(2026, 8, 9, 2, 30, 0, TimeSpan.Zero);

	private static ExtendedExpression Build(string expression, DateTimeOffset utcNow)
		=> new(expression, ExpressionOptions.None, CultureInfo.InvariantCulture, new FixedTimeProvider(utcNow));

	private static void AssertResult(string expression, DateTimeOffset utcNow, bool expected)
	{
		var result = Build(expression, utcNow).Evaluate();
		result.Should().BeOfType<bool>();
		((bool)result).Should().Be(expected);
	}

	[Fact]
	public void SixFieldCron_InsideWindow_ReturnsTrue()
		=> AssertResult("dateTimeIsInWindow('0 0 2 * * SUN', 7200)", SundayInsideWindowUtc, true);

	[Fact]
	public void SixFieldCron_OutsideWindow_ReturnsFalse()
		=> AssertResult("dateTimeIsInWindow('0 0 2 * * SUN', 7200)", new DateTimeOffset(2026, 8, 9, 5, 0, 0, TimeSpan.Zero), false);

	[Fact]
	public void FiveFieldCron_InsideWindow_ReturnsTrue()
		=> AssertResult("dateTimeIsInWindow('0 2 * * SUN', 7200)", SundayInsideWindowUtc, true);

	[Fact]
	public void QuartzStyleQuestionMark_InsideWindow_ReturnsTrue()
		=> AssertResult("dateTimeIsInWindow('0 0 2 ? * SUN', 7200)", SundayInsideWindowUtc, true);

	[Fact]
	public void AtWindowStart_ReturnsTrue()
		=> AssertResult("dateTimeIsInWindow('0 0 2 * * SUN', 7200)", new DateTimeOffset(2026, 8, 9, 2, 0, 0, TimeSpan.Zero), true);

	[Fact]
	public void AtWindowEnd_ReturnsFalse()
		=> AssertResult("dateTimeIsInWindow('0 0 2 * * SUN', 7200)", new DateTimeOffset(2026, 8, 9, 4, 0, 0, TimeSpan.Zero), false);

	[Fact]
	public void JustBeforeWindowEnd_ReturnsTrue()
		=> AssertResult("dateTimeIsInWindow('0 0 2 * * SUN', 7200)", new DateTimeOffset(2026, 8, 9, 3, 59, 59, TimeSpan.Zero), true);

	[Fact]
	public void WrongDay_ReturnsFalse()
		// 2026-08-07 is a Friday
		=> AssertResult("dateTimeIsInWindow('0 0 2 * * SUN', 7200)", new DateTimeOffset(2026, 8, 7, 2, 30, 0, TimeSpan.Zero), false);

	[Fact]
	public void TimeZoneInSummer_InsideLocalWindow_ReturnsTrue()
		// In August, Amsterdam is UTC+2, so the 02:00 local window starts at 00:00 UTC
		=> AssertResult("dateTimeIsInWindow('0 0 2 * * SUN', 7200, 'Europe/Amsterdam')", new DateTimeOffset(2026, 8, 9, 0, 30, 0, TimeSpan.Zero), true);

	[Fact]
	public void TimeZoneInSummer_AfterLocalWindow_ReturnsFalse()
		// 02:30 UTC is 04:30 in Amsterdam in August, after the two-hour window
		=> AssertResult("dateTimeIsInWindow('0 0 2 * * SUN', 7200, 'Europe/Amsterdam')", SundayInsideWindowUtc, false);

	[Fact]
	public void TimeZoneInWinter_InsideLocalWindow_ReturnsTrue()
		// 2026-01-11 is a Sunday; in January, Amsterdam is UTC+1, so 01:30 UTC is 02:30 local
		=> AssertResult("dateTimeIsInWindow('0 0 2 * * SUN', 7200, 'Europe/Amsterdam')", new DateTimeOffset(2026, 1, 11, 1, 30, 0, TimeSpan.Zero), true);

	[Fact]
	public void WindowLongerThanADay_StillOpen_ReturnsTrue()
		// A 48-hour window starting Sunday 02:00 is still open on Tuesday 01:00
		=> AssertResult("dateTimeIsInWindow('0 0 2 * * SUN', 172800)", new DateTimeOffset(2026, 8, 11, 1, 0, 0, TimeSpan.Zero), true);

	[Fact]
	public void EveryMinuteCron_AlwaysInWindow_ReturnsTrue()
		=> AssertResult("dateTimeIsInWindow('* * * * *', 60)", SundayInsideWindowUtc, true);

	[Theory]
	[InlineData("dateTimeIsInWindow()")]
	[InlineData("dateTimeIsInWindow('0 2 * * SUN')")]
	[InlineData("dateTimeIsInWindow('0 2 * * SUN', 7200, 'UTC', 'extra')")]
	public void InvalidParameterCount_ThrowsFormatException(string expression) => new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*requires two or three arguments*");

	[Fact]
	public void NonStringCron_ThrowsFormatException() => new ExtendedExpression("dateTimeIsInWindow(123, 7200)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*first argument must be a CRON expression*");

	[Fact]
	public void InvalidCron_ThrowsFormatException() => new ExtendedExpression("dateTimeIsInWindow('not a valid cron', 7200)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*must have 5 fields*");

	[Fact]
	public void InvalidCronField_ThrowsFormatException() => new ExtendedExpression("dateTimeIsInWindow('0 2 * * NOTADAY', 7200)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*is not a valid CRON expression*");

	[Fact]
	public void SevenFieldQuartzCronWithYear_ThrowsFormatException() => new ExtendedExpression("dateTimeIsInWindow('0 0 2 ? * SUN 2026', 7200)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*must have 5 fields*");

	[Theory]
	[InlineData("dateTimeIsInWindow('0 2 * * SUN', 0)")]
	[InlineData("dateTimeIsInWindow('0 2 * * SUN', -3600)")]
	public void NonPositiveDuration_ThrowsFormatException(string expression) => new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*must be positive*");

	[Fact]
	public void NonNumericDuration_ThrowsFormatException() => new ExtendedExpression("dateTimeIsInWindow('0 2 * * SUN', 'abc')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*must be a number*");

	[Fact]
	public void NonStringTimeZone_ThrowsFormatException() => new ExtendedExpression("dateTimeIsInWindow('0 2 * * SUN', 7200, 123)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*third argument should be a string*");

	[Fact]
	public void UnknownTimeZone_ThrowsFormatException() => new ExtendedExpression("dateTimeIsInWindow('0 2 * * SUN', 7200, 'Invalid/Timezone')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*timezone was not recognized*");

	private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
	{
		public override DateTimeOffset GetUtcNow() => utcNow;
	}
}
