using System.Linq;

namespace PanoramicData.NCalcExtensions.Test;

public class MaxTests
{
	[Theory]
	[InlineData("1, 2, 3", 3)]
	[InlineData("3, 2, 1", 3)]
	[InlineData("1, 3, 2", 3)]
	[InlineData("1, 1, 1", 1)]
	[InlineData("1, 1, 2", 2)]
	[InlineData("1, null, 2", 2)]
	[InlineData("1.1, null, 2", 2)]
	[InlineData("null, null, null", null)]

	public void Max_OfListOfNullableDoubles_ReturnsExpectedValue(string values, object? expectedOutput)
	{
		var expression = new ExtendedExpression($"max(listOf('double?', {values}), 'x', 'x')");
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("1, 2, 3", 3)]
	[InlineData("3, 2, 1", 3)]
	[InlineData("1, 3, 2", 3)]
	[InlineData("1, 1, 1", 1)]
	[InlineData("1, 1, 2", 2)]

	public void Max_OfListNumbers_WithLambda_ReturnsExpectedValue(string values, int expectedOutput)
	{
		var expression = new ExtendedExpression($"max(list({values}), 'x', 'x')");
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("1, 2, 3", 3)]
	[InlineData("3, 2, 1", 3)]
	[InlineData("1, 3, 2", 3)]
	[InlineData("1, 1, 1", 1)]
	[InlineData("1, 1, 2", 2)]

	public void Max_OfListNumbers_WithIEnumerable_ReturnsExpectedValue(string values, int expectedOutput)
	{
		var expression = new ExtendedExpression($"max(list({values}))");
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("'1', '2', '3'", "3")]
	[InlineData("'3', '2', '1'", "3")]
	[InlineData("'1', '3', null", "3")]
	[InlineData("'abc', 'raf', 'bbc'", "raf")]
	[InlineData("'abc', 'ABC', null", "ABC")]

	public void Max_OfStrings_ReturnsExpectedValue(string values, string expectedOutput)
	{
		var expression = new ExtendedExpression($"max(list({values}))");
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("1,2,3", "3")]
	[InlineData("3,2,1", "3")]
	[InlineData("1,3,null", "3")]
	[InlineData("abc,raf,bbc", "raf")]
	[InlineData("abc,ABC,null", "ABC")]

	public void Max_OfStringsAsVariable_ReturnsExpectedValue(string values, string expectedOutput)
	{
		ArgumentException.ThrowIfNullOrEmpty(values, nameof(values));

		var expression = new ExtendedExpression($"max(valuesList)");
		expression.Parameters["valuesList"] = values.Split(',').Select(x => x == "null" ? null : x).ToList();
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Fact]

	public void Max_OfNull_ReturnsExpectedValue()
	{
		var expression = new ExtendedExpression($"max(null)");
		expression.Evaluate().Should().BeNull();
	}


