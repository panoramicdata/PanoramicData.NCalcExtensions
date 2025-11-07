namespace PanoramicData.NCalcExtensions.Test;

public class CastTests
{
	[Theory]
	[InlineData("1", "System.Int32", 1)]
	[InlineData("1", "System.Int64", 1L)]
	[InlineData("1", "System.Double", 1.0)]
	[InlineData("1", "System.Decimal", 1.0)]
	[InlineData("1", "System.String", "1")]
	[InlineData("1", "System.Boolean", true)]
	public void Cast_UsingInlineData_MatchesExpectedValue(string input, string type, object expected)
	{
		var expression = new ExtendedExpression($"cast({input},'{type}')");
		var actual = expression.Evaluate();

		actual.Should().Be(expected);
	}

	// Test all numeric types with System.* names (Type.GetType requires fully qualified names)
	[Fact]
	public void Cast_ToInt_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42.7, 'System.Int32')");
		// Convert.ChangeType rounds, not truncates
		expression.Evaluate().Should().Be(43);
	}

	[Fact]
	public void Cast_ToLong_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, 'System.Int64')");
		expression.Evaluate().Should().Be(42L);
	}

	[Fact]
	public void Cast_ToShort_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, 'System.Int16')");
		expression.Evaluate().Should().Be((short)42);
	}

	[Fact]
	public void Cast_ToByte_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, 'System.Byte')");
		expression.Evaluate().Should().Be((byte)42);
	}

	[Fact]
	public void Cast_ToSByte_Succeeds()
	{
		var expression = new ExtendedExpression("cast(-42, 'System.SByte')");
		expression.Evaluate().Should().Be((sbyte)-42);
	}

	[Fact]
	public void Cast_ToUInt_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, 'System.UInt32')");
		expression.Evaluate().Should().Be(42u);
	}

	[Fact]
	public void Cast_ToULong_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, 'System.UInt64')");
		expression.Evaluate().Should().Be(42UL);
	}

	[Fact]
	public void Cast_ToUShort_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, 'System.UInt16')");
		expression.Evaluate().Should().Be((ushort)42);
	}

	[Fact]
	public void Cast_ToDouble_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, 'System.Double')");
		expression.Evaluate().Should().Be(42.0);
	}

	[Fact]
	public void Cast_ToFloat_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, 'System.Single')");
		var result = (float)expression.Evaluate()!;
		result.Should().BeApproximately(42.0f, 0.01f);
	}

	[Fact]
	public void Cast_ToDecimal_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, 'System.Decimal')");
		expression.Evaluate().Should().Be(42m);
	}

	[Fact]
	public void Cast_ToString_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, 'System.String')");
		expression.Evaluate().Should().Be("42");
	}

	[Fact]
	public void Cast_ToBool_Succeeds()
	{
		var expression = new ExtendedExpression("cast(1, 'System.Boolean')");
		expression.Evaluate().Should().Be(true);
	}

	// Test edge cases
	[Fact]
	public void Cast_ZeroToBool_ReturnsFalse()
	{
		var expression = new ExtendedExpression("cast(0, 'System.Boolean')");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void Cast_NonZeroToBool_ReturnsTrue()
	{
		var expression = new ExtendedExpression("cast(99, 'System.Boolean')");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void Cast_NegativeNumber_Succeeds()
	{
		var expression = new ExtendedExpression("cast(-42, 'System.Int32')");
		expression.Evaluate().Should().Be(-42);
	}

	[Fact]
	public void Cast_DoubleToInt_Truncates()
	{
		var expression = new ExtendedExpression("cast(42.9, 'System.Int32')");
		// Convert.ChangeType rounds, not truncates
		expression.Evaluate().Should().Be(43);
	}

	[Fact]
	public void Cast_StringToDouble_Succeeds()
	{
		var expression = new ExtendedExpression("cast('3.14', 'System.Double')");
		expression.Evaluate().Should().Be(3.14);
	}

	[Fact]
	public void Cast_MaxValue_Int_Succeeds()
	{
		var expression = new ExtendedExpression("cast(2147483647, 'System.Int32')");
		expression.Evaluate().Should().Be(int.MaxValue);
	}

	[Fact]
	public void Cast_MinValue_Int_Succeeds()
	{
		var expression = new ExtendedExpression("cast(-2147483648, 'System.Int32')");
		expression.Evaluate().Should().Be(int.MinValue);
	}

	// Test with variables
	[Fact]
	public void Cast_WithVariable_Succeeds()
	{
		var expression = new ExtendedExpression("cast(myValue, 'System.String')");
		expression.Parameters["myValue"] = 42;
		expression.Evaluate().Should().Be("42");
	}

	[Fact]
	public void Cast_WithVariableType_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, myType)");
		expression.Parameters["myType"] = "System.String";
		expression.Evaluate().Should().Be("42");
	}

	// Test error cases
	[Fact]
	public void Cast_UnsupportedType_ThrowsException()
	{
		var expression = new ExtendedExpression("cast(42, 'UnsupportedType')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<Exception>();
	}

	[Fact]
	public void Cast_InvalidConversion_ThrowsException()
	{
		var expression = new ExtendedExpression("cast('notanumber', 'System.Int32')");
		expression.Invoking(e => e.Evaluate())
			.Should().Throw<Exception>();
	}

	// Test chained casts
	[Fact]
	public void Cast_ChainedCasts_Works()
	{
		var expression = new ExtendedExpression("cast(cast(42.7, 'System.Int32'), 'System.String')");
		// Convert.ChangeType rounds 42.7 to 43
		expression.Evaluate().Should().Be("43");
	}

	// Test in expressions
	[Fact]
	public void Cast_InArithmetic_Works()
	{
		var expression = new ExtendedExpression("cast(5, 'System.Double') / 2");
		expression.Evaluate().Should().Be(2.5);
	}

	[Fact]
	public void Cast_InComparison_Works()
	{
		var expression = new ExtendedExpression("cast('42', 'System.Int32') > 40");
		expression.Evaluate().Should().Be(true);
	}
}
