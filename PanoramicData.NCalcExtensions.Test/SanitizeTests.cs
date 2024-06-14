namespace PanoramicData.NCalcExtensions.Test;

public class SanitizeTests
{
	[Theory]
	[InlineData("5", "5")]
	[InlineData("''", "5")]
	[InlineData("'abc'", "null")]
	[InlineData("'abc'", "5.0")]
	[InlineData("'abc'", "toDateTime('2019-01-01', 'yyyy-MM-dd')")]
	[InlineData("toDateTime('2019-01-01', 'yyyy-MM-dd')", "toDateTime('2019-01-01', 'yyyy-MM-dd')")]
	public void Evaluate_InvalidTwoParameterInput_ThrowsFormatException(string inputString, string allowedCharacters)
	{
		var expression = new ExtendedExpression($"sanitize({inputString}, {allowedCharacters})");

		var action = expression.Evaluate;

		action.Should().Throw<FormatException>();
	}

	[Theory]
	[InlineData("'abc'", "'abc'", "null")]
	[InlineData("'abc'", "'abc'", "5")]
	[InlineData("'abc'", "'abc'", "5.0")]
	[InlineData("'abc'", "'abc'", "toDateTime('2019-01-01', 'yyyy-MM-dd')")]
	public void Evaluate_InvalidThreeParameterInput_ThrowsFormatException(string inputString, string allowedCharacters, string replacementCharacters)
	{
		var expression = new ExtendedExpression($"sanitize({inputString}, {allowedCharacters}, {replacementCharacters})");

		var action = expression.Evaluate;

		action.Should().Throw<FormatException>();
	}

	[Theory]
	[InlineData("null", "null", null)]
	[InlineData("''", "''", "")]
	[InlineData("'abc'", "''", "")]
	[InlineData("'abc'", "'abc'", "abc")]
	[InlineData("'abc'", "'ABC'", "")]
	[InlineData("'abc'", "'CBa'", "a")]
	[InlineData("'abc def'", "'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'", "abcdef")]
	[InlineData("'abc def'", "'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ '", "abc def")]
	[InlineData("'abc_def'", "'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ '", "abcdef")]
	public void Evaluate_ValidTwoParameterInput_ReturnsExpectedResult(string inputString, string allowedCharacters, string? expectedResult)
	{
		var expression = new ExtendedExpression($"sanitize({inputString}, {allowedCharacters})");

		var result = expression.Evaluate();

		result.Should().Be(expectedResult);
	}

	[Theory]
	[InlineData("null", "null", "null", null)]
	[InlineData("''", "''", "''", "")]
	[InlineData("'abc'", "'abc'", "''", "abc")]
	[InlineData("'abc'", "'ABC'", "''", "")]
	[InlineData("'abc'", "'CBa'", "''", "a")]
	[InlineData("'abc def'", "'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'", "''", "abcdef")]
	[InlineData("'abc def'", "'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ '", "''", "abc def")]
	[InlineData("'abc_def'", "'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ '", "''", "abcdef")]
	[InlineData("'abc'", "'abc'", "'?'", "abc")]
	[InlineData("'abc'", "'ABC'", "'?'", "???")]
	[InlineData("'abc'", "'CBa'", "'?'", "a??")]
	[InlineData("'abc def'", "'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'", "'?'", "abc?def")]
	[InlineData("'abc def'", "'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ '", "'?'", "abc def")]
	[InlineData("'abc_def'", "'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ '", "'?'", "abc?def")]
	[InlineData("'.abc_def'", "'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ '", "'?'", "?abc?def")]
	public void Evaluate_ValidThreeParameterInput_ReturnsExpectedResult(string inputString, string allowedCharacters, string replacementCharacters, string? expectedResult)
	{
		var expression = new ExtendedExpression($"sanitize({inputString}, {allowedCharacters}, {replacementCharacters})");

		var result = expression.Evaluate();

		result.Should().Be(expectedResult);
	}
}
