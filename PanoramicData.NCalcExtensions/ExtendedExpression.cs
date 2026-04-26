namespace PanoramicData.NCalcExtensions;

/// <summary>
/// Extended NCalc expression that supports:
/// - Multi-line expressions
/// - Comments (// and /* */)
/// - Parameter definitions in comments (// parameterName:TypeName:value)
/// - Automatic parameter extraction and type conversion
/// - All extension functions
/// 
/// Use this when you need to parse comments and extract parameters.
/// For better performance with repeated evaluations, use ExtendedExpressionDocumentParser + SimpleExtendedExpression.
/// </summary>
public class ExtendedExpression : BaseExtendedExpression
{
	private readonly ExpressionOptions _expressionOptions;
	private readonly Lazy<SimpleExtendedExpression> _simpleExtendedExpression;

	/// <summary>
	/// Parsed document containing original/tidied expression, documentation, parameters, and comments.
	/// </summary>
	public ExtendedExpressionDocument Document { get; }

	/// <summary>
	/// Documentation extracted from the first /* */ block, if present.
	/// </summary>
	public string? Documentation => Document.Documentation;

	/// <summary>
	/// Parsed parameter definitions in // name:Type:value format.
	/// </summary>
	public IReadOnlyDictionary<string, (string TypeName, string Value)> ParameterDefinitions => Document.Parameters;

	/// <summary>
	/// Regular // comments that were not parameter definitions.
	/// </summary>
	public IReadOnlyList<string> Comments => Document.Comments;

	/// <summary>
	/// Cached SimpleExtendedExpression using the same options/culture and parsed document.
	/// </summary>
	public SimpleExtendedExpression SimpleExtendedExpression => _simpleExtendedExpression.Value;

	public ExtendedExpression(string expression) : this(
		expression,
		ExtendedExpressionDefaults,
		CultureInfo.InvariantCulture)
	{
	}

	public ExtendedExpression(
		string expression,
		ExpressionOptions expressionOptions,
		CultureInfo cultureInfo) : this(
		ExtendedExpressionDocumentParser.Parse(expression),
		expressionOptions,
		cultureInfo)
	{
	}

	private ExtendedExpression(
		ExtendedExpressionDocument document,
		ExpressionOptions expressionOptions,
		CultureInfo cultureInfo) : base(document.TidiedExpression, expressionOptions, cultureInfo)
	{
		Document = document;
		_expressionOptions = expressionOptions;
		SetParametersFromDefinitions(document.Parameters);
		_simpleExtendedExpression = new Lazy<SimpleExtendedExpression>(
			() => new SimpleExtendedExpression(Document.TidiedExpression, Document, _expressionOptions, CultureInfo)
		);
	}

}
