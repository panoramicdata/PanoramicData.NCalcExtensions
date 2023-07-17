namespace PanoramicData.NCalcExtensions.Test;
public class ToUpperTests
{
	[Theory]
	[InlineData("PaNToMIMe", "PANTOMIME")]
	[InlineData("pantomime", "PANTOMIME")]
	[InlineData("tEsT", "TEST")]

	public void ToUpper_Succeeds(string input, string expected)
	{
		var expression = new ExtendedExpression($"toUpper('{input}')");
		expression.Evaluate().Should().Be(expected);
	}
}
