using System.Collections.Generic;
using System.Linq;

namespace PanoramicData.NCalcExtensions.Test;

public class SumTests : NCalcTest
{
	private static readonly List<int> IntList = [1, 2, 3];

	[Theory]
	[InlineData("sum(list(100, 100, 100), 'n', 'n')", 300)]
	[InlineData("sum(list())", 0d)]
	[InlineData("sum(list(0, 0, 0, 0))", 0)]
	[InlineData("sum(list(-1, -2, -3))", -6)]
	[InlineData("sum(list(10, -5, 3, -2))", 6)]
	[InlineData("sum(list(1000000000, 1000000000, 1000000000))", 3000000000)]
	[InlineData("sum(list(1.5, 2.5, 3.5))", 7.5)]
	[InlineData("sum(list(42))", 42)]
	[InlineData("sum(list(), 'x', 'x * 2')", 0d)]
	// One list of each numeric type, plain and with a lambda.
	[InlineData("sum(listOf('byte', 1, 2, 3))", 6)]
	[InlineData("sum(listOf('byte', 1, 2, 3), 'n', 'n * 2')", 12)]
	[InlineData("sum(listOf('short', 100, 200, 300))", 600)]
	[InlineData("sum(listOf('short', 10, 20, 30), 'n', 'n * 2')", 120)]
	[InlineData("sum(listOf('long', 1000, 2000, 3000))", 6000L)]
	[InlineData("sum(listOf('long', 100, 200, 300), 'n', 'n * 2')", 1200L)]
	// JSON numbers.
	[InlineData("sum(jArray(1, 2, 3))", 6.0)]
	public void Sum_ReturnsExpectedValue(string expressionText, object expectedOutput)
		=> Test(expressionText).Should().Be(expectedOutput);

	[Theory]
	[InlineData("sum(listOf('float', 1.1, 2.2, 3.3))", 6.6f)]
	[InlineData("sum(listOf('float', 1.0, 2.0, 3.0), 'n', 'n * 2')", 12.0f)]
	public void Sum_OfFloats_ReturnsApproximatelyExpectedValue(string expressionText, float expectedOutput)
		=> ((float)Test(expressionText)!).Should().BeApproximately(expectedOutput, 0.01f);

	[Theory]
	[InlineData("sum(listOf('double', 1.1, 2.2, 3.3))", 6.6)]
	[InlineData("sum(listOf('double', 1.0, 2.0, 3.0), 'n', 'n * 2')", 12.0)]
	[InlineData("sum(jArray(1.5, 2.5, 3.0))", 7.0)]
	public void Sum_OfDoubles_ReturnsApproximatelyExpectedValue(string expressionText, double expectedOutput)
		=> ((double)Test(expressionText)!).Should().BeApproximately(expectedOutput, 0.01);

	// Decimal results, which InlineData cannot carry as constants.

	[Fact]
	public void Sum_OfDecimals_ReturnsDecimalSum()
		=> Test("sum(listOf('decimal', 1.5, 2.5, 3.5))").Should().Be(7.5m);

	[Fact]
	public void Sum_WithLambda_OverDecimals_ReturnsDecimalSum()
		=> Test("sum(listOf('decimal', 1.5, 2.5, 3.5), 'n', 'n * 2')").Should().Be(15.0m);

	[Fact]
	public void Sum_WithLambda_OverListParameter_ReturnsExpectedValue()
		=> Test("sum(x, 'n', 'n * n')", "x", IntList).Should().Be(IntList.Sum(n => n * n));

	[Fact]
	public void Sum_OfListParameter_ReturnsExpectedValue()
		=> Test("sum(x)", "x", IntList).Should().Be(IntList.Sum());

	[Fact]
	public void Sum_OfEnumerableParameter_ReturnsExpectedValue()
		=> Test("sum(x)", "x", IntList.AsEnumerable()).Should().Be(IntList.Sum());

	[Fact]
	public void Sum_OfMixedObjectListParameter_ReturnsExpectedValue()
		=> Test("sum(x)", "x", new List<object?> { 1f, 2d, 3, null }).Should().Be(IntList.Sum());

	[Fact]
	public void Sum_OfObjectListParameterWithNulls_SkipsNulls()
		=> Test("sum(x)", "x", new List<object?> { 1, null, 2, null, 3 }).Should().Be(6.0);

	[Fact]
	public void Sum_WithLambda_OverObjectListParameter_ReturnsExpectedValue()
		=> Test("sum(x, 'x', 'x * 2')", "x", new List<object?> { 1, 2, 3 }).Should().Be(12.0);

	[Theory]
	[InlineData("sum(null)", "*cannot be null*")]
	[InlineData("sum(list(1, 2, 3), 123, 'n')", "*must be a string*")]
	[InlineData("sum(list(1, 2, 3), 'n', 456)", "*must be a string*")]
	[InlineData("sum(list('a', 'b', 'c'))", "*")]
	public void Sum_WithInvalidArguments_ThrowsFormatException(string expressionText, string messagePattern)
		=> TestShouldThrowExactly<FormatException>(expressionText, messagePattern);

	[Fact]
	public void Sum_OfObjectListParameterHoldingAString_ThrowsFormatException()
		=> TestShouldThrowExactly<FormatException>(
			"sum(x)",
			"x",
			new List<object?> { 1, 2, "invalid" },
			"*unsupported type*");

	[Fact]
	public void Sum_OfListParameterHoldingUnsupportedJTokens_ThrowsFormatException()
		=> TestShouldThrowExactly<FormatException>(
			"sum(x)",
			"x",
			new List<object?> { new JValue(true), new JValue(false) },
			"*Found unsupported JToken type*");

	[Fact]
	public void Sum_OfStringListParameter_ThrowsFormatException()
		=> TestShouldThrowExactly<FormatException>(
			"sum(x)",
			"x",
			new List<string> { "a", "b", "c" },
			"*Found unsupported type*when completing sum*");

	[Fact]
	public void Sum_WithLambda_OverStringListParameter_ThrowsFormatException()
		=> TestShouldThrowExactly<FormatException>(
			"sum(x, 'x', 'x')",
			"x",
			new List<string> { "a", "b", "c" },
			"*Found unsupported type*when completing sum*");
}
