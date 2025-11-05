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
	public void MinValue_ForDateTime_ReturnsExpectedValue()
	{
		var expression = new ExtendedExpression($"minValue('DateTime')");
		expression.Evaluate().Should().BeEquivalentTo(DateTime.MinValue);
	}

	[Fact]
	public void MinValue_ForDateTimeOffset_ReturnsExpectedValue()
	{
		var expression = new ExtendedExpression($"minValue('DateTimeOffset')");
		expression.Evaluate().Should().BeEquivalentTo(DateTimeOffset.MinValue);
	}

	[Fact]
	public void MinValue_ForUnsupportedType_ThrowsFormatException()
	{
		var expression = new ExtendedExpression($"minValue('unsupportedType')");
		expression.Invoking(x => x.Evaluate()).Should().Throw<FormatException>();
	}

	// Error path tests

	[Fact]
	public void MinValue_NoParameters_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("minValue()");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<Exception>();
	}

	[Fact]
	public void MinValue_TwoParameters_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("minValue('int', 'extra')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*takes exactly one string parameter*");
	}

	[Fact]
	public void MinValue_NullParameter_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("minValue(null)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*takes exactly one string parameter*");
	}

	[Fact]
	public void MinValue_NumericParameter_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("minValue(123)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*takes exactly one string parameter*");
	}

	[Fact]
	public void MinValue_EmptyString_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("minValue('')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*takes exactly one string parameter*");
	}

	[Fact]
	public void MinValue_WrongCase_ThrowsFormatException()
	{
		// Type names are case-sensitive
		var expression = new ExtendedExpression("minValue('INT')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*takes exactly one string parameter*");
	}

	[Fact]
	public void MinValue_SystemNamespace_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("minValue('System.Int32')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*takes exactly one string parameter*");
	}

	// Integration tests

	[Fact]
	public void MinValue_UsedInComparison_Works()
	{
		var expression = new ExtendedExpression("-1000 > minValue('int')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void MinValue_UsedInArithmetic_Works()
	{
		var expression = new ExtendedExpression("minValue('byte') - 1");
		expression.Evaluate().Should().Be(-1);
	}

	[Fact]
	public void MinValue_ComparingWithVariable_Works()
	{
		var expression = new ExtendedExpression("value == minValue('int')");
		expression.Parameters["value"] = int.MinValue;
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void MinValue_DifferentTypesHaveDifferentValues()
	{
		var sbyteMin = new ExtendedExpression("minValue('sbyte')").Evaluate();
		var byteMin = new ExtendedExpression("minValue('byte')").Evaluate();
		((sbyte)sbyteMin! < (byte)byteMin!).Should().BeTrue();
	}

	[Fact]
	public void MinValue_DateTime_IsLessThanNow()
	{
		var expression = new ExtendedExpression("minValue('DateTime') < now()");
		expression.Evaluate().Should().Be(true);
	}

	// Specific value validation tests

	[Fact]
	public void MinValue_SByte_EqualsMinus128()
	{
		var expression = new ExtendedExpression("minValue('sbyte')");
		expression.Evaluate().Should().Be((sbyte)-128);
	}

	[Fact]
	public void MinValue_Byte_Equals0()
	{
		var expression = new ExtendedExpression("minValue('byte')");
		expression.Evaluate().Should().Be((byte)0);
	}

	[Fact]
	public void MinValue_UInt_Equals0()
	{
		var expression = new ExtendedExpression("minValue('uint')");
		expression.Evaluate().Should().Be(0u);
	}

	[Fact]
	public void MinValue_Int_EqualsMinus2147483648()
	{
		var expression = new ExtendedExpression("minValue('int')");
		expression.Evaluate().Should().Be(-2147483648);
	}
}