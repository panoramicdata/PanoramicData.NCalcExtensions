using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class AnyTests : NCalcTest
{
	[Theory]
	[InlineData("1, 2, 3", true)]
	[InlineData("7, 8, 9", false)]

	public void Any_LessThanFive_ResultMatchesExpectation(string stringList, bool expected)
	{
		var expression = new ExtendedExpression($"any(list({stringList}), 'n', 'n < 5')");

		var result = expression.Evaluate();

		result.Should().Be(expected);
	}

	[Fact]
	public void Any_Items_ReturnsTrue()
	{
		var expression = new ExtendedExpression($"any(list(1, 2, 3))");
		var result = expression.Evaluate();
		result.Should().Be(true);
	}

	[Fact]
	public void Any_NoItems_ReturnsFalse()
	{
		var expression = new ExtendedExpression($"any(list())");
		var result = expression.Evaluate();
		result.Should().Be(false);
	}

	[Theory]
	[InlineData("null")]
	[InlineData("'a'")]
	[InlineData("list(1), 1")]
	[InlineData("list(1), null, 1")]
	[InlineData("list(1), 'a', 1")]
	[InlineData("list(1), 'a', null")]
	[InlineData("1, 2")]
	[InlineData("1, 2, 3, 4")]
	public void Any_BadParameters_ThrowsException(string parametersString)
		=> new ExtendedExpression($"any({parametersString})").Invoking(x => x.Evaluate()).Should().ThrowExactly<FormatException>();

	// Additional comprehensive tests
	[Fact]
	public void Any_EmptyList_ReturnsFalse()
	{
		var expression = new ExtendedExpression("any(list(), 'n', 'n > 0')");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void Any_OneMatches_ReturnsTrue()
	{
		var expression = new ExtendedExpression("any(list(1, 3, 5, 6), 'n', 'n % 2 == 0')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_AllMatch_ReturnsTrue()
	{
		var expression = new ExtendedExpression("any(list(2, 4, 6, 8), 'n', 'n % 2 == 0')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_NoneMatch_ReturnsFalse()
	{
		var expression = new ExtendedExpression("any(list(1, 3, 5, 7), 'n', 'n % 2 == 0')");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void Any_Strings_Works()
	{
		var expression = new ExtendedExpression("any(list('cat', 'dog', 'bird'), 's', 'startsWith(s, \"d\")')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_Strings_NoneMatch()
	{
		var expression = new ExtendedExpression("any(list('cat', 'rat', 'bat'), 's', 'startsWith(s, \"d\")')");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void Any_GreaterThan_Works()
	{
		var expression = new ExtendedExpression("any(list(1, 2, 10), 'n', 'n > 5')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_LessThanOrEqual_Works()
	{
		var expression = new ExtendedExpression("any(list(10, 20, 5), 'n', 'n <= 5')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_ComplexCondition_Works()
	{
		var expression = new ExtendedExpression("any(list(5, 15, 25), 'n', 'n >= 10 && n <= 20')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_WithVariables_Works()
	{
		var expression = new ExtendedExpression("any(myList, 'n', 'n < 0')");
		expression.Parameters["myList"] = new List<object> { 1, 2, -3, 4, 5 };
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_SingleItemTrue_ReturnsTrue()
	{
		var expression = new ExtendedExpression("any(list(5), 'n', 'n > 0')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_SingleItemFalse_ReturnsFalse()
	{
		var expression = new ExtendedExpression("any(list(-5), 'n', 'n > 0')");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void Any_Doubles_Works()
	{
		var expression = new ExtendedExpression("any(list(1.1, 1.2, 2.5), 'n', 'n > 2.0')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_NegativeNumbers_Works()
	{
		var expression = new ExtendedExpression("any(list(10, 20, -5), 'n', 'n < 0')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_WithNulls_HandlesNull()
	{
		var expression = new ExtendedExpression("any(list(1, null, 3), 'n', 'n == null')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_Booleans_True()
	{
		var expression = new ExtendedExpression("any(list(false, false, true))");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Any_Booleans_False()
	{
		var expression = new ExtendedExpression("any(list(false, false, false))");
		// Any with no predicate just checks if list has items
		expression.Evaluate().Should().Be(true);
	}
}
