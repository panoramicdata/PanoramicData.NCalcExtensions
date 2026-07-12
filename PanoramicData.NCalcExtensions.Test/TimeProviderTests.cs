namespace PanoramicData.NCalcExtensions.Test;

/// <summary>
/// Verifies that an injected <see cref="TimeProvider"/> makes the time-dependent functions
/// (now, dateTimeIsInPast, dateTimeIsInFuture) evaluate deterministically, and that the
/// default (no provider supplied) behaviour continues to use the live system clock.
/// </summary>
public class TimeProviderTests
{
	// 2026-07-10T03:20:00Z - the out-of-hours instant from the motivating incident (PS-2787).
	private static readonly DateTimeOffset FixedInstant = new(2026, 7, 10, 3, 20, 0, TimeSpan.Zero);

	private static ExtendedExpression Build(string expression, TimeProvider? timeProvider)
		=> new(expression, ExpressionOptions.None, CultureInfo.InvariantCulture, timeProvider);

	[Fact]
	public void Now_Utc_WithFixedProvider_ReturnsFixedInstant()
	{
		var result = Build("now('UTC')", new FixedTimeProvider(FixedInstant)).Evaluate();

		result.Should().BeOfType<DateTime>();
		var dateTime = (DateTime)result!;
		dateTime.Should().Be(new DateTime(2026, 7, 10, 3, 20, 0, DateTimeKind.Utc));
		dateTime.Kind.Should().Be(DateTimeKind.Utc);
	}

	[Fact]
	public void Now_NoTimeZone_WithFixedProvider_ReturnsFixedInstantAsUtc()
	{
		var result = Build("now()", new FixedTimeProvider(FixedInstant)).Evaluate();

		result.Should().BeOfType<DateTime>();
		((DateTime)result!).Should().Be(new DateTime(2026, 7, 10, 3, 20, 0, DateTimeKind.Utc));
	}

	[Fact]
	public void Now_CentralStandardTime_WithFixedProvider_ReturnsDstCorrectLocalTime()
	{
		// Central Time in July observes daylight saving (CDT, UTC-5), so 03:20Z is 22:20 the previous day.
		var result = Build("now('Central Standard Time')", new FixedTimeProvider(FixedInstant)).Evaluate();

		result.Should().BeOfType<DateTime>();
		var dateTime = (DateTime)result!;
		dateTime.Should().Be(new DateTime(2026, 7, 9, 22, 20, 0));
		dateTime.Kind.Should().Be(DateTimeKind.Unspecified);
	}

	[Fact]
	public void Now_NoProvider_UsesLiveSystemClock()
	{
		var before = DateTime.UtcNow;
		var result = Build("now('UTC')", timeProvider: null).Evaluate();
		var after = DateTime.UtcNow;

		result.Should().BeOfType<DateTime>();
		var dateTime = (DateTime)result!;
		dateTime.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
	}

	[Fact]
	public void DateTimeIsInPast_WithFixedProvider_IsDeterministic()
	{
		var expression = Build("dateTimeIsInPast(valueUnderTest)", new FixedTimeProvider(FixedInstant));
		expression.Parameters.Add("valueUnderTest", new DateTime(2026, 7, 10, 3, 0, 0, DateTimeKind.Utc));

		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void DateTimeIsInPast_FutureValueWithFixedProvider_ReturnsFalse()
	{
		var expression = Build("dateTimeIsInPast(valueUnderTest)", new FixedTimeProvider(FixedInstant));
		expression.Parameters.Add("valueUnderTest", new DateTime(2026, 7, 10, 4, 0, 0, DateTimeKind.Utc));

		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void DateTimeIsInFuture_WithFixedProvider_IsDeterministic()
	{
		var expression = Build("dateTimeIsInFuture(valueUnderTest)", new FixedTimeProvider(FixedInstant));
		expression.Parameters.Add("valueUnderTest", new DateTime(2026, 7, 10, 4, 0, 0, DateTimeKind.Utc));

		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void DateTimeIsInFuture_PastValueWithFixedProvider_ReturnsFalse()
	{
		var expression = Build("dateTimeIsInFuture(valueUnderTest)", new FixedTimeProvider(FixedInstant));
		expression.Parameters.Add("valueUnderTest", new DateTime(2026, 7, 10, 3, 0, 0, DateTimeKind.Utc));

		expression.Evaluate().Should().Be(false);
	}

	/// <summary>
	/// A minimal <see cref="TimeProvider"/> that always reports a fixed instant, avoiding a
	/// dependency on Microsoft.Extensions.TimeProvider.Testing for these tests.
	/// </summary>
	private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
	{
		public override DateTimeOffset GetUtcNow() => utcNow;
	}
}
