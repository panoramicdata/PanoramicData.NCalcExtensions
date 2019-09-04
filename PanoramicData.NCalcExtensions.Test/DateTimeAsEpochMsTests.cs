using Xunit;

namespace PanoramicData.NCalcExtensions.Test
{
	public class DateTimeAsEpochMsTests : NCalcTest
	{
		[Fact]
		public void AllParameters_Succeeds()
		{
			var result = Test("dateTimeAsEpochMs('20190702T000000', 'yyyyMMddTHHmmssK')");
			const long desiredDateTimeEpochMs = 1562025600000;
			Assert.Equal(desiredDateTimeEpochMs, result);
		}
	}
}
