namespace PanoramicData.NCalcExtensions.Test;

public class IsNullOrWhiteSpaceTests
{
	[Theory]
	[InlineData("'a'", false)]
	[InlineData("' '", true)]
	[InlineData("null", true)]
	[InlineData("''", true)]
	public void IsNullOrWhiteSpace_Succeeds(string parameter, bool expectedValue)
		=> new ExtendedExpression($"isNullOrWhiteSpace({parameter})").Evaluate().Should().Be(expectedValue);

	[Theory]
	[InlineData("'a', 'a'")]
	[InlineData("")]
	[InlineData("'a', null, null")]
	public void IsNullOrWhiteSpace_Fails(string parameter)
		=> new ExtendedExpression($"isNullOrWhiteSpace({parameter})")
		.Invoking(x => x.Evaluate())
		.Should()
		.Throw<FormatException>().WithMessage("isNullOrWhiteSpace() requires one parameter.");
}
public class IsNullOrEmptyTests
{
	[Theory]
	[InlineData("'a'", false)]
	[InlineData("' '", false)]
	[InlineData("null", true)]
	[InlineData("''", true)]
	public void IsNullOrWhiteSpace_Succeeds(string parameter, bool expectedValue)
		=> new ExtendedExpression($"isNullOrEmpty({parameter})").Evaluate().Should().Be(expectedValue);

	[Theory]
	[InlineData("'a', 'a'")]
	[InlineData("")]
	[InlineData("'a', null, null")]
	public void IsNullOrWhiteSpace_Fails(string parameter)
		=> new ExtendedExpression($"isNullOrEmpty({parameter})")
		.Invoking(x => x.Evaluate())
		.Should()
		.Throw<FormatException>().WithMessage("isNullOrEmpty() requires one parameter.");
}
