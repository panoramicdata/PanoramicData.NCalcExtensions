using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

/// <summary>
/// The shared shape of the max() and min() tests. The two functions are mirror images of one
/// another, so each concrete class supplies only the rows; the assertions live here.
/// </summary>
// The case rows live on each concrete class, so they are named as strings rather than with
// nameof: the base class cannot see its subclasses' members.
public abstract class ExtremumTests : NCalcTest
{
	/// <summary>
	/// The function under test, as it is written in an expression.
	/// </summary>
	protected abstract string Function { get; }

	/// <summary>
	/// Results compared for equivalence, so that the boxed numeric width does not matter.
	/// </summary>
	[Theory]
	[MemberData("EquivalentCases")]
	public void Extremum_ReturnsEquivalentValue(string expressionText, object? expectedOutput)
		=> Test(expressionText).Should().BeEquivalentTo(expectedOutput);

	/// <summary>
	/// Results compared exactly, so that the type of the result matters.
	/// </summary>
	[Theory]
	[MemberData("ExactCases")]
	public void Extremum_ReturnsExactValue(string expressionText, object? expectedOutput)
		=> Test(expressionText).Should().Be(expectedOutput);

	/// <summary>
	/// Float results, which are compared to a tolerance.
	/// </summary>
	[Theory]
	[MemberData("FloatCases")]
	public void Extremum_OfFloats_ReturnsApproximatelyExpectedValue(string expressionText, float expectedOutput)
		=> ((float)Test(expressionText)!).Should().BeApproximately(expectedOutput, 0.01f);

	/// <summary>
	/// A list of strings supplied as a parameter rather than built within the expression, with
	/// "null" standing for a null entry.
	/// </summary>
	[Theory]
	[MemberData("StringListParameterCases")]
	public void Extremum_OfStringListParameter_ReturnsExpectedValue(string values, string expectedOutput)
	{
		var expression = new ExtendedExpression($"{Function}(valuesList)");
		expression.Parameters["valuesList"] = values
			.Split(',')
			.Select(value => value == "null" ? null : value)
			.ToList();

		expression.Evaluate().Should().BeEquivalentTo(expectedOutput);
	}

	[Theory]
	[MemberData("FailureCases")]
	public void Extremum_WithInvalidArguments_ThrowsFormatException(string expressionText, string messagePattern)
		=> new ExtendedExpression(expressionText)
			.Invoking(expression => expression.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage(messagePattern);

	[Fact]
	public void Extremum_OfListOfUnsupportedType_ThrowsFormatException()
		=> ShouldFailWith(
			$"{Function}(valuesList)",
			new List<object?> { new DateTime(2024, 1, 1), new DateTime(2024, 1, 2) },
			"*Found unsupported type*");

	[Fact]
	public void Extremum_OfListOfUnsupportedJTokenType_ThrowsFormatException()
		=> ShouldFailWith(
			$"{Function}(valuesList)",
			new List<object?> { new JValue(true), new JValue(false) },
			"*Found unsupported JToken type*");

	[Fact]
	public void Extremum_OfUnsupportedEnumerableType_ThrowsFormatException()
		=> ShouldFailWith(
			$"{Function}(valuesList)",
			new List<DateTime> { DateTime.Now, DateTime.Now.AddDays(1) },
			"*must be an IEnumerable of a numeric or string type*");

	[Fact]
	public void Extremum_WithLambda_OfUnsupportedEnumerableType_ThrowsFormatException()
		=> ShouldFailWith(
			$"{Function}(valuesList, 'x', 'x')",
			new List<DateTime> { DateTime.Now, DateTime.Now.AddDays(1) },
			"*must be an IEnumerable of a string or numeric type when processing as a lambda*");

	/// <summary>
	/// Asserts that <paramref name="expressionText"/>, evaluated over a "valuesList" parameter,
	/// fails with a FormatException matching <paramref name="messagePattern"/>.
	/// </summary>
	private static void ShouldFailWith(string expressionText, object valuesList, string messagePattern)
	{
		var expression = new ExtendedExpression(expressionText);
		expression.Parameters["valuesList"] = valuesList;

		expression.Invoking(x => x.Evaluate())
			.Should().Throw<FormatException>()
			.WithMessage(messagePattern);
	}
}
