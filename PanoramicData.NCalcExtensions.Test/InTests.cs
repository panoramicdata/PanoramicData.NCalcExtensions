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

	// Additional comprehensive tests

	[Fact]
	public void In_StringInList_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in('needle', 'hay', 'stack', 'needle', 'found')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void In_StringNotInList_ReturnsFalse()
	{
		var expression = new ExtendedExpression("in('needle', 'hay', 'stack', 'only')");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void In_NumberInList_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in(3, 1, 2, 3, 4, 5)");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void In_NumberNotInList_ReturnsFalse()
	{
		var expression = new ExtendedExpression("in(6, 1, 2, 3, 4, 5)");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void In_NullInListWithNull_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in(null, 1, 2, null, 4)");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void In_NullInListWithoutNull_ReturnsFalse()
	{
		var expression = new ExtendedExpression("in(null, 1, 2, 3, 4)");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void In_EmptyStringInList_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in('', 'a', '', 'b')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void In_FirstItemMatches_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in('first', 'first', 'second', 'third')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void In_LastItemMatches_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in('last', 'first', 'second', 'last')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void In_SingleItemList_Match_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in('item', 'item')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void In_SingleItemList_NoMatch_ReturnsFalse()
	{
		var expression = new ExtendedExpression("in('searching', 'different')");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void In_MixedTypes_StringAndNumbers_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in('3', '1', '2', '3', '4')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void In_BooleanValue_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in(true, false, true, false)");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void In_BooleanValue_ReturnsFalse()
	{
		var expression = new ExtendedExpression("in(true, false, false, false)");
		expression.Evaluate().Should().Be(false);
	}

	[Theory]
	[InlineData("in()")]
	public void In_NoParameters_ThrowsException(string expression)
		=> new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	[Fact]
	public void In_OneParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("in('single')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*requires at least two parameters*");
	}

	[Fact]
	public void In_CaseSensitive_NoMatch_ReturnsFalse()
	{
		var expression = new ExtendedExpression("in('NEEDLE', 'needle', 'Needle', 'NeEdLe')");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void In_WithVariable_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in(searchValue, 'apple', 'banana', 'cherry')");
		expression.Parameters["searchValue"] = "banana";
		expression.Evaluate().Should().Be(true);
	}
}
