namespace PanoramicData.NCalcExtensions;

/// <summary>
/// Extended NCalc expression that supports:
/// - Multi-line expressions
/// - Comments (// and /* */)
/// - Typed parameter and answer definitions in comments (for example // x:int:10, // y:string?, // answer:string?:null)
/// - Automatic parameter extraction and type conversion
/// - All extension functions
/// 
/// Use this when you need to parse comments and extract parameters.
/// For better performance with repeated evaluations, use ExtendedExpressionDocumentParser + SimpleExtendedExpression.
/// </summary>
public class ExtendedExpression : BaseExtendedExpression
{
	private readonly ExpressionOptions _expressionOptions;
	private readonly TimeProvider? _timeProvider;
	private readonly Lazy<SimpleExtendedExpression> _simpleExtendedExpression;
	private readonly Lazy<object?> _expectedAnswer;

	/// <summary>
	/// Parsed document containing original/tidied expression, documentation, parameters, and comments.
	/// </summary>
	public ExtendedExpressionDocument Document { get; }

	/// <summary>
	/// Documentation extracted from the first /* */ block, if present.
	/// </summary>
	public string? Documentation => Document.Documentation;

	/// <summary>
	/// Parsed parameter definitions from typed // name:type or // name:type:value comment lines.
	/// </summary>
	public IReadOnlyDictionary<string, TypedDefinition> ParameterDefinitions => Document.Parameters;

	/// <summary>
	/// Parsed answer definition from a typed // answer:type or // answer:type:value comment line.
	/// </summary>
	public TypedDefinition? AnswerDefinition => Document.Answer;

	/// <summary>
	/// Whether an answer definition is present.
	/// </summary>
	public bool HasExpectedAnswer => Document.HasAnswer;

	/// <summary>
	/// Whether the answer definition includes a value.
	/// </summary>
	public bool HasExpectedAnswerValue => Document.HasAnswerValue;

	/// <summary>
	/// Parsed and typed expected answer, if an answer definition is present.
	/// </summary>
	public object? ExpectedAnswer => _expectedAnswer.Value;

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
		CultureInfo cultureInfo,
		TimeProvider? timeProvider = null) : this(
		ExtendedExpressionDocumentParser.Parse(expression),
		expressionOptions,
		cultureInfo,
		timeProvider)
	{
	}

	private ExtendedExpression(
		ExtendedExpressionDocument document,
		ExpressionOptions expressionOptions,
		CultureInfo cultureInfo,
		TimeProvider? timeProvider = null) : base(document.TidiedExpression, expressionOptions, cultureInfo, timeProvider)
	{
		Document = document;
		_expressionOptions = expressionOptions;
		_timeProvider = timeProvider;
		SetParametersFromDefinitions(document.Parameters);
		_expectedAnswer = new Lazy<object?>(
			() => Document.Answer is { HasValue: true } answer
				? ConvertDefinitionValue("Answer", answer)
				: null);
		_simpleExtendedExpression = new Lazy<SimpleExtendedExpression>(
			() => new SimpleExtendedExpression(Document.TidiedExpression, Document, _expressionOptions, CultureInfo, _timeProvider)
		);
	}

}
