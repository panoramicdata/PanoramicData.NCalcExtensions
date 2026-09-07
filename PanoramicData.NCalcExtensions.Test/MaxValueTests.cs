namespace PanoramicData.NCalcExtensions.Test;

public class MaxValueTests : NCalcTest
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
		=> Test($"maxValue('{type}')").Should().BeEquivalentTo(expectedOutput);

	[Fact]
	public void MaxValue_ForDecimal_ReturnsExpectedValue()
		=> Test($"maxValue('decimal')").Should().BeEquivalentTo(decimal.MaxValue);

	[Fact]
	public void MaxValue_ForDateTime_ReturnsExpectedValue()
		=> Test($"maxValue('DateTime')").Should().BeEquivalentTo(DateTime.MaxValue);

	[Fact]
	public void MaxValue_ForDateTimeOffset_ReturnsExpectedValue()
		=> Test($"maxValue('DateTimeOffset')").Should().BeEquivalentTo(DateTimeOffset.MaxValue);

	[Fact]
	public void MaxValue_ForUnsupportedType_ThrowsFormatException()
	{
		var expression = new ExtendedExpression($"maxValue('unsupportedType')");
		expression.Invoking(x => x.Evaluate()).Should().Throw<FormatException>();
	}

	// Error path tests

	[Fact]
	public void MaxValue_NoParameters_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("maxValue()");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<Exception>();
	}

	[Fact]
	public void MaxValue_TwoParameters_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("maxValue('int', 'extra')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*takes exactly one string parameter*");
	}

	[Fact]
	public void MaxValue_NullParameter_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("maxValue(null)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*takes exactly one string parameter*");
	}

	[Fact]
	public void MaxValue_NumericParameter_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("maxValue(123)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*takes exactly one string parameter*");
	}

	[Fact]
	public void MaxValue_EmptyString_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("maxValue('')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*takes exactly one string parameter*");
	}

	[Fact]
	public void MaxValue_WrongCase_ThrowsFormatException()
	{
		// Type names are case-sensitive
		var expression = new ExtendedExpression("maxValue('INT')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*takes exactly one string parameter*");
	}

	[Fact]
	public void MaxValue_SystemNamespace_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("maxValue('System.Int32')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*takes exactly one string parameter*");
	}

	// Integration tests

	[Fact]
	public void MaxValue_UsedInComparison_Works()
		=> Test("1000 < maxValue('int')").Should().Be(true);

	[Fact]
	public void MaxValue_UsedInArithmetic_Works()
		=> Test("maxValue('byte') + 1").Should().Be(256);

	[Fact]
	public void MaxValue_ComparingWithVariable_Works()
	{
		var expression = new ExtendedExpression("value == maxValue('int')");
		expression.Parameters["value"] = int.MaxValue;
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void MaxValue_DifferentTypesHaveDifferentValues()
	{
		var byteMax = new ExtendedExpression("maxValue('byte')").Evaluate();
		var ushortMax = new ExtendedExpression("maxValue('ushort')").Evaluate();
		((byte)byteMax! < (ushort)ushortMax!).Should().BeTrue();
	}

	[Fact]
	public void MaxValue_DateTime_IsGreaterThanNow()
		=> Test("maxValue('DateTime') > now()").Should().Be(true);

	// Specific value validation tests

	[Fact]
	public void MaxValue_SByte_Equals127()
		=> Test("maxValue('sbyte')").Should().Be((sbyte)127);

	[Fact]
	public void MaxValue_Byte_Equals255()
		=> Test("maxValue('byte')").Should().Be((byte)255);

	[Fact]
	public void MaxValue_UShort_Equals65535()
		=> Test("maxValue('ushort')").Should().Be((ushort)65535);

	[Fact]
	public void MaxValue_Int_Equals2147483647()
		=> Test("maxValue('int')").Should().Be(2147483647);
}