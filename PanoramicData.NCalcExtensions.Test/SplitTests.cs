using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class SplitTests : NCalcTest
{
	/// <summary>
	/// The expected parts are themselves written comma-separated, which none of these cases
	/// contains a comma within.
	/// </summary>
	[Theory]
	[InlineData("split('a b c', ' ')", "a,b,c")]
	[InlineData("split('aXXbXXc', 'XX')", "a,b,c")]
	[InlineData("split('one,two,three', ',')", "one,two,three")]
	[InlineData("split('one<=>two<=>three', '<=>')", "one,two,three")]
	[InlineData("split('a|b|c|d', '|')", "a,b,c,d")]
	[InlineData("split('1,2,3,4,5,6,7,8,9,10', ',')", "1,2,3,4,5,6,7,8,9,10")]
	[InlineData("split('nodivider', ',')", "nodivider")]
	// A delimiter at either end, or repeated, yields an empty part.
	[InlineData("split('', ',')", "")]
	[InlineData("split('a,,b', ',')", "a,,b")]
	[InlineData("split(',a,b', ',')", ",a,b")]
	[InlineData("split('a,b,', ',')", "a,b,")]
	// White space is part of a value, not a delimiter.
	[InlineData("split('  a  ,  b  ', ',')", "  a  ,  b  ")]
	[InlineData("split('line1\nline2\nline3', '\n')", "line1,line2,line3")]
	[InlineData("split('col1\tcol2\tcol3', '\t')", "col1,col2,col3")]
	// The delimiter is matched whole, not as a set of characters.
	[InlineData("split('a@b#c$d', '@#$')", "a@b#c$d")]
	public void Split_ReturnsExpectedParts(string expressionText, string expectedParts)
		=> EvaluateToStrings(expressionText).Should().Equal(expectedParts.Split(','));

	[Fact]
	public void Split_WithVariables_Works()
	{
		var expression = new ExtendedExpression("split(myString, myDelimiter)");
		expression.Parameters["myString"] = "apple;banana;cherry";
		expression.Parameters["myDelimiter"] = ";";

		(expression.Evaluate() as List<string>).Should().Equal("apple", "banana", "cherry");
	}

	[Fact]
	public void Split_ThenJoin_RoundTrip()
		=> Test("join(split('a,b,c', ','), ',')").Should().Be("a,b,c");

	[Fact]
	public void Split_ThenCount_Works()
		=> Test("count(split('one two three four', ' '))").Should().Be(4);

	[Fact]
	public void Split_InSelect_Works()
		=> (Test("select(split('a,b,c', ','), 's', 'toUpper(s)')") as List<object?>)
			.Should().BeEquivalentTo(["A", "B", "C"], options => options.WithStrictOrdering());

	[Theory]
	[InlineData("split()")]
	[InlineData("split('a b c')")]
	[InlineData("split('a b c', '')")]
	[InlineData("split('x x x', '')")]
	[InlineData("split(null, ',')")]
	[InlineData("split('a,b,c', null)")]
	[InlineData("split(123, ',')")]
	[InlineData("split('a,b,c', 123)")]
	public void Split_WithInvalidArguments_ThrowsFormatException(string expressionText)
		=> TestShouldThrowExactly<FormatException>(expressionText, "*");

	/// <summary>
	/// Evaluates <paramref name="expressionText"/> to the list of strings that split() returns.
	/// </summary>
	private static List<string> EvaluateToStrings(string expressionText)
	{
		var result = Test(expressionText);
		result.Should().BeOfType<List<string>>();
		return (List<string>)result!;
	}
}
