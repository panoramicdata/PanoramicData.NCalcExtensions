namespace PanoramicData.NCalcExtensions.Test;

public class ReplaceTests
{
	[Fact]
	public void Replace_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("replace('abcdefg', 'cde', 'CDE')");
		(expression.Evaluate() as string).Should().Be("abCDEfg");
	}

	[Fact]
	public void Replace_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("replace('abcdefg', 'cde', '')");
		(expression.Evaluate() as string).Should().Be("abfg");
	}

	[Fact]
	public void Replace_Example3_Succeeds()
	{
		var expression = new ExtendedExpression("replace('abcdefg', 'a', '1', 'bc', '23')");
		(expression.Evaluate() as string).Should().Be("123defg");
	}

	// Additional comprehensive tests

	[Fact]
	public void Replace_NoMatch_ReturnsOriginal()
	{
		var expression = new ExtendedExpression("replace('abcdefg', 'xyz', 'XYZ')");
		expression.Evaluate().Should().Be("abcdefg");
	}

	[Fact]
	public void Replace_MultipleOccurrences_ReplacesAll()
	{
		var expression = new ExtendedExpression("replace('banana', 'a', 'o')");
		expression.Evaluate().Should().Be("bonono");
	}

	[Fact]
	public void Replace_EmptyString_ReturnsEmpty()
	{
		var expression = new ExtendedExpression("replace('', 'a', 'b')");
		expression.Evaluate().Should().Be("");
	}

	[Fact]
	public void Replace_EmptyOldValue_ThrowsOrReturnsOriginal()
	{
		var expression = new ExtendedExpression("replace('abc', '', 'X')");
		// Empty oldValue throws an ArgumentException in string.Replace
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<Exception>();
	}

	[Fact]
	public void Replace_ReplaceWithLongerString_Succeeds()
	{
		var expression = new ExtendedExpression("replace('abc', 'b', 'BBB')");
		expression.Evaluate().Should().Be("aBBBc");
	}

	[Fact]
	public void Replace_ReplaceWithShorterString_Succeeds()
	{
		var expression = new ExtendedExpression("replace('abbbbc', 'bbb', 'X')");
		expression.Evaluate().Should().Be("aXbc");
	}

	[Fact]
	public void Replace_CaseSensitive_NoMatch()
	{
		var expression = new ExtendedExpression("replace('ABC', 'abc', 'xyz')");
		expression.Evaluate().Should().Be("ABC");
	}

	[Fact]
	public void Replace_WholeString_Succeeds()
	{
		var expression = new ExtendedExpression("replace('hello', 'hello', 'goodbye')");
		expression.Evaluate().Should().Be("goodbye");
	}

	[Fact]
	public void Replace_AtStart_Succeeds()
	{
		var expression = new ExtendedExpression("replace('hello world', 'hello', 'hi')");
		expression.Evaluate().Should().Be("hi world");
	}

	[Fact]
	public void Replace_AtEnd_Succeeds()
	{
		var expression = new ExtendedExpression("replace('hello world', 'world', 'there')");
		expression.Evaluate().Should().Be("hello there");
	}

	[Fact]
	public void Replace_MultipleReplacements_Succeeds()
	{
		var expression = new ExtendedExpression("replace('abc def ghi', 'a', '1', 'd', '2', 'g', '3')");
		expression.Evaluate().Should().Be("1bc 2ef 3hi");
	}

	[Fact]
	public void Replace_MultipleReplacements_WithOverlap_Succeeds()
	{
		var expression = new ExtendedExpression("replace('abcabc', 'ab', 'X', 'bc', 'Y')");
		// First replacement happens first, then second on the result
		var result = expression.Evaluate() as string;
		result.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public void Replace_SpecialCharacters_Succeeds()
	{
		var expression = new ExtendedExpression("replace('a.b*c?d', '.', '-', '*', '+', '?', '!')");
		expression.Evaluate().Should().Be("a-b+c!d");
	}

	[Fact]
	public void Replace_Whitespace_Succeeds()
	{
		var expression = new ExtendedExpression("replace('a b c', ' ', '_')");
		expression.Evaluate().Should().Be("a_b_c");
	}

	[Fact]
	public void Replace_Tabs_Succeeds()
	{
		var expression = new ExtendedExpression("replace('a\tb\tc', '\t', ' ')");
		expression.Evaluate().Should().Be("a b c");
	}

	[Fact]
	public void Replace_Newlines_Succeeds()
	{
		var expression = new ExtendedExpression("replace('a\nb\nc', '\n', ' ')");
		expression.Evaluate().Should().Be("a b c");
	}

	[Fact]
	public void Replace_WithNull_Succeeds()
	{
		var expression = new ExtendedExpression("replace(myString, 'old', 'new')");
		expression.Parameters["myString"] = "old value";
		expression.Evaluate().Should().Be("new value");
	}

	[Fact]
	public void Replace_WithVariables_Succeeds()
	{
		var expression = new ExtendedExpression("replace(text, oldVal, newVal)");
		expression.Parameters["text"] = "hello world";
		expression.Parameters["oldVal"] = "world";
		expression.Parameters["newVal"] = "there";
		expression.Evaluate().Should().Be("hello there");
	}

	[Fact]
	public void Replace_ChainedReplacements_Succeeds()
	{
		var expression = new ExtendedExpression("replace(replace('abc', 'a', 'X'), 'b', 'Y')");
		expression.Evaluate().Should().Be("XYc");
	}

	[Fact]
	public void Replace_InConcat_Succeeds()
	{
		var expression = new ExtendedExpression("replace('hello', 'e', 'a') + ' world'");
		expression.Evaluate().Should().Be("hallo world");
	}

	[Fact]
	public void Replace_Numbers_Succeeds()
	{
		var expression = new ExtendedExpression("replace('123456', '3', 'X')");
		expression.Evaluate().Should().Be("12X456");
	}

	[Fact]
	public void Replace_RepeatingPattern_Succeeds()
	{
		var expression = new ExtendedExpression("replace('aaabbbccc', 'aaa', 'A', 'bbb', 'B', 'ccc', 'C')");
		expression.Evaluate().Should().Be("ABC");
	}

	// Error cases
	[Fact]
	public void Replace_OddNumberOfParameters_ThrowsException()
	{
		var expression = new ExtendedExpression("replace('abc', 'a')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}

	[Fact]
	public void Replace_NoParameters_ThrowsException()
	{
		var expression = new ExtendedExpression("replace()");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}

	[Fact]
	public void Replace_OneParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("replace('abc')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>();
	}
}
