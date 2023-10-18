namespace PanoramicData.NCalcExtensions.Test;

public class MinValueTests
{
	[Theory]
	[InlineData("sbyte", sbyte.MinValue)]
	[InlineData("byte", byte.MinValue)]
	[InlineData("short", short.MinValue)]
	[InlineData("ushort", ushort.MinValue)]
	[InlineData("int", int.MinValue)]
	[InlineData("uint", uint.MinValue)]
	[InlineData("long", long.MinValue)]
	[InlineData("ulong", ulong.MinValue)]
	[InlineData("float", float.MinValue)]
	[InlineData("double", double.MinValue)]
	public void MinValue_ReturnsExpectedValue(string type, object expectedOutput)
	{
		var expression = new ExtendedExpression($"minValue('{type}')");
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Fact]
	public void MinValue_ForDecimal_ReturnsExpectedValue()
	{
		var expression = new ExtendedExpression($"minValue('decimal')");
		expression.Evaluate().Should().BeEquivalentTo(decimal.MinValue);
	}

	[Fact]
	public void MinValue_ForUnsupportedType_ThrowsFormatException()
	{
		var expression = new ExtendedExpression($"minValue('unsupportedType')");
		expression.Invoking(x => x.Evaluate()).Should().Throw<FormatException>();
	}
}