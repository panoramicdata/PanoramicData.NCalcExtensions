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

	// AC-01: Existing varargs behavior unchanged - positive case
	[Fact]
	public void In_Varargs_ExistingBehavior_PositiveCase()
	{
		var expression = new ExtendedExpression("in('needle', 'haystack', 'with', 'a', 'needle', 'in', 'it')");
		expression.Evaluate().Should().Be(true);
	}

	// AC-01b: Existing varargs behavior unchanged - negative case
	[Fact]
	public void In_Varargs_ExistingBehavior_NegativeCase()
	{
		var expression = new ExtendedExpression("in('needle', 'haystack', 'with', 'only', 'hay')");
		expression.Evaluate().Should().Be(false);
	}

	// AC-02: Variable list support - positive case
	[Fact]
	public void In_ListVariable_PositiveCase_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in(ThingToFind, Haystack)");
		expression.Parameters["ThingToFind"] = "needle";
		expression.Parameters["Haystack"] = new List<object?> { "haystack", "with", "needle", "in", "it" };
		expression.Evaluate().Should().Be(true);
	}

	// AC-02: list() literal in second position - positive case
	[Fact]
	public void In_ListLiteral_PositiveCase_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in(ThingToFind, list('haystack', 'with', 'needle', 'in', 'it'))");
		expression.Parameters["ThingToFind"] = "needle";
		expression.Evaluate().Should().Be(true);
	}

	// AC-03: Variable list support - negative case
	[Fact]
	public void In_ListVariable_NegativeCase_ReturnsFalse()
	{
		var expression = new ExtendedExpression("in(ThingToFind, HayOnly)");
		expression.Parameters["ThingToFind"] = "needle";
		expression.Parameters["HayOnly"] = new List<object?> { "haystack", "with", "only", "hay" };
		expression.Evaluate().Should().Be(false);
	}

	// AC-04: Case-sensitive - wrong case returns false
	[Fact]
	public void In_ListVariable_CaseSensitive_WrongCase_ReturnsFalse()
	{
		var expression = new ExtendedExpression("in(ThingToFind, Haystack)");
		expression.Parameters["ThingToFind"] = "Needle";
		expression.Parameters["Haystack"] = new List<object?> { "haystack", "with", "needle", "in", "it" };
		expression.Evaluate().Should().Be(false);
	}

	// AC-05: Case-sensitive - exact case returns true
	[Fact]
	public void In_ListVariable_CaseSensitive_ExactCase_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in(ThingToFind, Haystack)");
		expression.Parameters["ThingToFind"] = "Needle";
		expression.Parameters["Haystack"] = new List<object?> { "haystack", "with", "Needle", "in", "it" };
		expression.Evaluate().Should().Be(true);
	}

	// Typed List<string> variable works as haystack
	[Fact]
	public void In_TypedListStringVariable_Works()
	{
		var expression = new ExtendedExpression("in(ThingToFind, Haystack)");
		expression.Parameters["ThingToFind"] = "needle";
		expression.Parameters["Haystack"] = new List<string> { "haystack", "with", "needle" };
		expression.Evaluate().Should().Be(true);
	}

	// Empty list variable returns false
	[Fact]
	public void In_EmptyListVariable_ReturnsFalse()
	{
		var expression = new ExtendedExpression("in(ThingToFind, Haystack)");
		expression.Parameters["ThingToFind"] = "needle";
		expression.Parameters["Haystack"] = new List<object?>();
		expression.Evaluate().Should().Be(false);
	}

	// Empty list() literal returns false
	[Fact]
	public void In_EmptyListLiteral_ReturnsFalse()
	{
		var expression = new ExtendedExpression("in(ThingToFind, list())");
		expression.Parameters["ThingToFind"] = "needle";
		expression.Evaluate().Should().Be(false);
	}

	// list() literal - negative case
	[Fact]
	public void In_ListLiteral_NegativeCase_ReturnsFalse()
	{
		var expression = new ExtendedExpression("in(ThingToFind, list('haystack', 'with', 'only', 'hay'))");
		expression.Parameters["ThingToFind"] = "needle";
		expression.Evaluate().Should().Be(false);
	}

	// Null needle found in list variable
	[Fact]
	public void In_NullNeedle_InListVariable_ReturnsTrue()
	{
		var expression = new ExtendedExpression("in(ThingToFind, Haystack)");
		expression.Parameters["ThingToFind"] = null;
		expression.Parameters["Haystack"] = new List<object?> { "a", null, "b" };
		expression.Evaluate().Should().Be(true);
	}

	// Null needle not in list variable
	[Fact]
	public void In_NullNeedle_NotInListVariable_ReturnsFalse()
	{
		var expression = new ExtendedExpression("in(ThingToFind, Haystack)");
		expression.Parameters["ThingToFind"] = null;
		expression.Parameters["Haystack"] = new List<object?> { "a", "b", "c" };
		expression.Evaluate().Should().Be(false);
	}

	// Typed int[] array variable works as haystack
	[Fact]
	public void In_TypedIntArrayVariable_Works()
	{
		var expression = new ExtendedExpression("in(ThingToFind, Haystack)");
		expression.Parameters["ThingToFind"] = 3;
		expression.Parameters["Haystack"] = new int[] { 1, 2, 3, 4 };
		expression.Evaluate().Should().Be(true);
	}
}
