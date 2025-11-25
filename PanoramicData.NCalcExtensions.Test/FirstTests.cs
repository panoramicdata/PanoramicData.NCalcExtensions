namespace PanoramicData.NCalcExtensions.Test;

public class FirstTests
{
	[Fact]
	public void First_Succeeds()
	{
		var expression = new ExtendedExpression("first(list(1, 5, 2, 3, 4, 1), 'n', 'n % 2 == 0')");
		var result = expression.Evaluate() as int?;

		result.Should().Be(2);
	}

	[Fact]
	public void First_SplittingString_Succeeds()
	{
		var expression = new ExtendedExpression("first(split('a b c', ' '))");
		var result = expression.Evaluate() as string;

		result.Should().Be("a");
	}

	[Fact]
	public void FirstFromStored_Succeeds()
	{
		var expression = new ExtendedExpression("store('x', list(1, 5, 2, 3)) && first(retrieve('x'), 'n', 'n % 2 == 0') == 2");

		expression
			.Evaluate()
			.Should()
			.Be(true);
	}

	[Fact]
	public void First_NoMatchingItem_ThrowsException()
	{
		var expression = new ExtendedExpression("first(list(1, 5, 7, 3), 'n', 'n % 2 == 0')");

		expression
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("No matching element found.");
	}

	// Error case tests
	[Fact]
	public void First_NullFirstParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("first(null, 'n', 'n > 0')");
		expression.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*First*must be an IEnumerable*");
	}

	[Fact]
	public void First_NonListFirstParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("first(123, 'n', 'n > 0')");
		expression.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*First*must be an IEnumerable*");
	}

	[Fact]
	public void First_NullSecondParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("first(list(1,2,3), null, 'n > 0')");
		expression.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*Second*must be a string*");
	}

	[Fact]
	public void First_NullThirdParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("first(list(1,2,3), 'n', null)");
		expression.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*Third*must be a string*");
	}

	// Edge case tests
	[Fact]
	public void First_OneParameter_SingleElement_ReturnsElement()
	{
		var expression = new ExtendedExpression("first(list(42))");
		expression.Evaluate().Should().Be(42);
	}

	[Fact]
	public void First_OneParameter_MultipleElements_ReturnsFirst()
	{
		var expression = new ExtendedExpression("first(list(10, 20, 30))");
		expression.Evaluate().Should().Be(10);
	}

	[Fact]
	public void First_EmptyList_ThrowsException()
	{
		var expression = new ExtendedExpression("first(list())");
		expression.Invoking(e => e.Evaluate())
			.Should()
			.Throw<Exception>();
	}

	// Various data type tests
	[Fact]
	public void First_Strings_WithPredicate_ReturnsMatch()
	{
		var expression = new ExtendedExpression("first(list('apple', 'banana', 'cherry'), 's', 'startsWith(s, \"b\")')");
		expression.Evaluate().Should().Be("banana");
	}

	[Fact]
	public void First_Strings_NoMatch_ThrowsException()
	{
		var expression = new ExtendedExpression("first(list('apple', 'banana', 'cherry'), 's', 'startsWith(s, \"z\")')");
		expression.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("No matching element found.");
	}

	[Fact]
	public void First_Numbers_GreaterThan_ReturnsFirst()
	{
		var expression = new ExtendedExpression("first(list(1, 5, 10, 15), 'n', 'n > 7')");
		expression.Evaluate().Should().Be(10);
	}

	[Fact]
	public void First_Numbers_LessThan_ReturnsFirst()
	{
		var expression = new ExtendedExpression("first(list(10, 5, 3, 1), 'n', 'n < 5')");
		expression.Evaluate().Should().Be(3);
	}

	[Fact]
	public void First_Doubles_Works()
	{
		var expression = new ExtendedExpression("first(list(1.5, 2.5, 3.5), 'n', 'n > 2.0')");
		expression.Evaluate().Should().Be(2.5);
	}

	[Fact]
	public void First_ComplexPredicate_Works()
	{
		var expression = new ExtendedExpression("first(list(1, 5, 10, 15, 20), 'n', 'n >= 10 && n <= 15')");
		expression.Evaluate().Should().Be(10);
	}

	[Fact]
	public void First_NegativeNumbers_Works()
	{
		var expression = new ExtendedExpression("first(list(-5, -2, 3, 7), 'n', 'n > 0')");
		expression.Evaluate().Should().Be(3);
	}

	[Fact]
	public void First_AllMatch_ReturnsFirst()
	{
		var expression = new ExtendedExpression("first(list(2, 4, 6, 8), 'n', 'n % 2 == 0')");
		expression.Evaluate().Should().Be(2);
	}

	[Fact]
	public void First_LastItemMatches_ReturnsLast()
	{
		var expression = new ExtendedExpression("first(list(1, 3, 5, 8), 'n', 'n % 2 == 0')");
		expression.Evaluate().Should().Be(8);
	}

	[Fact]
	public void First_Booleans_ReturnsFirstTrue()
	{
		var expression = new ExtendedExpression("first(list(false, true, false), 'b', 'b == true')");
		expression.Evaluate().Should().Be(true);
	}
}
