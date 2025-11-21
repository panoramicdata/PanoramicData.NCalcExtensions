using System.Collections.Generic;
using System.Linq;

namespace PanoramicData.NCalcExtensions.Test;

public class CountTests
{
	private readonly List<string> _stringList = ["a", "b", "c"];

	[Theory]
	[InlineData("split('a piece of string', ' ')", 4)]
	[InlineData("'a piece of string'", 17)]
	[InlineData("list(1,2,3,4,5)", 5)]
	[InlineData("list()", 0)]
	public void Count_VariousInputs_ReturnsExpected(string input, int expected)
	{
		var expression = new ExtendedExpression($"count({input})");
		expression.Evaluate().Should().Be(expected);
	}

	[Fact]
	public void Count_OfListOfString_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"count(x)");
		expression.Parameters.Add("x", _stringList);
		var result = expression.Evaluate();
		result.Should().Be(_stringList.Count);
	}

	[Fact]
	public void Count_WithLambda_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"count(list(1,2,3), 'n', 'n < 3')");
		var result = expression.Evaluate();
		result.Should().Be(2);
	}

	[Fact]
	public void Count_OfEnumerableOfString_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"count(x)");
		expression.Parameters.Add("x", _stringList.AsEnumerable());
		var result = expression.Evaluate();
		result.Should().Be(_stringList.Count);
	}

	[Fact]
	public void Count_OfReadOnlyOfString_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"count(x)");
		expression.Parameters.Add("x", _stringList.AsReadOnly());
		var result = expression.Evaluate();
		result.Should().Be(_stringList.Count);
	}

	[Fact]
	public void Count_JArray_ReturnsExpectedResult()
	{
		var expression = new ExtendedExpression($"count(x)");
		expression.Parameters.Add("x", JArray.FromObject(_stringList));
		var result = expression.Evaluate();
		result.Should().Be(_stringList.Count);
	}
}
