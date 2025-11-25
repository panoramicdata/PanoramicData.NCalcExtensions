namespace PanoramicData.NCalcExtensions.Test;

public class InTests
{
	[Theory]
	[InlineData("1,2,3,4", "1", true)]
	[InlineData("1,2,3,4", "2", true)]
	[InlineData("1,2,3,4", "3", true)]
	[InlineData("1,2,3,4", "4", true)]
	[InlineData("1,2,3,4", "5", false)]
	public void In_UsingInlineData_ResultMatchesExpectation(string stringList, string item, bool expected)
	{
		var expression = new ExtendedExpression($"in({item},{stringList})");
		var result = expression.Evaluate();
		result.Should().Be(expected);
	}

	[Theory]
	[InlineData("in('needle', 'hay', 'stack', 'needle', 'found')", true)]
	[InlineData("in('needle', 'hay', 'stack', 'only')", false)]
	[InlineData("in(3, 1, 2, 3, 4, 5)", true)]
	[InlineData("in(6, 1, 2, 3, 4, 5)", false)]
	[InlineData("in(null, 1, 2, null, 4)", true)]
	[InlineData("in(null, 1, 2, 3, 4)", false)]
	[InlineData("in('', 'a', '', 'b')", true)]
	[InlineData("in('first', 'first', 'second', 'third')", true)]
	[InlineData("in('last', 'first', 'second', 'last')", true)]
	[InlineData("in('item', 'item')", true)]
	[InlineData("in('searching', 'different')", false)]
	[InlineData("in('3', '1', '2', '3', '4')", true)]
	[InlineData("in(true, false, true, false)", true)]
	[InlineData("in(true, false, false, false)", false)]
	[InlineData("in('NEEDLE', 'needle', 'Needle', 'NeEdLe')", false)]
	[InlineData("in(3.14, 1.5, 2.7, 3.14, 4.2)", true)]
	[InlineData("in(0, 1, 0, 2, 3)", true)]
	[InlineData("in(-5, 1, -5, 3)", true)]
	[InlineData("in(50, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 40, 50, 60)", true)]
	[InlineData("in('x', 'x', 'x', 'x', 'x')", true)]
	[InlineData("in('!@#', 'abc', '!@#', 'xyz')", true)]
	[InlineData("in(' ', 'a', ' ', 'b')", true)]
	[InlineData("in(10, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15)", true)]
	[InlineData("in(1.5, 0.5, 1.0, 1.5, 2.0)", true)]
	public void In_VariousScenarios_ReturnsExpected(string expression, bool expected) => new ExtendedExpression(expression).Evaluate().Should().Be(expected);

	[Fact]
	public void In_WithVariable_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in(searchValue, 'apple', 'banana', 'cherry')");
		expression.Parameters["searchValue"] = "banana";
		expression.Evaluate().Should().Be(true);
	}

	[Theory]
	[InlineData("in()")]
	[InlineData("in('single')")]
	public void In_InvalidParameterCount_ThrowsException(string expression)
		=> new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*requires at least two parameters*");

	// Additional tests to improve coverage
	[Fact]
	public void In_TwoParameters_MinimumValid_Works()
	{
		var expression = new ExtendedExpression("in(1, 1)");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void In_ManyParameters_Works()
	{
		var expression = new ExtendedExpression("in(100, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 100)");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void In_WithJObjects_Works()
	{
		// JObjects are compared by reference, not value
		var expression = new ExtendedExpression("in(searchValue, jObject('a', 2), searchValue)");
		expression.Parameters["searchValue"] = JObject.Parse("{\"a\": 1}");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void In_WithMixedTypes_Works()
	{
		var expression = new ExtendedExpression("in(1, '1', 1.0, 1)");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void In_CaseSensitiveStrings_Works()
	{
		var expression = new ExtendedExpression("in('Test', 'test', 'TEST', 'TeSt')");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void In_WithBooleans_Works()
	{
		var expression = new ExtendedExpression("in(false, true, true, false)");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void In_WithDoubles_Works()
	{
		var expression = new ExtendedExpression("in(2.5, 1.5, 2.5, 3.5)");
		expression.Evaluate().Should().Be(true);
	}
}
