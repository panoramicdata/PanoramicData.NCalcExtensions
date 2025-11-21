namespace PanoramicData.NCalcExtensions.Test;

public class ParseIntTests
{
	[Theory]
	[InlineData("'1'", 1)]
	[InlineData("'-42'", -42)]
	[InlineData("'2147483647'", int.MaxValue)]
	[InlineData("'0'", 0)]
	[InlineData("'-2147483648'", int.MinValue)]
	[InlineData("'  42  '", 42)]
	[InlineData("'00042'", 42)]
	[InlineData("'+42'", 42)]
	public void ParseInt_ValidInput_ReturnsExpectedValue(string input, int expected)
	{
		var expression = new ExtendedExpression($"parseInt({input})");
		expression.Evaluate().Should().Be(expected);
	}

	[Theory]
	[InlineData("parseInt()")]
	[InlineData("parseInt('1', '2')")]
	[InlineData("parseInt(1)")]
	[InlineData("parseInt('not a number')")]
	[InlineData("parseInt('2147483648')")]
	[InlineData("parseInt('-2147483649')")]
	[InlineData("parseInt('')")]
	[InlineData("parseInt('3.14')")]
	[InlineData("parseInt('1,234')")]
	[InlineData("parseInt('0xFF')")]
	[InlineData("parseInt(null)")]
	public void ParseInt_InvalidInput_ThrowsException(string expression) => new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();

	[Fact]
	public void ParseInt_WithVariable_Succeeds()
	{
		var expression = new ExtendedExpression("parseInt(myValue)");
		expression.Parameters["myValue"] = "123";
		expression.Evaluate().Should().Be(123);
	}

	[Fact]
	public void ParseInt_InExpression_Works()
	{
		var expression = new ExtendedExpression("parseInt('10') + parseInt('20')");
		expression.Evaluate().Should().Be(30);
	}

	[Fact]
	public void ParseInt_InComparison_Works()
	{
		var expression = new ExtendedExpression("parseInt('42') > 40");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void ParseInt_InList_Works()
	{
		var expression = new ExtendedExpression("list(parseInt('1'), parseInt('2'), parseInt('3'))");
		var result = expression.Evaluate() as System.Collections.Generic.List<object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
	}
}
