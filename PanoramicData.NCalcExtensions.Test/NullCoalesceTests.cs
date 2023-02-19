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
	public void Parse_GoodValues_Succeeds(string parameters, object? expectedValue)
	{
		var expression = new ExtendedExpression($"nullCoalesce({parameters})");
		var result = expression.Evaluate();
		result.Should().Be(expectedValue);
	}

	[Theory]
	[InlineData(null, null, null, null)]
	[InlineData("A", null, null, "A")]
	[InlineData("A", "B", "C", "A")]
	[InlineData(null, "B", null, null)]
	[InlineData(null, null, "C", null)]
	[InlineData(null, "B", "C", "B.C")]
	public void Parse_ComplexInput_Succeeds(object? a, object? b, object? c, string? expectedValue)
	{
		var expression = new ExtendedExpression($"nullCoalesce(a, if(!isNull(b) && !isNull(c), b + '.' + c, null))");
		expression.Parameters.Add("a", a);
		expression.Parameters.Add("b", b);
		expression.Parameters.Add("c", c);
		var result = expression.Evaluate();
		result.Should().Be(expectedValue);
	}
}
