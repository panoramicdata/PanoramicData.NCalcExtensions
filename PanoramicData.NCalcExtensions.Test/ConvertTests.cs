namespace PanoramicData.NCalcExtensions.Test;

public class ConvertTests
{
	[Theory]
	[InlineData("null", "null", null)]
	[InlineData("1", "null", null)]
	[InlineData("null", "1", 1)]
	[InlineData("null", "value", null)]
	[InlineData("1", "value", 1)]
	[InlineData("1 + 1", "value", 2)]
	public void Convert_Succeeds(
		string firstParameter,
		string secondParameter,
		object? expectedResult
	)
		=> new ExtendedExpression($"convert({firstParameter}, {secondParameter})")
		.Evaluate()
		.Should()
		.Be(expectedResult);

	[Theory]
	[InlineData("")]
	[InlineData("1")]
	[InlineData("1, 1, 1")]
	[InlineData("1, 1, 1, 1")]
	public void IncorrectParameterCount_Throws(string parameters) =>
		new ExtendedExpression($"convert({parameters})")
		.Invoking(e => e.Evaluate())
		.Should()
		.Throw<FormatException>()
		.WithMessage($"{ExtensionFunction.Convert}() requires two parameters.");
}