	[Fact]
	public void Max_OfEmptyList_ReturnsNull()
	{
		var expression = new ExtendedExpression($"max(list())");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void Max_UsingLambdaForInt_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"max(listOf('int', 1, 2, 3), 'x', 'x + 1')");
		expression.Evaluate().Should().Be(4);
	}

	[Fact]
	public void Max_UsingLambdaForString_ReturnsExpected()
	{
		var expression = new ExtendedExpression("max(listOf('string', '1', '2', '3'), 'x', 'x + x')");
		expression.Evaluate().Should().Be("33");
	}

	// Additional comprehensive tests for all numeric types

	[Fact]
	public void Max_ByteType_ReturnsMaxByte()
	{
		var expression = new ExtendedExpression("max(listOf('byte', 1, 255, 100))");
		expression.Evaluate().Should().Be((byte)255);
	}

	[Fact]
	public void Max_SByteType_ReturnsMaxSByte()
	{
		var expression = new ExtendedExpression("max(listOf('sbyte', -128, 127, 0))");
		expression.Evaluate().Should().Be((sbyte)127);
	}

	[Fact]
	public void Max_ShortType_ReturnsMaxShort()
	{
		var expression = new ExtendedExpression("max(listOf('short', -100, 32767, 100))");
		expression.Evaluate().Should().Be((short)32767);
	}

	[Fact]
	public void Max_UShortType_ReturnsMaxUShort()
	{
		var expression = new ExtendedExpression("max(listOf('ushort', 1, 65535, 100))");
		expression.Evaluate().Should().Be((ushort)65535);
	}

	[Fact]
	public void Max_UIntType_ReturnsMaxUInt()
	{
		var expression = new ExtendedExpression("max(listOf('uint', 1, 4294967295, 100))");
		expression.Evaluate().Should().Be(4294967295u);
	}

	[Fact]
	public void Max_LongType_ReturnsMaxLong()
	{
		var expression = new ExtendedExpression("max(listOf('long', -1000, 9223372036854775807, 1000))");
		expression.Evaluate().Should().Be(9223372036854775807L);
	}

	[Fact]
	public void Max_ULongType_ReturnsMaxULong()
	{
		// Note: Using values that can be safely represented in double (which NCalc uses for numeric literals)
		var expression = new ExtendedExpression("max(listOf('ulong', 1, 9999999999999, 100))");
		expression.Evaluate().Should().Be(9999999999999UL);
	}

	[Fact]
	public void Max_FloatType_ReturnsMaxFloat()
	{
		var expression = new ExtendedExpression("max(listOf('float', 1.1, 2.2, 3.3))");
		var result = (float)expression.Evaluate()!;
		result.Should().BeApproximately(3.3f, 0.01f);
	}

	[Fact]
	public void Max_DecimalType_ReturnsMaxDecimal()
	{
		var expression = new ExtendedExpression("max(listOf('decimal', 1.1, 2.2, 3.3))");
		expression.Evaluate().Should().Be(3.3m);
	}

	[Fact]
	public void Max_WithLambda_EmptyList_ReturnsNull()
	{
		var expression = new ExtendedExpression("max(list(), 'x', 'x')");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void Max_NullableInt_WithNulls_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('int?', 1, null, 3, null, 2))");
		expression.Evaluate().Should().Be(3);
	}

	[Fact]
	public void Max_NullableInt_AllNulls_ReturnsNull()
	{
		var expression = new ExtendedExpression("max(listOf('int?', null, null, null))");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void Max_WithLambda_ReturningNull_HandlesCorrectly()
	{
		var expression = new ExtendedExpression("max(listOf('int?', 1, 2, 3), 'x', 'if(x == 2, null, x)')");
		expression.Evaluate().Should().Be(3);
	}

	[Fact]
	public void Max_VeryLargeNumbers_HandlesCorrectly()
	{
		var expression = new ExtendedExpression("max(listOf('long', 9223372036854775806, 9223372036854775807))");
		expression.Evaluate().Should().Be(9223372036854775807L);
	}

	[Fact]
	public void Max_NegativeNumbers_ReturnsLeastNegative()
	{
		var expression = new ExtendedExpression("max(listOf('int', -100, -50, -200))");
		expression.Evaluate().Should().Be(-50);
	}

	[Fact]
	public void Max_SingleElement_ReturnsThatElement()
	{
		var expression = new ExtendedExpression("max(listOf('int', 42))");
		expression.Evaluate().Should().Be(42);
	}

	[Fact]
	public void Max_WithLambda_ComplexExpression_Works()
	{
		var expression = new ExtendedExpression("max(listOf('int', 1, 2, 3), 'x', 'x * x')");
		expression.Evaluate().Should().Be(9);
	}

	// Lambda form tests for additional numeric types

	[Fact]
	public void Max_WithLambda_UIntType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('uint', 100, 50, 200), 'x', 'x')");
		expression.Evaluate().Should().Be(200u);
	}

	[Fact]
	public void Max_WithLambda_NullableUIntType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('uint?', 100, null, 50, 200), 'x', 'x')");
		expression.Evaluate().Should().Be(200u);
	}

	[Fact]
	public void Max_WithLambda_ULongType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('ulong', 1000, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be(2000UL);
	}

	[Fact]
	public void Max_WithLambda_NullableULongType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('ulong?', 1000, null, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be(2000UL);
	}

	[Fact]
	public void Max_WithLambda_FloatType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('float', 3.3, 1.1, 2.2), 'x', 'x')");
		var result = (float)expression.Evaluate()!;
		result.Should().BeApproximately(3.3f, 0.01f);
	}

	[Fact]
	public void Max_WithLambda_NullableFloatType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('float?', 3.3, null, 1.1, 2.2), 'x', 'x')");
		var result = (float)expression.Evaluate()!;
		result.Should().BeApproximately(3.3f, 0.01f);
	}

	[Fact]
	public void Max_WithLambda_DoubleType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('double', 3.3, 1.1, 2.2), 'x', 'x')");
		expression.Evaluate().Should().Be(3.3);
	}

	[Fact]
	public void Max_WithLambda_NullableDoubleType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('double?', 3.3, null, 1.1, 2.2), 'x', 'x')");
		expression.Evaluate().Should().Be(3.3);
	}

	[Fact]
	public void Max_WithLambda_DecimalType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('decimal', 3.3, 1.1, 2.2), 'x', 'x')");
		expression.Evaluate().Should().Be(3.3m);
	}

	[Fact]
	public void Max_WithLambda_NullableDecimalType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('decimal?', 3.3, null, 1.1, 2.2), 'x', 'x')");
		expression.Evaluate().Should().Be(3.3m);
	}

	[Fact]
	public void Max_WithLambda_SByteType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('sbyte', 10, -5, 3), 'x', 'x')");
		expression.Evaluate().Should().Be((int)10);
	}

	[Fact]
	public void Max_WithLambda_NullableSByteType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('sbyte?', 10, null, -5, 3), 'x', 'x')");
		expression.Evaluate().Should().Be((int)10);
	}

	[Fact]
	public void Max_WithLambda_ByteType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('byte', 100, 50, 200), 'x', 'x')");
		expression.Evaluate().Should().Be((int)200);
	}

	[Fact]
	public void Max_WithLambda_NullableByteType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('byte?', 100, null, 50, 200), 'x', 'x')");
		expression.Evaluate().Should().Be((int)200);
	}

	[Fact]
	public void Max_WithLambda_ShortType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('short', 1000, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be((int)2000);
	}

	[Fact]
	public void Max_WithLambda_NullableShortType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('short?', 1000, null, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be((int)2000);
	}

	[Fact]
	public void Max_WithLambda_UShortType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('ushort', 1000, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be((int)2000);
	}

	[Fact]
	public void Max_WithLambda_NullableUShortType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('ushort?', 1000, null, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be((int)2000);
	}

	[Fact]
	public void Max_WithLambda_LongType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('long', 1000, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be(2000L);
	}

	[Fact]
	public void Max_WithLambda_NullableLongType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('long?', 1000, null, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be(2000L);
	}

	[Fact]
	public void Max_WithLambda_StringType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('string', 'abc', 'xyz', 'def'), 'x', 'x')");
		expression.Evaluate().Should().Be("xyz");
	}

	[Fact]
	public void Max_WithLambda_NullableStringType_ReturnsMax()
	{
		var expression = new ExtendedExpression("max(listOf('string?', 'abc', null, 'xyz', 'def'), 'x', 'x')");
		expression.Evaluate().Should().Be("xyz");
	}

	// Error path tests

	[Fact]
	public void Max_WithInvalidFirstParameter_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("max('not a list')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*must be an IEnumerable*");
	}

	[Fact]
	public void Max_WithLambda_InvalidSecondParameter_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("max(list(1, 2, 3), 123, 'x')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*Second*parameter must be a string*");
	}

	[Fact]
	public void Max_WithLambda_InvalidThirdParameter_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("max(list(1, 2, 3), 'x', 456)")
;
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*Third*parameter must be a string*");
	}
}
