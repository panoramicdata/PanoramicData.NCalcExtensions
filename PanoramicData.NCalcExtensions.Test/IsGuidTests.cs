namespace PanoramicData.NCalcExtensions.Test;

public class IsGuidTests
{
	[Theory]
	[InlineData(null, false)]
	[InlineData(1, false)]
	[InlineData(true, false)]
	[InlineData("abc", false)]
	[InlineData("9384EF0Z-38AD-4E8E-A24E-0ACD3273A401", false)]
	[InlineData("{9384EF0Z-38AD-4E8E-A24E-0ACD3273A401}", false)]
	[InlineData("9384EF0A-38AD-4E8E-A24E-0ACD3273A401", true)]
	[InlineData("{9384EF0A-38AD-4E8E-A24E-0ACD3273A401}", true)]
	public void IsGuid_ParameterInputs_ReturnsExpectedResult(object? parameterValue, bool expectedResult)
	{
		var expression = new ExtendedExpression("isGuid(parameter1)");
		expression.Parameters["parameter1"] = parameterValue;
		(expression.Evaluate() as bool?).Should().Be(expectedResult);
	}
}
