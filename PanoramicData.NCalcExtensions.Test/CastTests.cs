namespace PanoramicData.NCalcExtensions.Test;

public class CastTests
{
	[Theory]
	[InlineData("1", "System.Int32", 1)]
	[InlineData("1", "System.Int64", 1L)]
	[InlineData("1", "System.Double", 1.0)]
	[InlineData("1", "System.String", "1")]
	[InlineData("1", "System.Boolean", true)]
	[InlineData("42", "System.String", "42")]
	[InlineData("-42", "System.Int32", -42)]
	[InlineData("42.7", "System.Int32", 43)] // Convert.ChangeType rounds
	[InlineData("42", "System.Int64", 42L)]
	[InlineData("42", "System.Double", 42.0)]
	public void Cast_CommonTypes_MatchesExpectedValue(string input, string type, object expected)
	{
		var expression = new ExtendedExpression($"cast({input},'{type}')");
		expression.Evaluate().Should().Be(expected);
	}

	[Fact]
	public void Cast_ToDecimal_Succeeds()
	{
		var expression = new ExtendedExpression("cast(1, 'System.Decimal')");
		expression.Evaluate().Should().Be(1.0m);
	}

	[Fact]
	public void Cast_ToUInt32_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, 'System.UInt32')");
		expression.Evaluate().Should().Be(42u);
	}

	[Fact]
	public void Cast_ToUInt64_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, 'System.UInt64')");
		expression.Evaluate().Should().Be(42UL);
	}

	[Fact]
	public void Cast_ToInt16_Succeeds()
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
	public void Cast_ToUInt16_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, 'System.UInt16')");
		expression.Evaluate().Should().Be((ushort)42);
	}

	[Fact]
	public void Cast_ToFloat_Succeeds()
	{
		var expression = new ExtendedExpression("cast(42, 'System.Single')");
		var result = (float)expression.Evaluate()!;
		result.Should().BeApproximately(42.0f, 0.01f);
	}

	[Theory]
	[InlineData("0", false)]
	[InlineData("1", true)]
	[InlineData("99", true)]
	public void Cast_ToBool_ReturnsExpected(string input, bool expected)
	{
		var expression = new ExtendedExpression($"cast({input}, 'System.Boolean')");
		expression.Evaluate().Should().Be(expected);
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

	[Theory]
	[InlineData("cast(42, 'UnsupportedType')")]
	[InlineData("cast('notanumber', 'System.Int32')")]
	public void Cast_InvalidInput_ThrowsException(string expression) => new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should().Throw<Exception>();

	[Fact]
	public void Cast_ChainedCasts_Works()
	{
		var expression = new ExtendedExpression("cast(cast(42.7, 'System.Int32'), 'System.String')");
		expression.Evaluate().Should().Be("43");
	}

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
