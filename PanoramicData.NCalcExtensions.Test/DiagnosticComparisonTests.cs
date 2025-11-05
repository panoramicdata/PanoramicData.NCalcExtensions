namespace PanoramicData.NCalcExtensions.Test;

public class DiagnosticComparisonTests
{
	[Fact]
	public void Diagnostic_IntegerVsEmptyString_Equality()
	{
		var expression = new ExtendedExpression("1 == ''");
		var result = expression.Evaluate();

		// Log what we actually get
		Console.WriteLine($"1 == '' evaluates to: {result} (Type: {result?.GetType().Name})");

		// This SHOULD be false (they're not equal)
		result.Should().Be(false, "An integer should not equal an empty string");
	}

	[Fact]
	public void Diagnostic_IntegerVsEmptyString_Inequality()
	{
		// This test seems to be an NCalc bug, as it returns false instead of true
		// Relevant issue numbers:
		// - https://github.com/ncalc/ncalc/issues/471
		// - https://github.com/ncalc/ncalc/pull/489
		// - Fixed in 5.8.0


		var expression = new ExtendedExpression("1 != ''");
		var result = expression.Evaluate();

		// Log what we actually get
		Console.WriteLine($"1 != '' evaluates to: {result} (Type: {result?.GetType().Name})");

		// This SHOULD be true (they're not equal)
		result.Should().Be(true, "An integer should not equal an empty string");
	}

	[Fact]
	public void Diagnostic_IntegerVsString1_Equality()
	{
		var expression = new ExtendedExpression("1 == '1'");
		var result = expression.Evaluate();

		// Log what we actually get
		Console.WriteLine($"1 == '1' evaluates to: {result} (Type: {result?.GetType().Name})");

		// With StrictTypeMatching and NoStringTypeCoercion, this SHOULD be false
		result.Should().Be(false, "With strict type matching, int 1 should not equal string '1'");
	}

	[Fact]
	public void Diagnostic_StringVsString_Equality()
	{
		var expression = new ExtendedExpression("'a' == 'a'");
		var result = expression.Evaluate();

		// Log what we actually get
		Console.WriteLine($"'a' == 'a' evaluates to: {result} (Type: {result?.GetType().Name})");

		// This SHOULD be true
		result.Should().Be(true, "Same strings should be equal");
	}

	[Fact]
	public void Diagnostic_IntegerVsInteger_Equality()
	{
		var expression = new ExtendedExpression("1 == 1");
		var result = expression.Evaluate();

		// Log what we actually get
		Console.WriteLine($"1 == 1 evaluates to: {result} (Type: {result?.GetType().Name})");

		// This SHOULD be true
		result.Should().Be(true, "Same integers should be equal");
	}
}
