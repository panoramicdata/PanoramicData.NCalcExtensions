namespace PanoramicData.NCalcExtensions.Test;

public class RegexReplaceTests
{
	[Fact]
	public void RegexReplace_BasicReplacement_Succeeds()
	{
		var expression = new ExtendedExpression("regexReplace('hello world', 'world', 'there')");
		(expression.Evaluate() as string).Should().Be("hello there");
	}

	[Fact]
	public void RegexReplace_PatternMatchesMultiple_ReplacesAll()
	{
		var expression = new ExtendedExpression("regexReplace('aabbcc', '[abc]', 'x')");
		(expression.Evaluate() as string).Should().Be("xxxxxx");
	}

	[Fact]
	public void RegexReplace_NoMatch_ReturnsOriginal()
	{
		var expression = new ExtendedExpression("regexReplace('hello', 'xyz', 'abc')");
		(expression.Evaluate() as string).Should().Be("hello");
	}

	[Fact]
	public void RegexReplace_EmptyPattern_ReplacesAllPositions()
	{
		var expression = new ExtendedExpression("regexReplace('ab', '', '-')");
		(expression.Evaluate() as string).Should().Be("-a-b-");
	}

	[Fact]
	public void RegexReplace_EmptyReplacement_RemovesMatches()
	{
		var expression = new ExtendedExpression("regexReplace('hello world', '\\\\s+', '')");
		(expression.Evaluate() as string).Should().Be("helloworld");
	}

	[Fact]
	public void RegexReplace_CaptureGroupInReplacement_Succeeds()
	{
		var expression = new ExtendedExpression("regexReplace('2024-01-15', '(\\\\d{4})-(\\\\d{2})-(\\\\d{2})', '$3/$2/$1')");
		(expression.Evaluate() as string).Should().Be("15/01/2024");
	}

	[Fact]
	public void RegexReplace_NullInput_ThrowsException()
		=> new ExtendedExpression("regexReplace(null, 'x', 'y')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*string input*string pattern*string replacement*");

	[Fact]
	public void RegexReplace_NullPattern_ThrowsException()
		=> new ExtendedExpression("regexReplace('hello', null, 'y')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*string input*string pattern*string replacement*");

	[Fact]
	public void RegexReplace_NullReplacement_ThrowsException()
		=> new ExtendedExpression("regexReplace('hello', 'x', null)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*string input*string pattern*string replacement*");
}
