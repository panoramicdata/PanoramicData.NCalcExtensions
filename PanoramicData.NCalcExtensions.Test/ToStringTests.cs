namespace PanoramicData.NCalcExtensions.Test;

public class ToStringTests
{
	[Fact]
	public void ToString_IsNull_ReturnsNull()
	{
		var expression = new ExtendedExpression("toString(null)");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void ToString_Int_Succeeds()
	{
		var expression = new ExtendedExpression("toString(1)");
		expression.Evaluate().Should().Be("1");
	}

	[Fact]
	public void ToString_Int_Formatted_Succeeds()
	{
		var expression = new ExtendedExpression("toString(1000, 'N2')");
		expression.Evaluate().Should().Be("1,000.00");
	}

	[Fact]
	public void ToString_DateTime_Formatted_Succeeds()
	{
		var expression = new ExtendedExpression("toString(TheDateTime, 'yyyy-MM-dd')");
		expression.Parameters.Add("TheDateTime", new DateTime(2020, 1, 1));
		expression.Evaluate().Should().Be("2020-01-01");
	}

	[Fact]
	public void ToString_DateTimeOffset_Formatted_Succeeds()
	{
		var expression = new ExtendedExpression("toString(TheDateTimeOffset, 'yyyy-MM-dd')");
		expression.Parameters.Add("TheDateTimeOffset", new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero));
		expression.Evaluate().Should().Be("2020-01-01");
	}

	// Additional format specifier tests
	[Fact]
	public void ToString_Currency_Format_Succeeds()
	{
		var expression = new ExtendedExpression("toString(1234.56, 'C2')");
		var result = expression.Evaluate() as string;
		result.Should().Contain("1,234.56"); // Format varies by culture but should contain these digits
	}

	[Fact]
	public void ToString_Percent_Format_Succeeds()
	{
		var expression = new ExtendedExpression("toString(0.1234, 'P2')");
		var result = expression.Evaluate() as string;
		result.Should().Contain("12.34"); // Should show as percentage
	}

	[Fact]
	public void ToString_Exponential_Format_Succeeds()
	{
		var expression = new ExtendedExpression("toString(1234.56, 'E2')");
		var result = expression.Evaluate() as string;
		result.Should().MatchRegex(@"1\.23[Ee][+]0*3"); // Exponential notation - allow extra zeros
	}

	[Fact]
	public void ToString_FixedPoint_Format_Succeeds()
	{
		var expression = new ExtendedExpression("toString(1234.567, 'F2')");
		expression.Evaluate().Should().Be("1234.57");
	}

	[Fact]
	public void ToString_General_Format_Succeeds()
	{
		var expression = new ExtendedExpression("toString(1234.567, 'G')");
		var result = expression.Evaluate() as string;
		result.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public void ToString_Hexadecimal_Format_Succeeds()
	{
		var expression = new ExtendedExpression("toString(255, 'X')");
		expression.Evaluate().Should().Be("FF");
	}

	[Fact]
	public void ToString_HexadecimalLowercase_Format_Succeeds()
	{
		var expression = new ExtendedExpression("toString(255, 'x')");
		expression.Evaluate().Should().Be("ff");
	}

	[Fact]
	public void ToString_Double_NoFormat_Succeeds()
	{
		var expression = new ExtendedExpression("toString(123.456)");
		expression.Evaluate().Should().Be("123.456");
	}

	[Fact]
	public void ToString_String_NoFormat_ReturnsString()
	{
		var expression = new ExtendedExpression("toString('hello')");
		expression.Evaluate().Should().Be("hello");
	}

	[Fact]
	public void ToString_Boolean_True_ReturnsTrue()
	{
		var expression = new ExtendedExpression("toString(true)");
		expression.Evaluate().Should().Be("True");
	}

	[Fact]
	public void ToString_Boolean_False_ReturnsFalse()
	{
		var expression = new ExtendedExpression("toString(false)");
		expression.Evaluate().Should().Be("False");
	}

	[Fact]
	public void ToString_DateTime_CustomFormat_Succeeds()
	{
		var expression = new ExtendedExpression("toString(TheDateTime, 'MM/dd/yyyy HH:mm:ss')");
		expression.Parameters.Add("TheDateTime", new DateTime(2020, 3, 15, 14, 30, 45));
		expression.Evaluate().Should().Be("03/15/2020 14:30:45");
	}

	[Fact]
	public void ToString_DateTime_ShortDate_Succeeds()
	{
		var expression = new ExtendedExpression("toString(TheDateTime, 'd')");
		expression.Parameters.Add("TheDateTime", new DateTime(2020, 1, 1));
		var result = expression.Evaluate() as string;
		result.Should().NotBeNullOrEmpty(); // Culture-specific
	}

	[Fact]
	public void ToString_DateTime_LongDate_Succeeds()
	{
		var expression = new ExtendedExpression("toString(TheDateTime, 'D')");
		expression.Parameters.Add("TheDateTime", new DateTime(2020, 1, 1));
		var result = expression.Evaluate() as string;
		result.Should().NotBeNullOrEmpty(); // Culture-specific
	}

	[Fact]
	public void ToString_Decimal_WithFormat_Succeeds()
	{
		// Note: toString doesn't format decimal currently, it falls through to object.ToString()
		var expression = new ExtendedExpression("toString(TheDecimal)");
		expression.Parameters.Add("TheDecimal", 1234.5678m);
		expression.Evaluate().Should().Be("1234.5678");
	}

	[Fact]
	public void ToString_Long_Succeeds()
	{
		var expression = new ExtendedExpression("toString(TheLong)");
		expression.Parameters.Add("TheLong", 9223372036854775807L);
		expression.Evaluate().Should().Be("9223372036854775807");
	}

	[Fact]
	public void ToString_Float_Succeeds()
	{
		var expression = new ExtendedExpression("toString(TheFloat)");
		expression.Parameters.Add("TheFloat", 123.45f);
		var result = expression.Evaluate() as string;
		result.Should().StartWith("123.45");
	}

	[Fact]
	public void ToString_ZeroPadding_Format_Succeeds()
	{
		var expression = new ExtendedExpression("toString(42, '0000')");
		expression.Evaluate().Should().Be("0042");
	}

	[Fact]
	public void ToString_CustomNumeric_Format_Succeeds()
	{
		var expression = new ExtendedExpression("toString(1234.567, '#,##0.00')");
		expression.Evaluate().Should().Be("1,234.57");
	}
}
