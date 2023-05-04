using System.Linq;

namespace PanoramicData.NCalcExtensions.Test;

public class MinTests
{
	[Theory]
	[InlineData("1, 2, 3", 1)]
	[InlineData("3, 2, 1", 1)]
	[InlineData("1, 3, 2", 1)]
	[InlineData("1, 1, 1", 1)]
	[InlineData("1, 1, 2", 1)]
	[InlineData("1, null, 2", 1)]
	[InlineData("1.1, null, 2", 1.1)]
	[InlineData("null, null, null", null)]

	public void Min_OfNumbers_ReturnsExpectedValue(string values, object expectedOutput)
	{
		var expression = new ExtendedExpression($"min(listOf('double?', {values}), 'x', 'x')");
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("'1', '2', '3'", "1")]
	[InlineData("'3', '2', '1'", "1")]
	[InlineData("'1', '3', null", "1")]
	[InlineData("'abc', 'raf', 'bbc'", "abc")]
	[InlineData("'abc', 'ABC', null", "abc")]

	public void Min_OfStrings_ReturnsExpectedValue(string values, string expectedOutput)
	{
		var expression = new ExtendedExpression($"min(list({values}))");
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("1,2,3", "1")]
	[InlineData("3,2,1", "1")]
	[InlineData("1,3,null", "1")]
	[InlineData("abc,raf,bbc", "abc")]
	[InlineData("abc,ABC,null", "abc")]

	public void Min_OfStringsAsVariable_ReturnsExpectedValue(string values, string expectedOutput)
	{
		var expression = new ExtendedExpression($"min(valuesList)");
		expression.Parameters["valuesList"] = values.Split(',').Select(x => x == "null" ? null : x).ToList();
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Fact]

	public void Min_OfNull_ReturnsExpectedValue()
	{
		var expression = new ExtendedExpression($"min(null)");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void Min_OfEmptyList_ReturnsNull()
	{
		var expression = new ExtendedExpression($"min(list())");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void Max_UsingLambdaForInt_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"min(listOf('int', 1, 2, 3), 'x', 'x + 1')");
		expression.Evaluate().Should().Be(2);
	}

	[Fact]
	public void Max_UsingLambdaForString_ReturnsExpected()
	{
		var expression = new ExtendedExpression("min(listOf('string', '1', '2', '3'), 'x', 'x + x')");
		expression.Evaluate().Should().Be("11");
	}
}