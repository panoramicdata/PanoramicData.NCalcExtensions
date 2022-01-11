namespace PanoramicData.NCalcExtensions.Test;

public class ToDateTimeTests : NCalcTest
{
	[Fact]
	public void StandardConversionNonDst_Succeeds()
	{
		const string format = "yyyy-MM-dd HH:mm";
		var result = Test($"toDateTime('2020-02-29 12:00', '{format}')");
		result.Should().Be(new DateTime(2020, 02, 29, 12, 00, 00));
	}

	[Fact]
	public void SingleParameter_Fails()
	{
		var expression = new ExtendedExpression("toDateTime('2020-02-29 12:00')");
		AssertionExtensions
			.Should(() => { expression.Evaluate(); })
			.Throw<ArgumentException>();
	}

	[Fact]
	public void StandardConversionDst_Succeeds()
	{
		const string format = "yyyy-MM-dd HH:mm";
		var result = Test($"toDateTime('2020-06-06 12:00', '{format}')");
		result.Should().Be(new DateTime(2020, 06, 06, 12, 00, 00));
	}

	[Fact]
	public void TimeZoneConversion_Succeeds()
	{
		const string format = "yyyy-MM-dd HH:mm";
		var result = Test($"toDateTime('2020-02-29 12:00', '{format}', 'Eastern Standard Time')");
		result.Should().Be(new DateTime(2020, 02, 29, 17, 00, 00));
	}

	[Fact]
	public void TimeZoneDuringStupidTimeConversion_Succeeds()
	{
		const string format = "yyyy-MM-dd HH:mm";
		var result = Test($"toDateTime('2020-03-13 12:00', '{format}', 'Eastern Standard Time')");
		result.Should().Be(new DateTime(2020, 03, 13, 16, 00, 00));
	}

	[Fact]
	public void DateTimeFirstParameterWithTimeZone_Succeeds()
	{
		var estDateTime = new DateTime(2020, 03, 02, 12, 00, 00);
		var expression = new ExtendedExpression("toDateTime(estDateTime, 'Eastern Standard Time')");
		expression.Parameters[nameof(estDateTime)] = estDateTime;
		var utcDateTime = expression.Evaluate();
		utcDateTime.Should().Be(new DateTime(2020, 03, 02, 17, 00, 00));
	}

	[Fact]
	public void NullFirstParameterWithTimeZone_Succeeds()
	{
		object? estDateTime = null;
		var expression = new ExtendedExpression("toDateTime(estDateTime, 'Eastern Standard Time')");
		expression.Parameters[nameof(estDateTime)] = estDateTime;
		var utcDateTime = expression.Evaluate();
		utcDateTime.Should().BeNull();
	}

	[Fact]
	public void DateTimeFirstParameterWithoutTimeZone_Fails()
	{
		var estDateTime = new DateTime(2020, 03, 02, 12, 00, 00);
		var expression = new ExtendedExpression("toDateTime(theDateTime)");
		expression.Parameters[nameof(estDateTime)] = estDateTime;
		AssertionExtensions
			.Should(() => { expression.Evaluate(); })
			.Throw<ArgumentException>();
	}

	[Fact]
	public void DateTimeIntSeconds_Succeeds()
	{
		var expectedDateTime = new DateTime(1975, 02, 17, 00, 00, 00);
		var expression = new ExtendedExpression("toDateTime(161827200.0, 's', 'UTC')");
		expression.Parameters[nameof(expectedDateTime)] = expectedDateTime;
		expression.Evaluate().Should().Be(expectedDateTime);
	}

	[Fact]
	public void DateTimeLongMilliseconds_Succeeds()
	{
		var expectedDateTime = new DateTime(1975, 02, 17, 00, 00, 00);
		var expression = new ExtendedExpression("toDateTime(161827200000.0, 'ms', 'UTC')");
		expression.Parameters[nameof(expectedDateTime)] = expectedDateTime;
		expression.Evaluate().Should().Be(expectedDateTime);
	}

	[Fact]
	public void DateTimeLongMicroseconds_Succeeds()
	{
		var expectedDateTime = new DateTime(1975, 02, 17, 00, 00, 00);
		var expression = new ExtendedExpression("toDateTime(161827200000000.0, 'us', 'UTC')");
		expression.Parameters[nameof(expectedDateTime)] = expectedDateTime;
		expression.Evaluate().Should().Be(expectedDateTime);
	}
}
