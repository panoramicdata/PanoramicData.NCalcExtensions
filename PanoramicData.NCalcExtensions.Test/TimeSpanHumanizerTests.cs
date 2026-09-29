namespace PanoramicData.NCalcExtensions.Test;

public class TimeSpanHumanizerTests : NCalcTest
{
	[Theory]
	[InlineData(0.51428, TimeUnit.Weeks, null, "3 days 14 hours 23 minutes 56 seconds")]
	[InlineData(0.51428, TimeUnit.Weeks, TimeUnit.Hours, "3 days 14 hours")]
	[InlineData(0.51428, TimeUnit.Weeks, TimeUnit.Minutes, "3 days 14 hours 24 minutes")]
	[InlineData(0.51428, TimeUnit.Weeks, TimeUnit.Days, "4 days")]
	[InlineData(0.51428, TimeUnit.Weeks, TimeUnit.Weeks, "1 week")]
	[InlineData(1.5, TimeUnit.Years, TimeUnit.Years, "2 years")]
	public void Humanize_Value_MatchesExpected(double value, TimeUnit timeUnit, TimeUnit? resolution, string expected)
		=> TimeSpanHumanizer.Humanize(value, timeUnit, resolution).Should().Be(expected);

	[Theory]
	[InlineData(0.51428, "weeks", null)]
	[InlineData(0.51428, "weeks", "hours")]
	[InlineData(0.51428, "weeks", "minutes")]
	[InlineData(0.51428, "weeks", "seconds")]
	[InlineData(0.51428, "weeks", "milliseconds")]
	[InlineData(400, "days", "years")]
	[InlineData(90, "seconds", "minutes")]
	[InlineData(-90, "seconds", "minutes")]
	public void Humanize_Value_MatchesTheNCalcFunction(double value, string timeUnit, string? resolution)
	{
		var expression = new ExtendedExpression(resolution is null
			? $"humanize(theValue, '{timeUnit}')"
			: $"humanize(theValue, '{timeUnit}', '{resolution}')");
		expression.Parameters["theValue"] = value;

		TimeSpanHumanizer.Humanize(
				value,
				Enum.Parse<TimeUnit>(timeUnit, true),
				resolution is null ? null : Enum.Parse<TimeUnit>(resolution, true))
			.Should().Be((string?)expression.Evaluate());
	}

	[Fact]
	public void Humanize_TimeSpan_WithoutResolution_TruncatesToWholeSeconds()
		=> TimeSpanHumanizer.Humanize(TimeSpan.FromSeconds(3661.9)).Should().Be("1 hour 1 minute 1 second");

	[Fact]
	public void Humanize_TimeSpan_WithResolution_Rounds()
		=> TimeSpanHumanizer.Humanize(TimeSpan.FromSeconds(3661.9), TimeUnit.Seconds).Should().Be("1 hour 1 minute 2 seconds");

	[Fact]
	public void Humanize_ValueTooLarge_ThrowsOverflowException()
		=> FluentActions.Invoking(() => TimeSpanHumanizer.Humanize(double.MaxValue, TimeUnit.Milliseconds))
			.Should().Throw<OverflowException>();

	[Fact]
	public void Humanize_RoundedValueTooLarge_ThrowsOverflowException()
		=> FluentActions.Invoking(() => TimeSpanHumanizer.Humanize(TimeSpan.MaxValue, TimeUnit.Hours))
			.Should().Throw<OverflowException>();

	[Fact]
	public void Humanize_UndefinedTimeUnit_ThrowsArgumentOutOfRangeException()
		=> FluentActions.Invoking(() => TimeSpanHumanizer.Humanize(1, (TimeUnit)99))
			.Should().Throw<ArgumentOutOfRangeException>();

	[Fact]
	public void Humanize_UndefinedResolution_ThrowsArgumentOutOfRangeException()
		=> FluentActions.Invoking(() => TimeSpanHumanizer.Humanize(1, TimeUnit.Days, (TimeUnit)99))
			.Should().Throw<ArgumentOutOfRangeException>();
}
