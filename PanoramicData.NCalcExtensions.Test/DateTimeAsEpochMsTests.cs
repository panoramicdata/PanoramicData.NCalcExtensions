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

		[Fact]
		public void WeirdShit_Succeeds()
		{
			var expression = new ExtendedExpression("1 > dateTimeAsEpochMs([connectMagic.systemItem.sys_updated_on], 'yyyy-MM-dd HH:mm:ss')");
			expression.Parameters.Add("connectMagic.systemItem.sys_updated_on", "2018-01-01 01:01:01");
			var result = expression.Evaluate();
			const long desiredDateTimeEpochMs = 1514768461000;
			Assert.Equal(false, result);
		}
	}
}
