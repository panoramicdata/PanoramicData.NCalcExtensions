namespace PanoramicData.NCalcExtensions;

/// <summary>
/// Represents a parsed NCalc expression document containing:
/// - Documentation (extracted from /* */ multi-line comments)
/// - Parameters (extracted from typed // name:type or // name:type:value lines)
/// - Answer definition (extracted from typed // answer:type or // answer:type:value lines)
/// - The tidied expression (ready for evaluation)
/// </summary>
public class ExtendedExpressionDocument
{
	/// <summary>
	/// The original multi-line expression before tidying.
	/// </summary>
	public required string OriginalExpression { get; init; }

	/// <summary>
	/// The tidied expression (comments and parameter definitions removed, ready for evaluation).
	/// </summary>
	public required string TidiedExpression { get; init; }

	/// <summary>
	/// Documentation extracted from /* */ multi-line comments (may contain markdown).
	/// Null or empty if no documentation block was found.
	/// </summary>
	public string? Documentation { get; init; }

	/// <summary>
	/// Parameter definitions extracted from typed comment lines.
	/// </summary>
	public required Dictionary<string, TypedDefinition> Parameters { get; init; }

	/// <summary>
	/// Answer definition extracted from a typed answer comment line.
	/// Null if no answer definition was found.
	/// </summary>
	public TypedDefinition? Answer { get; init; }

	/// <summary>
	/// Whether an answer definition was present in the source document.
	/// </summary>
	public bool HasAnswer => Answer is not null;

	/// <summary>
	/// Whether the answer definition includes a value.
	/// </summary>
	public bool HasAnswerValue => Answer?.HasValue == true;

	/// <summary>
	/// Regular comment lines (// comments that are not parameter definitions or documentation).
	/// </summary>
	public required List<string> Comments { get; init; }
}
