using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class AllTests : NCalcTest
{
	[Theory]
	[InlineData("1, 2, 3", true)]
	[InlineData("4, 5, 6", false)]
	[InlineData("7, 8, 9", false)]
	public void All_LessThanFive_Succeeds(string stringList, bool allLessThanFive)
		=> new ExtendedExpression($"all(list({stringList}), 'n', 'n < 5')")
		.Evaluate()
		.Should()
		.Be(allLessThanFive);

	[Theory]
	[InlineData("true, true, true", true)]
	[InlineData("true, true, false", false)]
	[InlineData("true", true)]
	[InlineData("false", false)]
	[InlineData("", true)]
	public void All_Bools_Succeeds(string stringList, bool expectedResult)
		=> new ExtendedExpression($"all(list({stringList}))")
		.Evaluate()
		.Should()
		.Be(expectedResult);

	// Additional comprehensive tests
	[Fact]
	public void All_EmptyList_ReturnsTrue()
		=> Test("all(list(), 'n', 'n > 0')").Should().Be(true);

	[Fact]
	public void All_NoParameters_ReturnsTrue()
		=> Test("all()").Should().Be(true);

	[Fact]
	public void All_AllMatch_ReturnsTrue()
		=> Test("all(list(2, 4, 6, 8), 'n', 'n % 2 == 0')").Should().Be(true);

	[Fact]
	public void All_OneDoesNotMatch_ReturnsFalse()
		=> Test("all(list(2, 4, 5, 8), 'n', 'n % 2 == 0')").Should().Be(false);

	[Fact]
	public void All_NoneMatch_ReturnsFalse()
		=> Test("all(list(1, 3, 5, 7), 'n', 'n % 2 == 0')").Should().Be(false);

	[Fact]
	public void All_Strings_Works()
		=> Test("all(list('apple', 'apricot', 'avocado'), 's', 'startsWith(s, \"a\")')").Should().Be(true);

	[Fact]
	public void All_Strings_OneFails()
		=> Test("all(list('apple', 'banana', 'avocado'), 's', 'startsWith(s, \"a\")')").Should().Be(false);

	[Fact]
	public void All_GreaterThan_Works()
		=> Test("all(list(10, 20, 30), 'n', 'n > 5')").Should().Be(true);

	[Fact]
	public void All_LessThanOrEqual_Works()
		=> Test("all(list(1, 2, 3, 4, 5), 'n', 'n <= 5')").Should().Be(true);

	[Fact]
	public void All_ComplexCondition_Works()
		=> Test("all(list(10, 20, 30), 'n', 'n >= 10 && n <= 30')").Should().Be(true);

	[Fact]
	public void All_WithVariables_Works()
	{
		var expression = new ExtendedExpression("all(myList, 'n', 'n > 0')");
		expression.Parameters["myList"] = new List<object> { 1, 2, 3, 4, 5 };
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void All_SingleItemTrue_ReturnsTrue()
		=> Test("all(list(5), 'n', 'n > 0')").Should().Be(true);

	[Fact]
	public void All_SingleItemFalse_ReturnsFalse()
		=> Test("all(list(-5), 'n', 'n > 0')").Should().Be(false);

	[Fact]
	public void All_Doubles_Works()
		=> Test("all(list(1.5, 2.5, 3.5), 'n', 'n > 1.0')").Should().Be(true);

	[Fact]
	public void All_NegativeNumbers_Works()
		=> Test("all(list(-10, -20, -30), 'n', 'n < 0')").Should().Be(true);

	[Fact]
	public void All_WithNulls_HandlesNull()
		=> Test("all(list(1, null, 3), 'n', 'n != null')").Should().Be(false);

	// Error cases
	[Fact]
	public void All_NullList_ThrowsException()
	{
		var expression = new ExtendedExpression("all(null, 'n', 'n > 0')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}

	[Fact]
	public void All_InvalidPredicate_ThrowsException()
	{
		var expression = new ExtendedExpression("all(list(1, 2, 3), null, 'n > 0')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}

	[Fact]
	public void All_InvalidLambda_ThrowsException()
	{
		var expression = new ExtendedExpression("all(list(1, 2, 3), 'n', null)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}

	[Fact]
	public void All_TwoParameters_ThrowsException()
	{
		var expression = new ExtendedExpression("all(list(1, 2, 3), 'n')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}
}
