using NCalc.Exceptions;

namespace PanoramicData.NCalcExtensions.Test;

/// <summary>
/// Issue #65: the Is* predicates convert an unexpected evaluation failure into a <see cref="FormatException"/>.
/// They must pass the original exception through as <see cref="Exception.InnerException"/>, or the caller has
/// no way to tell *why* evaluation failed.
///
/// <para>
/// The motivating case: an unbound parameter raises <see cref="NCalcParameterNotDefinedException"/>. Referenced
/// directly, a caller can catch that type. Referenced from inside one of these functions it became a bare
/// <see cref="FormatException"/> with a null InnerException, leaving message text as the only signal - which is
/// indistinguishable from a genuine formatting failure such as parsing a non-numeric string.
/// </para>
/// </summary>
public class InnerExceptionPreservationTests
{
	private const string UnboundParameterName = "someUnboundField";

	public static TheoryData<string> IsFunctions => new(
		"isNullOrWhiteSpace",
		"isNullOrEmpty",
		"isNull",
		"isNaN",
		"isInfinite");

	/// <summary>
	/// The type of the original failure must survive the conversion.
	/// </summary>
	[Theory]
	[MemberData(nameof(IsFunctions))]
	public void IsFunction_WhenTheParameterIsUnbound_PreservesTheOriginalExceptionAsInner(string functionName)
		=> new ExtendedExpression($"{functionName}({UnboundParameterName})")
			.Invoking(x => x.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithInnerExceptionExactly<NCalcParameterNotDefinedException>();

	/// <summary>
	/// The outer type and message are deliberately unchanged, so anything already catching FormatException
	/// or matching on its text keeps working. This is what makes the fix safe to take.
	/// </summary>
	[Theory]
	[MemberData(nameof(IsFunctions))]
	public void IsFunction_WhenTheParameterIsUnbound_KeepsTheOuterTypeAndMessage(string functionName)
		=> new ExtendedExpression($"{functionName}({UnboundParameterName})")
			.Invoking(x => x.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage($"Parameter {UnboundParameterName} not defined.");

	/// <summary>
	/// The wrong-parameter-count guard throws FormatException directly rather than converting something else,
	/// so it legitimately has no inner exception. Pinned so a later refactor does not quietly start wrapping it.
	/// </summary>
	[Fact]
	public void IsFunction_WithTooManyParameters_ThrowsWithoutAnInnerException()
		=> new ExtendedExpression("isNullOrWhiteSpace('a', 'b')")
			.Invoking(x => x.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("isNullOrWhiteSpace() requires one parameter.")
			.And.InnerException.Should().BeNull();
}
