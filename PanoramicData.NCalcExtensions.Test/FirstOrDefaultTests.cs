using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class FirstOrDefaultTests
{
	[Fact]
	public void FirstOrDefault_MatchingItem_Succeeds()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(1, 5, 2, 3), 'n', 'n % 2 == 0')");
		var result = expression.Evaluate() as int?;

		result.Should().Be(2);
	}

	[Fact]
	public void FirstOrDefault_NoMatchingItem_Succeeds()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(1, 5, 7, 3), 'n', 'n % 2 == 0')");
		var result = expression.Evaluate() as int?;

		result.Should().BeNull();
	}

	[Fact]
	public void FirstOrDefault_OneParameter_ReturnsFirstElement()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(1, 2, 3))");
		expression.Evaluate().Should().Be(1);
	}

	[Fact]
	public void FirstOrDefault_OneParameter_EmptyList_ReturnsNull()
	{
		var expression = new ExtendedExpression("firstOrDefault(list())");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void FirstOrDefault_NullFirstParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("firstOrDefault(null, 'n', 'n > 0')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*First*must be an IEnumerable*");
	}

	[Fact]
	public void FirstOrDefault_NullSecondParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(1,2,3), null, 'n > 0')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*Second*must be a string*");
	}

	[Fact]
	public void FirstOrDefault_NullThirdParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(1,2,3), 'n', null)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*Third*must be a string*");
	}

	// Additional comprehensive tests
	[Fact]
	public void FirstOrDefault_Strings_ReturnsFirst()
	{
		var expression = new ExtendedExpression("firstOrDefault(list('apple', 'banana', 'cherry'))");
		expression.Evaluate().Should().Be("apple");
	}

	[Fact]
	public void FirstOrDefault_Strings_WithPredicate_ReturnsMatch()
	{
		var expression = new ExtendedExpression("firstOrDefault(list('apple', 'banana', 'cherry'), 's', 'startsWith(s, \"b\")')");
		expression.Evaluate().Should().Be("banana");
	}

	[Fact]
	public void FirstOrDefault_Strings_NoMatch_ReturnsNull()
	{
		var expression = new ExtendedExpression("firstOrDefault(list('apple', 'banana', 'cherry'), 's', 'startsWith(s, \"z\")')");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void FirstOrDefault_Numbers_GreaterThan_ReturnsFirst()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(1, 5, 10, 15), 'n', 'n > 7')");
		expression.Evaluate().Should().Be(10);
	}

	[Fact]
	public void FirstOrDefault_Numbers_LessThan_ReturnsFirst()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(10, 5, 3, 1), 'n', 'n < 5')");
		expression.Evaluate().Should().Be(3);
	}

	[Fact]
	public void FirstOrDefault_WithNullInList_SkipsNull()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(null, 1, 2, 3))");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void FirstOrDefault_AllNulls_ReturnsNull()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(null, null, null))");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void FirstOrDefault_SingleItem_ReturnsIt()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(42))");
		expression.Evaluate().Should().Be(42);
	}

	[Fact]
	public void FirstOrDefault_SingleItemNoMatch_ReturnsNull()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(42), 'n', 'n < 0')");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void FirstOrDefault_Booleans_ReturnsFirstTrue()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(false, true, false), 'b', 'b == true')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void FirstOrDefault_Doubles_Works()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(1.5, 2.5, 3.5), 'n', 'n > 2.0')");
		expression.Evaluate().Should().Be(2.5);
	}

	[Fact]
	public void FirstOrDefault_ComplexPredicate_Works()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(1, 5, 10, 15, 20), 'n', 'n >= 10 && n <= 15')");
		expression.Evaluate().Should().Be(10);
	}

	[Fact]
	public void FirstOrDefault_WithVariables_Works()
	{
		var expression = new ExtendedExpression("firstOrDefault(myList, 'n', 'n > threshold')");
		expression.Parameters["myList"] = new List<object> { 1, 2, 5, 10 };
		expression.Parameters["threshold"] = 3;
		expression.Evaluate().Should().Be(5);
	}

	[Fact]
	public void FirstOrDefault_TypedArray_Works()
	{
		var expression = new ExtendedExpression("firstOrDefault(myArray)");
		expression.Parameters["myArray"] = new[] { "first", "second", "third" };
		expression.Evaluate().Should().Be("first");
	}

	[Fact]
	public void FirstOrDefault_ChainedWithOtherFunctions_Works()
	{
		var expression = new ExtendedExpression("firstOrDefault(select(list(1, 2, 3), 'n', 'n * 2'), 'n', 'n > 3')");
		expression.Evaluate().Should().Be(4);
	}

	[Fact]
	public void FirstOrDefault_NonEnumerable_ThrowsException()
	{
		var expression = new ExtendedExpression("firstOrDefault(42)");
		expression.Invoking(e => e.Evaluate()).Should().Throw<FormatException>();
	}

	[Fact]
	public void FirstOrDefault_NoParameters_ThrowsException()
	{
		var expression = new ExtendedExpression("firstOrDefault()");
		expression.Invoking(e => e.Evaluate()).Should().Throw<Exception>();
	}

	[Fact]
	public void FirstOrDefault_NegativeNumbers_Works()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(-5, -2, 3, 7), 'n', 'n > 0')");
		expression.Evaluate().Should().Be(3);
	}

	[Fact]
	public void FirstOrDefault_AllMatch_ReturnsFirst()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(2, 4, 6, 8), 'n', 'n % 2 == 0')");
		expression.Evaluate().Should().Be(2);
	}
}
