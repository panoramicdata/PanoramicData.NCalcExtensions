using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace PanoramicData.NCalcExtensions.Test;

public class MinTests
{
	[Theory]
	[InlineData("1, 2, 3", 1)]
	[InlineData("3, 2, 1", 1)]
	[InlineData("1, 3, 2", 1)]
	[InlineData("1, 1, 1", 1)]
	[InlineData("1, 1, 2", 1)]
	[InlineData("1, null, 2", 1)]
	[InlineData("1.1, null, 2", 1.1d)]
	[InlineData("null, null, null", null)]

	public void Min_OfNumbers_ReturnsExpectedValue(string values, object? expectedOutput)
	{
		var expression = new ExtendedExpression($"min(listOf('double?', {values}), 'x', 'x')");
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("1, 2, 3", 1)]
	[InlineData("3, 2, 1", 1)]
	[InlineData("1, 3, 2", 1)]
	[InlineData("1, 1, 1", 1)]
	[InlineData("1, 1, 2", 1)]

	public void Min_OfListNumbers_WithLambda_ReturnsExpectedValue(string values, int expectedOutput)
	{
		var expression = new ExtendedExpression($"min(list({values}), 'x', 'x')");
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("1, 2, 3", 1)]
	[InlineData("3, 2, 1", 1)]
	[InlineData("1, 3, 2", 1)]
	[InlineData("1, 1, 1", 1)]
	[InlineData("1, 1, 2", 1)]

	public void Min_OfListNumbers_WithIEnumerable_ReturnsExpectedValue(string values, int expectedOutput)
	{
		var expression = new ExtendedExpression($"min(list({values}))");
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("'1', '2', '3'", "1")]
	[InlineData("'3', '2', '1'", "1")]
	[InlineData("'1', '3', null", "1")]
	[InlineData("'abc', 'raf', 'bbc'", "abc")]
	[InlineData("'abc', 'ABC', null", "abc")]

