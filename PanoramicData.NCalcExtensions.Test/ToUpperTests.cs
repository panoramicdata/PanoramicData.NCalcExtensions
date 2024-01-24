namespace PanoramicData.NCalcExtensions.Test;
public class ToUpperTests
{
	[Theory]
	[InlineData("PaNToMIMe", "PANTOMIME")]
	[InlineData("pantomime", "PANTOMIME")]
	[InlineData("tEsT", "TEST")]

	public void ToUpper_UsingInlineData_ResultMatchExpectedValue(string input, string expected)
	{
		var expression = new ExtendedExpression($"toUpper('{input}')");
		var result = expression.Evaluate();
		result.Should().Be(expected);
	}
}
