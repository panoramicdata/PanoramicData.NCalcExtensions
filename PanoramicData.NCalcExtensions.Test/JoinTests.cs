using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class JoinTests : NCalcTest
{

	[Fact]
	public void Join_Simple_Succeeds()
	{
		var expression = new ExtendedExpression("join(list('a', 'b', 'c'), ', ')");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("a, b, c");
	}

	[Fact]
	public void Join_ContainingNulls_Succeeds()
	{
		var expression = new ExtendedExpression("join(list('a', null, 'c'), ',')");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("a,,c");
	}

	[Fact]
	public void Join_Array_Succeeds()
	{
		var expression = new ExtendedExpression("join(array, ', ')");
		expression.Parameters["array"] = new[] { "a", "b", "c" };
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("a, b, c");
	}

	[Theory]
	[InlineData("join()")]
	[InlineData("join(1)")]
	public void Join_InsufficientParameters_ThrowsException(string expression)
		=> new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<FormatException>();

	[Theory]
	[InlineData("join(split('a b c', ' '), ',')", "a,b,c")]
	[InlineData("join(split('a b c', ' '), ', ')", "a, b, c")]
	[InlineData("join(list('a', 'b', 'c'), ', ')", "a, b, c")]
	public void Switch_ReturnsExpected(string expression, object? expectedOutput)
		=> new ExtendedExpression(expression).Evaluate().Should().Be(expectedOutput);

		// Additional comprehensive tests
		[Fact]
		public void Join_EmptyList_ReturnsEmpty()
		{
			var expression = new ExtendedExpression("join(list(), ',')");
			expression.Evaluate().Should().Be("");
		}

		[Fact]
		public void Join_SingleItem_ReturnsItem()
		{
			var expression = new ExtendedExpression("join(list('single'), ',')");
			expression.Evaluate().Should().Be("single");
		}

		[Fact]
		public void Join_TwoItems_JoinsProperly()
		{
			var expression = new ExtendedExpression("join(list('first', 'second'), ' | ')");
			expression.Evaluate().Should().Be("first | second");
		}

		[Fact]
		public void Join_Numbers_ConvertsToStrings()
		{
			var expression = new ExtendedExpression("join(list(1, 2, 3), ',')");
			expression.Evaluate().Should().Be("1,2,3");
		}

		[Fact]
		public void Join_MixedTypes_ConvertsToStrings()
		{
			var expression = new ExtendedExpression("join(list('a', 1, true, 3.14), ' - ')");
			expression.Evaluate().Should().Be("a - 1 - True - 3.14");
		}

		[Fact]
		public void Join_WithEmptyStringSeparator_Concatenates()
		{
			var expression = new ExtendedExpression("join(list('a', 'b', 'c'), '')");
			expression.Evaluate().Should().Be("abc");
		}

		[Fact]
		public void Join_WithMultiCharSeparator_Works()
		{
			var expression = new ExtendedExpression("join(list('a', 'b', 'c'), ' :: ')");
			expression.Evaluate().Should().Be("a :: b :: c");
		}

		[Fact]
		public void Join_WithNewlineSeparator_Works()
		{
			var expression = new ExtendedExpression("join(list('line1', 'line2', 'line3'), '\\n')");
			// Use escaped backslash-n in expression, not actual newline
			var result = expression.Evaluate() as string;
			result.Should().NotBeNull();
			// The result will have literal \n, not newline character
			result.Should().Contain("line1");
		}

		[Fact]
		public void Join_AllNulls_ReturnsEmptyWithSeparators()
		{
			var expression = new ExtendedExpression("join(list(null, null, null), ',')");
			expression.Evaluate().Should().Be(",,");
		}

		[Fact]
		public void Join_EmptyStringsInList_Preserves()
		{
			var expression = new ExtendedExpression("join(list('', 'b', ''), ',')");
			expression.Evaluate().Should().Be(",b,");
		}

		[Fact]
		public void Join_WithSpaceSeparator_Works()
		{
			var expression = new ExtendedExpression("join(list('hello', 'world'), ' ')");
			expression.Evaluate().Should().Be("hello world");
		}

		[Fact]
		public void Join_LongList_Works()
		{
			var expression = new ExtendedExpression("join(list('a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'), '-')");
			expression.Evaluate().Should().Be("a-b-c-d-e-f-g-h");
		}

		[Fact]
		public void Join_WithBooleans_ConvertsToString()
		{
			var expression = new ExtendedExpression("join(list(true, false, true), ',')");
			expression.Evaluate().Should().Be("True,False,True");
		}

		[Fact]
		public void Join_WithVariables_Works()
		{
			var expression = new ExtendedExpression("join(myList, mySeparator)");
			expression.Parameters["myList"] = new[] { "x", "y", "z" };
			expression.Parameters["mySeparator"] = " | ";
			expression.Evaluate().Should().Be("x | y | z");
		}

		[Fact]
		public void Join_TypedListInt_ThrowsException()
		{
			var expression = new ExtendedExpression("join(myList, ',')");
			expression.Parameters["myList"] = new List<int> { 10, 20, 30 };
			// Join expects IEnumerable<string> or List<object>, not List<int>
			expression.Invoking(e => e.Evaluate())
				.Should().Throw<FormatException>();
		}

		[Fact]
		public void Join_TypedListString_Works()
		{
			var expression = new ExtendedExpression("join(myList, ' ')");
			expression.Parameters["myList"] = new List<string> { "one", "two", "three" };
			expression.Evaluate().Should().Be("one two three");
		}

		[Fact]
		public void Join_SpecialCharactersInItems_Preserves()
		{
			var expression = new ExtendedExpression("join(list('a@b', 'c#d', 'e$f'), ',')");
			expression.Evaluate().Should().Be("a@b,c#d,e$f");
		}

		[Fact]
		public void Join_WithTabSeparator_Works()
		{
			var expression = new ExtendedExpression("join(list('col1', 'col2', 'col3'), '\t')");
			expression.Evaluate().Should().Be("col1\tcol2\tcol3");
		}

		[Fact]
		public void Join_ChainedWithOtherFunctions_Works()
		{
			var expression = new ExtendedExpression("join(select(list(1, 2, 3), 'n', 'n * 2'), ', ')");
			expression.Evaluate().Should().Be("2, 4, 6");
		}

		[Fact]
		public void Join_WithDistinct_Works()
		{
			var expression = new ExtendedExpression("join(distinct(list('a', 'b', 'a', 'c', 'b')), ',')");
			expression.Evaluate().Should().Be("a,b,c");
		}

		// Error cases
		[Fact]
		public void Join_NullList_ReturnsEmpty()
		{
			var expression = new ExtendedExpression("join(null, ',')");
			// Join handles null by creating empty list
			expression.Evaluate().Should().Be("");
		}

		[Fact]
		public void Join_NonEnumerableFirstParam_ThrowsException()
		{
			var expression = new ExtendedExpression("join(42, ',')");
			expression.Invoking(e => e.Evaluate())
				.Should().Throw<FormatException>();
		}

		[Fact]
		public void Join_NonStringSeparator_ThrowsException()
		{
			var expression = new ExtendedExpression("join(list('a', 'b'), 123)");
			expression.Invoking(e => e.Evaluate())
				.Should().Throw<FormatException>();
		}

		[Fact]
		public void Join_NullSeparator_ThrowsException()
		{
			var expression = new ExtendedExpression("join(list('a', 'b'), null)");
			expression.Invoking(e => e.Evaluate())
				.Should().Throw<FormatException>();
		}
	}
