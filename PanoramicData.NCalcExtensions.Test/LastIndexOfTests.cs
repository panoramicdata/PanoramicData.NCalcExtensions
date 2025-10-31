namespace PanoramicData.NCalcExtensions.Test;

public class LastIndexOfTests : NCalcTest
{
	[Theory]
	[InlineData("lastIndexOf('abcdefabc', 'abc')", 6)]
	[InlineData("lastIndexOf('abcdefabc', 'def')", 3)]
	[InlineData("lastIndexOf('hello', 'l')", 3)]
	public void LastIndexOf_Found_ReturnsCorrectIndex(string expression, int expected)
		=> Test(expression).Should().Be(expected);

	[Theory]
	[InlineData("lastIndexOf('abcdef', 'xyz')", -1)]
	[InlineData("lastIndexOf('hello', 'world')", -1)]
	public void LastIndexOf_NotFound_ReturnsMinusOne(string expression, int expected)
		=> Test(expression).Should().Be(expected);

	[Theory]
	[InlineData("lastIndexOf('aaa', 'a')", 2)]
	[InlineData("lastIndexOf('test test test', 'test')", 10)]
	public void LastIndexOf_MultipleOccurrences_ReturnsLast(string expression, int expected)
		=> Test(expression).Should().Be(expected);

	[Theory]
	[InlineData("lastIndexOf('', 'a')", -1)]
	[InlineData("lastIndexOf('test', '')", 4)] // .NET behavior: returns length of string
	public void LastIndexOf_EmptyStrings_HandlesCorrectly(string expression, int expected)
		=> Test(expression).Should().Be(expected);

	[Theory]
	[InlineData("lastIndexOf()")]
	[InlineData("lastIndexOf('test')")]
	public void LastIndexOf_InsufficientParameters_ThrowsException(string expression)
		=> new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	[Fact]
	public void LastIndexOf_NullFirstParameter_ThrowsException()
		=> new ExtendedExpression("lastIndexOf(null, 'test')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();

	[Fact]
	public void LastIndexOf_NullSecondParameter_ThrowsException()
		=> new ExtendedExpression("lastIndexOf('test', null)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();
}
