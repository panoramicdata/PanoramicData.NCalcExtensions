namespace PanoramicData.NCalcExtensions;

/// <summary>
/// Represents a parsed NCalc expression document containing:
/// - Documentation (extracted from /* */ multi-line comments)
/// - Parameters (extracted from // parameterName:TypeName:value lines)
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
	/// Parameter definitions extracted from // parameterName:TypeName:value lines.
	/// Key: parameter name, Value: (type name, value string)
	/// </summary>
	public required Dictionary<string, (string TypeName, string Value)> Parameters { get; init; }

	/// <summary>
	/// Regular comment lines (// comments that are not parameter definitions or documentation).
	/// </summary>
	public required List<string> Comments { get; init; }
}
