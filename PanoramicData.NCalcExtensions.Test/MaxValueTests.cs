namespace PanoramicData.NCalcExtensions.Test;

public class MaxValueTests
{
	[Theory]
	[InlineData("sbyte", sbyte.MaxValue)]
	[InlineData("byte", byte.MaxValue)]
	[InlineData("short", short.MaxValue)]
	[InlineData("ushort", ushort.MaxValue)]
	[InlineData("int", int.MaxValue)]
	[InlineData("uint", uint.MaxValue)]
	[InlineData("long", long.MaxValue)]
	[InlineData("ulong", ulong.MaxValue)]
	[InlineData("float", float.MaxValue)]
	[InlineData("double", double.MaxValue)]
	public void MaxValue_ReturnsExpectedValue(string type, object expectedOutput)
	{
		var expression = new ExtendedExpression($"maxValue('{type}')");
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Fact]
	public void MaxValue_ForDecimal_ReturnsExpectedValue()
	{
		var expression = new ExtendedExpression($"maxValue('decimal')");
		expression.Evaluate().Should().BeEquivalentTo(decimal.MaxValue);
	}

	[Fact]
	public void MaxValue_ForUnsupportedType_ThrowsFormatException()
	{
		var expression = new ExtendedExpression($"maxValue('unsupportedType')");
		expression.Invoking(x => x.Evaluate()).Should().Throw<FormatException>();
	}
}