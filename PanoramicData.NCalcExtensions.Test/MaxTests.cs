using System.Linq;

namespace PanoramicData.NCalcExtensions.Test;

public class MaxTests
{
	[Theory]
	[InlineData("1, 2, 3", 3)]
	[InlineData("3, 2, 1", 3)]
	[InlineData("1, 3, 2", 3)]
	[InlineData("1, 1, 1", 1)]
	[InlineData("1, 1, 2", 2)]
	[InlineData("1, null, 2", 2)]
	[InlineData("1.1, null, 2", 2)]
	[InlineData("null, null, null", null)]

	public void Max_OfNumbers_ReturnsExpectedValue(string values, object expectedOutput)
	{
		var expression = new ExtendedExpression($"max(listOf('double?', {values}), 'x', 'x')");
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("'1', '2', '3'", "3")]
	[InlineData("'3', '2', '1'", "3")]
	[InlineData("'1', '3', null", "3")]
	[InlineData("'abc', 'raf', 'bbc'", "raf")]
	[InlineData("'abc', 'ABC', null", "ABC")]

	public void Max_OfStrings_ReturnsExpectedValue(string values, string expectedOutput)
	{
		var expression = new ExtendedExpression($"max(list({values}))");
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("1,2,3", "3")]
	[InlineData("3,2,1", "3")]
	[InlineData("1,3,null", "3")]
	[InlineData("abc,raf,bbc", "raf")]
	[InlineData("abc,ABC,null", "ABC")]

	public void Max_OfStringsAsVariable_ReturnsExpectedValue(string values, string expectedOutput)
	{
		ArgumentException.ThrowIfNullOrEmpty(values, nameof(values));

		var expression = new ExtendedExpression($"max(valuesList)");
		expression.Parameters["valuesList"] = values.Split(',').Select(x => x == "null" ? null : x).ToList();
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Fact]

	public void Max_OfNull_ReturnsExpectedValue()
	{
		var expression = new ExtendedExpression($"max(null)");
		expression.Evaluate().Should().BeNull();
	}


	[Fact]
	public void Max_OfEmptyList_ReturnsNull()
	{
		var expression = new ExtendedExpression($"max(list())");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void Max_UsingLambdaForInt_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"max(listOf('int', 1, 2, 3), 'x', 'x + 1')");
		expression.Evaluate().Should().Be(4);
	}

	[Fact]
	public void Max_UsingLambdaForString_ReturnsExpected()
	{
		var expression = new ExtendedExpression("max(listOf('string', '1', '2', '3'), 'x', 'x + x')");
		expression.Evaluate().Should().Be("33");
	}
}
