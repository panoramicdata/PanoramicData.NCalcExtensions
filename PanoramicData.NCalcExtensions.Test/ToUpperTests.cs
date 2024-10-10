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

public class TrimTests
{
	[Theory]
	[InlineData(" ", " ")]
	[InlineData("\r", "")]
	[InlineData("", "\r")]
	[InlineData("\t", "\r")]
	[InlineData(" \n", "\r ")]

	public void Trim_UsingInlineData_ResultMatchExpectedValue(string prefix, string suffix)
	{
		var input = $"{prefix}test{suffix}";
		var expression = new ExtendedExpression($"trim(x)");
		expression.Parameters.Add("x", input);
		var result = expression.Evaluate();
		result.Should().Be("test");
	}
}
