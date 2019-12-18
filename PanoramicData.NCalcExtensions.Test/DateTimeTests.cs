using System;
using System.Globalization;
using Xunit;

namespace PanoramicData.NCalcExtensions.Test
{
	public class DateTimeTests : NCalcTest
	{
		[Fact]
		public void AllParameters_Succeeds()
		{
			const string format = "yyyy-MM-dd HH:mm";
			var result = Test($"dateTime('UTC', '{format}', -90, 0, 0, 0)");
			var desiredDateTime = DateTime.UtcNow.AddDays(-90);
			Assert.Equal(desiredDateTime.ToString(format, CultureInfo.InvariantCulture), result);
		}
	}
}
