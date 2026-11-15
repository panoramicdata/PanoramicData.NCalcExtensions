namespace PanoramicData.NCalcExtensions.Test;

/// <summary>
/// Behaviour shared by dateTimeIsInFuture and dateTimeIsInPast, which are mirror images.
/// </summary>
public abstract class DateTimeDirectionTests(string functionName, bool isFuture) : NCalcTest
{
	private const string _luanda = "'Africa/Luanda'";
	private const string _isoFormat = "'yyyy-MM-ddTHH:mm:ss'";

	/// <summary>The answer for a moment that is in the future of now.</summary>
	private bool Expected(bool momentIsInTheFuture) => momentIsInTheFuture == isFuture;

	private object? Evaluate(string arguments, DateTime? value = null)
		=> value is null
			? Test($"{functionName}({arguments})")
			: Test($"{functionName}(valueUnderTest{arguments})", "valueUnderTest", value);

	[Theory]
	[InlineData(2000, true)]
	[InlineData(-2000, false)]
	public void Evaluate_UtcDateAroundNow_ReturnsExpected(int offsetMilliseconds, bool inTheFuture)
		=> Evaluate(string.Empty, DateTime.UtcNow.AddMilliseconds(offsetMilliseconds)).Should().Be(Expected(inTheFuture));

	// West Africa Time does not observe Daylight Saving, so its offset from UTC is constant
	[Theory]
	[InlineData(2000, true)]
	[InlineData(-2000, false)]
	public void Evaluate_WestAfricaTimeAroundNow_ReturnsExpected(int offsetMilliseconds, bool inTheFuture)
		=> Evaluate($", {_luanda}", DateTime.UtcNow.AddHours(1).AddMilliseconds(offsetMilliseconds)).Should().Be(Expected(inTheFuture));

	[Fact]
	public void Evaluate_UtcNowInComparisonToWestAfricaTime_IsInThePast()
		=> Evaluate($"now(), {_luanda}").Should().Be(Expected(false));

	[Fact]
	public void Evaluate_CentralEuropeanTime_UtcNowMasqueradingAsCetIsInThePast()
		=> Evaluate(", 'Central European Standard Time'", DateTime.UtcNow).Should().Be(Expected(false));

	[Theory]
	[InlineData("2001-01-01T00:00:00", false)]
	[InlineData("2201-01-01T00:00:00", true)]
	public void Evaluate_FixedDate_ReturnsExpected(string isoDate, bool inTheFuture)
		=> Evaluate($"toDateTime('{isoDate}', {_isoFormat})").Should().Be(Expected(inTheFuture));

	[Fact]
	public void Evaluate_VeryOldDate_IsInThePast()
		=> Evaluate(string.Empty, new DateTime(1900, 1, 1)).Should().Be(Expected(false));

	[Fact]
	public void Evaluate_DateTimeMax_IsInTheFuture()
		=> Evaluate(string.Empty, DateTime.MaxValue).Should().Be(Expected(true));

	[Theory]
	[InlineData("()")]
	[InlineData("(null)")]
	public void InvalidParameters_ThrowsException(string arguments)
		=> TestShouldThrow<FormatException>(functionName + arguments, "*");

	[Theory]
	[InlineData(", 123", "*second argument should be a string*")]
	[InlineData(", 'Invalid/Timezone'", "*timezone was not a recognized*")]
	public void InvalidTimezone_ThrowsException(string arguments, string messagePattern)
		=> TestShouldThrow<FormatException>($"{functionName}(valueUnderTest{arguments})", "valueUnderTest", DateTime.UtcNow, messagePattern);
}
