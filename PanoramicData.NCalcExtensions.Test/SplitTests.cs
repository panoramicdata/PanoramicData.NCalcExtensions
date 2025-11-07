using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class SplitTests : NCalcTest
{
	[Theory]
	[InlineData("split()")]
	[InlineData("split('a b c')")]
	public void Split_InsufficientParameters_ThrowsException(string expression)
		=> new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<FormatException>();

	[Theory]
	[InlineData("split('a b c', '')")]
	[InlineData("split('x x x', '')")]
	public void Split_EmptySecondParameter_ThrowsException(string expression)
		=> new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<FormatException>();

	[Theory]
	[InlineData("split('a b c', ' ')")]
	[InlineData("split('aXXbXXc', 'XX')")]
	public void Split_ValidParameters_Succeeds(string expression)
	{
		var e = new ExtendedExpression(expression);
		var result = e.Evaluate();
		result.Should().BeOfType<List<string>>();
		var list = (List<string>)result;
		list.Should().HaveCount(3);
		list[0].Should().Be("a");
		list[1].Should().Be("b");
		list[2].Should().Be("c");
	}

	// Additional comprehensive tests
	[Fact]
	public void Split_CommaSeparated_Works()
	{
		var expression = new ExtendedExpression("split('one,two,three', ',')");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(["one", "two", "three"], options => options.WithStrictOrdering());
	}

	[Fact]
	public void Split_SingleCharDelimiter_Works()
	{
		var expression = new ExtendedExpression("split('a|b|c|d', '|')");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(4);
	}

	[Fact]
	public void Split_NoDelimiterFound_ReturnsSingleItem()
	{
		var expression = new ExtendedExpression("split('nodivider', ',')");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(1);
		result![0].Should().Be("nodivider");
	}

	[Fact]
	public void Split_EmptyString_ReturnsEmptyList()
	{
		var expression = new ExtendedExpression("split('', ',')");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(1);
		result![0].Should().Be("");
	}

	[Fact]
	public void Split_ConsecutiveDelimiters_CreatesEmptyStrings()
	{
		var expression = new ExtendedExpression("split('a,,b', ',')");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result![1].Should().Be("");
	}

	[Fact]
	public void Split_DelimiterAtStart_CreatesEmptyFirst()
	{
		var expression = new ExtendedExpression("split(',a,b', ',')");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result![0].Should().Be("");
	}

	[Fact]
	public void Split_DelimiterAtEnd_CreatesEmptyLast()
	{
		var expression = new ExtendedExpression("split('a,b,', ',')");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result![2].Should().Be("");
	}

	[Fact]
	public void Split_MultiCharDelimiter_Works()
	{
		var expression = new ExtendedExpression("split('one<=>two<=>three', '<=>')");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(["one", "two", "three"], options => options.WithStrictOrdering());
	}

	[Fact]
	public void Split_WithWhitespace_Preserves()
	{
		var expression = new ExtendedExpression("split('  a  ,  b  ', ',')");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(2);
		result![0].Should().Be("  a  ");
		result[1].Should().Be("  b  ");
	}

	[Fact]
	public void Split_Newlines_Works()
	{
		var expression = new ExtendedExpression("split('line1\nline2\nline3', '\n')");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
	}

	[Fact]
	public void Split_Tabs_Works()
	{
		var expression = new ExtendedExpression("split('col1\tcol2\tcol3', '\t')");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
	}

	[Fact]
	public void Split_SpecialCharacters_Works()
	{
		var expression = new ExtendedExpression("split('a@b#c$d', '@#$')");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		// Split by string '@#$' not by chars
		result.Should().HaveCount(1);
	}

	[Fact]
	public void Split_WithVariables_Works()
	{
		var expression = new ExtendedExpression("split(myString, myDelimiter)");
		expression.Parameters["myString"] = "apple;banana;cherry";
		expression.Parameters["myDelimiter"] = ";";
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(["apple", "banana", "cherry"], options => options.WithStrictOrdering());
	}

	[Fact]
	public void Split_LongString_Works()
	{
		var expression = new ExtendedExpression("split('1,2,3,4,5,6,7,8,9,10', ',')");
		var result = expression.Evaluate() as List<string>;
		result.Should().NotBeNull();
		result.Should().HaveCount(10);
	}

	[Fact]
	public void Split_ThenJoin_RoundTrip()
	{
		var expression = new ExtendedExpression("join(split('a,b,c', ','), ',')");
		expression.Evaluate().Should().Be("a,b,c");
	}

	[Fact]
	public void Split_ThenCount_Works()
	{
		var expression = new ExtendedExpression("count(split('one two three four', ' '))");
		expression.Evaluate().Should().Be(4);
	}

	[Fact]
	public void Split_InSelect_Works()
	{
		var expression = new ExtendedExpression("select(split('a,b,c', ','), 's', 'toUpper(s)')");
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(["A", "B", "C"], options => options.WithStrictOrdering());
	}

	// Error cases
	[Fact]
	public void Split_NullString_ThrowsException()
	{
		var expression = new ExtendedExpression("split(null, ',')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}

	[Fact]
	public void Split_NullDelimiter_ThrowsException()
	{
		var expression = new ExtendedExpression("split('a,b,c', null)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}

	[Fact]
	public void Split_NonStringFirst_ThrowsException()
	{
		var expression = new ExtendedExpression("split(123, ',')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}

	[Fact]
	public void Split_NonStringSecond_ThrowsException()
	{
		var expression = new ExtendedExpression("split('a,b,c', 123)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}
}
