using System;
using Xunit;

namespace PanoramicData.NCalcExtensions.Test
{
	public class ToDateTimeTests : NCalcTest
	{
		[Fact]
		public void StandardConversionNonDst_Succeeds()
		{
			const string format = "yyyy-MM-dd HH:mm";
			var result = Test($"toDateTime('2020-02-29 12:00', '{format}')");
			Assert.Equal(new DateTime(2020, 02, 29, 12, 00, 00), result);
		}
		[Fact]
		public void SingleParameter_Fails()
		{
			var expression = new ExtendedExpression("toDateTime('2020-02-29 12:00')");
			Assert.Throws<ArgumentException>(() => expression.Evaluate());
		}

		[Fact]
		public void StandardConversionDst_Succeeds()
		{
			const string format = "yyyy-MM-dd HH:mm";
			var result = Test($"toDateTime('2020-06-06 12:00', '{format}')");
			Assert.Equal(new DateTime(2020, 06, 06, 12, 00, 00), result);
		}

		[Fact]
		public void TimeZoneConversion_Succeeds()
		{
			const string format = "yyyy-MM-dd HH:mm";
			var result = Test($"toDateTime('2020-02-29 12:00', '{format}', 'Eastern Standard Time')");
			Assert.Equal(new DateTime(2020, 02, 29, 17, 00, 00), result);
		}

		[Fact]
		public void TimeZoneDuringStupidTimeConversion_Succeeds()
		{
			const string format = "yyyy-MM-dd HH:mm";
			var result = Test($"toDateTime('2020-03-13 12:00', '{format}', 'Eastern Standard Time')");
			Assert.Equal(new DateTime(2020, 03, 13, 16, 00, 00), result);
		}

		[Fact]
		public void DateTimeFirstParameterWithTimeZone_Succeeds()
		{
			var estDateTime = new DateTime(2020, 03, 02, 12, 00, 00);
			var expression = new ExtendedExpression("toDateTime(estDateTime, 'Eastern Standard Time')");
			expression.Parameters[nameof(estDateTime)] = estDateTime;
			var utcDateTime = expression.Evaluate();
			Assert.Equal(new DateTime(2020, 03, 02, 17, 00, 00), utcDateTime);
		}

		[Fact]
		public void NullFirstParameterWithTimeZone_Succeeds()
		{
			object? estDateTime = null;
			var expression = new ExtendedExpression("toDateTime(estDateTime, 'Eastern Standard Time')");
			expression.Parameters[nameof(estDateTime)] = estDateTime;
			var utcDateTime = expression.Evaluate();
			Assert.Null(utcDateTime);
		}

		[Fact]
		public void DateTimeFirstParameterWithoutTimeZone_Fails()
		{
			var estDateTime = new DateTime(2020, 03, 02, 12, 00, 00);
			var expression = new ExtendedExpression("toDateTime(theDateTime)");
			expression.Parameters[nameof(estDateTime)] = estDateTime;
			Assert.Throws<ArgumentException>(() => expression.Evaluate());
		}
	}
}
