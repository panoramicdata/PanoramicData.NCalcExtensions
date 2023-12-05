namespace PanoramicData.NCalcExtensions.Test;

public class DateTimeAsEpochTests : NCalcTest
{
	[Fact]
	public void DateTimeAsEpoch_ValidParameters_CalculatesCorrectValue()
	{
		var result = Test("dateTimeAsEpoch('20190702T000000', 'yyyyMMddTHHmmssK')");
		const long expectedDateTimeEpoch = 1562025600;
		Assert.Equal(expectedDateTimeEpoch, result);
	}

	[Fact]
	public void DateTimeAsEpoch_ExpressionWithSquareBrackets_SuccessfullyInsertsParameter()
	{
		var expression = new ExtendedExpression("1 > dateTimeAsEpoch([connectMagic.systemItem.sys_updated_on], 'yyyy-MM-dd HH:mm:ss')");
		expression.Parameters.Add("connectMagic.systemItem.sys_updated_on", "2018-01-01 01:01:01");
		var result = expression.Evaluate();
		Assert.Equal(false, result);
	}
}
