namespace PanoramicData.NCalcExtensions.Test;

public class NullCoalesceTests
{
	[Theory]
	[InlineData("'xxx', 1, null", "xxx")]
	[InlineData("1, 'xxx', null", 1)]
	[InlineData("null, 1, 'xxx'", 1)]
	[InlineData("null, 'xxx', 1", "xxx")]
	[InlineData("null, null, 1", 1)]
	[InlineData("null, null, null", null)]
	[InlineData("null", null)]
	[InlineData("1", 1)]
	[InlineData("", null)]
	public void Parse_IncorrectParameterCountOrType_Throws(string parameters, object? expectedValue)
	{
		var expression = new ExtendedExpression($"nullCoalesce({parameters})");
		var result = expression.Evaluate();
		result.Should().Be(expectedValue);
	}
}
