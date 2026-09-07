using System.Globalization;

namespace PanoramicData.NCalcExtensions.Test;

public class IsNanTests : NCalcTest
{
	[Fact]
	public void IsNan_Example1_Succeeds()
		=> Test("isNaN(1)").Should().Be(false);

	[Fact]
	public void IsNan_Example2_Succeeds()
		=> Test("isNaN(null)").Should().Be(true);

	[Fact]
	public void IsNan_Example3_Succeeds()
		=> Test("isNaN('text')").Should().Be(true);

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
		=> Test("isNaN(0.0 / 0.0)").Should().Be(true);

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
		=> Test($"isNaN(cast(1, 'System.{systemType}'))").Should().Be(false);

	[Fact]
	public void IsNaN_Float_HandlesCorrectly()
		=> Test("isNaN(cast(1.5, 'System.Single'))").Should().Be(false);

	[Fact]
	public void IsNaN_Double_HandlesCorrectly()
		=> Test("isNaN(1.5)").Should().Be(false);

	[Fact]
	public void IsNaN_Decimal_ReturnsFalse()
		=> Test("isNaN(cast(1.5, 'System.Decimal'))").Should().Be(false);
}