	public void Min_OfStrings_ReturnsExpectedValue(string values, string expectedOutput)
	{
		var expression = new ExtendedExpression($"min(list({values}))");
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[InlineData("1,2,3", "1")]
	[InlineData("3,2,1", "1")]
	[InlineData("1,3,null", "1")]
	[InlineData("abc,raf,bbc", "abc")]
	[InlineData("abc,ABC,null", "abc")]

	public void Min_OfStringsAsVariable_ReturnsExpectedValue(string values, string expectedOutput)
	{
		ArgumentException.ThrowIfNullOrEmpty(values, nameof(values));

		var expression = new ExtendedExpression($"min(valuesList)");
		expression.Parameters["valuesList"] = values.Split(',').Select(x => x == "null" ? null : x).ToList();
		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Fact]

	public void Min_OfNull_ReturnsExpectedValue()
	{
		var expression = new ExtendedExpression($"min(null)");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void Min_OfEmptyList_ReturnsNull()
	{
		var expression = new ExtendedExpression($"min(list())");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void Min_UsingLambdaForInt_ReturnsExpected()
	{
		var expression = new ExtendedExpression($"min(listOf('int', 1, 2, 3), 'x', 'x + 1')");
		expression.Evaluate().Should().Be(2);
	}

	[Fact]
	public void Min_UsingLambdaForString_ReturnsExpected()
	{
		var expression = new ExtendedExpression("min(listOf('string', '1', '2', '3'), 'x', 'x + x')");
		expression.Evaluate().Should().Be("2");
	}

	// Additional comprehensive tests for all numeric types

	[Fact]
	public void Min_ByteType_ReturnsMinByte()
	{
		var expression = new ExtendedExpression("min(listOf('byte', 255, 1, 100))");
		expression.Evaluate().Should().Be((byte)1);
	}

	[Fact]
	public void Min_SByteType_ReturnsMinSByte()
	{
		var expression = new ExtendedExpression("min(listOf('sbyte', 127, -128, 0))");
		expression.Evaluate().Should().Be((sbyte)-128);
	}

	[Fact]
	public void Min_ShortType_ReturnsMinShort()
	{
		var expression = new ExtendedExpression("min(listOf('short', 32767, -100, 100))");
		expression.Evaluate().Should().Be((short)-100);
	}

	[Fact]
	public void Min_UShortType_ReturnsMinUShort()
	{
		var expression = new ExtendedExpression("min(listOf('ushort', 65535, 1, 100))");
		expression.Evaluate().Should().Be((ushort)1);
	}

	[Fact]
	public void Min_UIntType_ReturnsMinUInt()
	{
		var expression = new ExtendedExpression("min(listOf('uint', 4294967295, 1, 100))");
		expression.Evaluate().Should().Be(1u);
	}

	[Fact]
	public void Min_LongType_ReturnsMinLong()
	{
		var expression = new ExtendedExpression("min(listOf('long', 9223372036854775807, -1000, 1000))");
		expression.Evaluate().Should().Be(-1000L);
	}

	[Fact]
	public void Min_ULongType_ReturnsMinULong()
	{
		// Note: Using values that can be safely represented in double (which NCalc uses for numeric literals)
		var expression = new ExtendedExpression("min(listOf('ulong', 100, 1, 50))");
		expression.Evaluate().Should().Be(1UL);
	}

	[Fact]
	public void Min_FloatType_ReturnsMinFloat()
	{
		var expression = new ExtendedExpression("min(listOf('float', 3.3, 1.1, 2.2))");
		var result = (float)expression.Evaluate()!;
		result.Should().BeApproximately(1.1f, 0.01f);
	}

	[Fact]
	public void Min_DecimalType_ReturnsMinDecimal()
	{
		var expression = new ExtendedExpression("min(listOf('decimal', 3.3, 1.1, 2.2))");
		expression.Evaluate().Should().Be(1.1m);
	}

	[Fact]
	public void Min_WithLambda_EmptyList_ReturnsNull()
	{
		var expression = new ExtendedExpression("min(list(), 'x', 'x')");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void Min_NullableInt_WithNulls_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('int?', 3, null, 1, null, 2))");
		expression.Evaluate().Should().Be(1);
	}

	[Fact]
	public void Min_NullableInt_AllNulls_ReturnsNull()
	{
		var expression = new ExtendedExpression("min(listOf('int?', null, null, null))");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void Min_WithLambda_ReturningNull_HandlesCorrectly()
	{
		var expression = new ExtendedExpression("min(listOf('int?', 1, 2, 3), 'x', 'if(x == 2, null, x)')");
		expression.Evaluate().Should().Be(1);
	}

	[Fact]
	public void Min_VeryLargeNumbers_HandlesCorrectly()
	{
		var expression = new ExtendedExpression("min(listOf('long', 9223372036854775806, 9223372036854775807))");
		expression.Evaluate().Should().Be(9223372036854775806L);
	}

	[Fact]
	public void Min_NegativeNumbers_ReturnsMostNegative()
	{
		var expression = new ExtendedExpression("min(listOf('int', -100, -50, -200))");
		expression.Evaluate().Should().Be(-200);
	}

	[Fact]
	public void Min_SingleElement_ReturnsThatElement()
	{
		var expression = new ExtendedExpression("min(listOf('int', 42))");
		expression.Evaluate().Should().Be(42);
	}

	[Fact]
	public void Min_WithLambda_ComplexExpression_Works()
	{
		var expression = new ExtendedExpression("min(listOf('int', 1, 2, 3), 'x', 'x * x')");
		expression.Evaluate().Should().Be(1);
	}

	// Lambda form tests for additional numeric types

	[Fact]
	public void Min_WithLambda_UIntType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('uint', 100, 50, 200), 'x', 'x')");
		expression.Evaluate().Should().Be(50u);
	}

	[Fact]
	public void Min_WithLambda_NullableUIntType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('uint?', 100, null, 50, 200), 'x', 'x')");
		expression.Evaluate().Should().Be(50u);
	}

	[Fact]
	public void Min_WithLambda_ULongType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('ulong', 1000, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be(500UL);
	}

	[Fact]
	public void Min_WithLambda_NullableULongType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('ulong?', 1000, null, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be(500UL);
	}

	[Fact]
	public void Min_WithLambda_FloatType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('float', 3.3, 1.1, 2.2), 'x', 'x')");
		var result = (float)expression.Evaluate()!;
		result.Should().BeApproximately(1.1f, 0.01f);
	}

	[Fact]
	public void Min_WithLambda_NullableFloatType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('float?', 3.3, null, 1.1, 2.2), 'x', 'x')");
		var result = (float)expression.Evaluate()!;
		result.Should().BeApproximately(1.1f, 0.01f);
	}

	[Fact]
	public void Min_WithLambda_DoubleType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('double', 3.3, 1.1, 2.2), 'x', 'x')");
		expression.Evaluate().Should().Be(1.1);
	}

	[Fact]
	public void Min_WithLambda_NullableDoubleType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('double?', 3.3, null, 1.1, 2.2), 'x', 'x')");
		expression.Evaluate().Should().Be(1.1);
	}

	[Fact]
	public void Min_WithLambda_DecimalType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('decimal', 3.3, 1.1, 2.2), 'x', 'x')");
		expression.Evaluate().Should().Be(1.1m);
	}

	[Fact]
	public void Min_WithLambda_NullableDecimalType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('decimal?', 3.3, null, 1.1, 2.2), 'x', 'x')");
		expression.Evaluate().Should().Be(1.1m);
	}

	[Fact]
	public void Min_WithLambda_SByteType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('sbyte', 10, -5, 3), 'x', 'x')");
		expression.Evaluate().Should().Be((sbyte)-5);
	}

	[Fact]
	public void Min_WithLambda_NullableSByteType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('sbyte?', 10, null, -5, 3), 'x', 'x')");
		expression.Evaluate().Should().Be((sbyte)-5);
	}

	[Fact]
	public void Min_WithLambda_ByteType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('byte', 100, 50, 200), 'x', 'x')");
		expression.Evaluate().Should().Be((byte)50);
	}

	[Fact]
	public void Min_WithLambda_NullableByteType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('byte?', 100, null, 50, 200), 'x', 'x')");
		expression.Evaluate().Should().Be((byte)50);
	}

	[Fact]
	public void Min_WithLambda_ShortType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('short', 1000, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be((short)500);
	}

	[Fact]
	public void Min_WithLambda_NullableShortType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('short?', 1000, null, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be((short)500);
	}

	[Fact]
	public void Min_WithLambda_UShortType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('ushort', 1000, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be((ushort)500);
	}

	[Fact]
	public void Min_WithLambda_NullableUShortType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('ushort?', 1000, null, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be((ushort)500);
	}

	[Fact]
	public void Min_WithLambda_LongType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('long', 1000, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be(500L);
	}

	[Fact]
	public void Min_WithLambda_NullableLongType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('long?', 1000, null, 500, 2000), 'x', 'x')");
		expression.Evaluate().Should().Be(500L);
	}

	[Fact]
	public void Min_WithLambda_StringType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('string', 'abc', 'xyz', 'def'), 'x', 'x')");
		expression.Evaluate().Should().Be("abc");
	}

	[Fact]
	public void Min_WithLambda_NullableStringType_ReturnsMin()
	{
		var expression = new ExtendedExpression("min(listOf('string?', 'abc', null, 'xyz', 'def'), 'x', 'x')");
		expression.Evaluate().Should().Be("abc");
	}

	// Error path tests

	[Fact]
	public void Min_WithInvalidFirstParameter_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("min('not a list')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*must be an IEnumerable*");
	}

	[Fact]
	public void Min_WithLambda_InvalidSecondParameter_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("min(list(1, 2, 3), 123, 'x')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*Second*parameter must be a string*");
	}

	[Fact]
	public void Min_WithLambda_InvalidThirdParameter_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("min(list(1, 2, 3), 'x', 456)");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*Third*parameter must be a string*");
	}

	// Test for GetMin with unsupported type
	[Fact]
	public void Min_WithUnsupportedType_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("min(unsupportedList)");
		expression.Parameters["unsupportedList"] = new List<object?> { new System.DateTime(2024, 1, 1), new System.DateTime(2024, 1, 2) };
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*Found unsupported type*");
	}

	// Test for GetMin with unsupported JToken type (not Float, Integer, or String)
	[Fact]
	public void Min_WithUnsupportedJTokenType_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("min(jValueList)");
		// Create a JValue with an unsupported type (e.g., Boolean or Date)
		expression.Parameters["jValueList"] = new List<object?> { new JValue(true), new JValue(false) };
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*Found unsupported JToken type*");
	}

	// Test for invalid IEnumerable type in single parameter mode
	[Fact]
	public void Min_WithUnsupportedEnumerableType_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("min(dateList)");
		expression.Parameters["dateList"] = new List<DateTime> { DateTime.Now, DateTime.Now.AddDays(1) };
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*must be an IEnumerable of a numeric or string type*");
	}

	// Test for invalid IEnumerable type in lambda mode
	[Fact]
	public void Min_WithLambda_UnsupportedEnumerableType_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("min(dateList, 'x', 'x')");
		expression.Parameters["dateList"] = new List<DateTime> { DateTime.Now, DateTime.Now.AddDays(1) };
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage("*must be an IEnumerable of a string or numeric type when processing as a lambda*");
	}
}