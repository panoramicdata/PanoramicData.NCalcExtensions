using System.Globalization;

namespace PanoramicData.NCalcExtensions.Test;

public class IsNanTests : NCalcTest
{
	[Fact]
	public void IsNan_Example1_Succeeds()
	{
		var expression = new ExtendedExpression("isNaN(1)");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void IsNan_Example2_Succeeds()
	{
		var expression = new ExtendedExpression("isNaN(null)");
		expression.Evaluate().Should().Be(true);
	}

	[Fact]
	public void IsNan_Example3_Succeeds()
	{
		var expression = new ExtendedExpression("isNaN('text')");
		expression.Evaluate().Should().Be(true);
	}

	// Additional comprehensive tests

	[Theory]
	[InlineData("isNaN(1)", false)]
	[InlineData("isNaN(1.0)", false)]
	[InlineData("isNaN(0)", false)]
	[InlineData("isNaN(-1)", false)]
	public void IsNaN_NumericValues_ReturnsFalse(string expression, bool expected)
		=> new ExtendedExpression(expression).Evaluate().Should().Be(expected);

	[Theory]
	[InlineData("isNaN(1.0 / 0.0)", false)] // Infinity, not NaN
	[InlineData("isNaN(-1.0 / 0.0)", false)] // Negative infinity, not NaN
	public void IsNaN_InfinityValues_ReturnsFalse(string expression, bool expected)
		=> new ExtendedExpression(expression).Evaluate().Should().Be(expected);

	[Fact]
	public void IsNaN_ActualNaN_ReturnsTrue()
	{
		var expression = new ExtendedExpression("isNaN(0.0 / 0.0)");
		expression.Evaluate().Should().Be(true);
	}

	[Theory]
	[InlineData("isNaN()")]
	[InlineData("isNaN(1, 2)")]
	public void IsNaN_WrongParameterCount_ThrowsException(string expression)
		=> new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*requires one parameter*");

	[Theory]
	[InlineData("Int32")]
	[InlineData("Int64")]
	[InlineData("Int16")]
	[InlineData("Byte")]
	public void IsNaN_IntegerTypes_ReturnsFalse(string systemType)
	{
		var expression = new ExtendedExpression($"isNaN(cast(1, 'System.{systemType}'))");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void IsNaN_Float_HandlesCorrectly()
	{
		var expression = new ExtendedExpression("isNaN(cast(1.5, 'System.Single'))");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void IsNaN_Double_HandlesCorrectly()
	{
		var expression = new ExtendedExpression("isNaN(1.5)");
		expression.Evaluate().Should().Be(false);
	}

	[Fact]
	public void IsNaN_Decimal_ReturnsFalse()
	{
		var expression = new ExtendedExpression("isNaN(cast(1.5, 'System.Decimal'))");
		expression.Evaluate().Should().Be(false);
	}
}
