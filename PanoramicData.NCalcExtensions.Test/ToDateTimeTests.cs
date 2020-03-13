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
	}
}
