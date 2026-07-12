namespace PanoramicData.NCalcExtensions;

/// <summary>
/// A lightweight version of ExtendedExpression optimized for performance.
/// Use this when you already have a tidied expression and don't need to parse comments.
/// 
/// This is typically used after parsing an ExtendedExpressionDocument to avoid
/// re-parsing the same expression multiple times.
/// </summary>
public class SimpleExtendedExpression : BaseExtendedExpression
{
	/// <summary>
	/// Parsed answer definition in // answer:Type:value format.
	/// </summary>
	public TypedDefinition? AnswerDefinition { get; }

	/// <summary>
	/// Whether an answer definition is present.
	/// </summary>
	public bool HasExpectedAnswer => AnswerDefinition is not null;

	/// <summary>
	/// Whether the answer definition includes a value.
	/// </summary>
	public bool HasExpectedAnswerValue => AnswerDefinition?.HasValue == true;

	/// <summary>
	/// Parsed and typed expected answer, if an answer definition is present.
	/// </summary>
	public object? ExpectedAnswer { get; }

	/// <summary>
	/// Create a SimpleExtendedExpression from a pre-tidied expression.
	/// </summary>
	/// <param name="tidiedExpression">The expression with all comments and parameter definitions already removed.</param>
	public SimpleExtendedExpression(string tidiedExpression) : this(
		tidiedExpression,
		ExtendedExpressionDefaults,
		CultureInfo.InvariantCulture)
	{
	}

	/// <summary>
	/// Create a SimpleExtendedExpression from a pre-tidied expression with custom options.
	/// </summary>
	/// <param name="tidiedExpression">The expression with all comments and parameter definitions already removed.</param>
	/// <param name="expressionOptions">The expression options to use.</param>
	/// <param name="cultureInfo">The culture info for parsing values.</param>
	/// <param name="timeProvider">Optional time source for time-dependent functions; defaults to the system clock.</param>
	public SimpleExtendedExpression(
		string tidiedExpression,
		ExpressionOptions expressionOptions,
		CultureInfo cultureInfo,
		TimeProvider? timeProvider = null) : base(ValidateTidiedExpression(tidiedExpression), expressionOptions, cultureInfo, timeProvider)
	{
	}

	/// <summary>
	/// Create a SimpleExtendedExpression from a document and tidied expression.
	/// This version automatically sets parameters from the document.
	/// </summary>
	/// <param name="tidiedExpression">The expression with all comments and parameter definitions already removed.</param>
	/// <param name="document">The parsed document containing parameter definitions.</param>
	public SimpleExtendedExpression(string tidiedExpression, ExtendedExpressionDocument document) : this(
		tidiedExpression,
		ExtendedExpressionDefaults,
		CultureInfo.InvariantCulture)
	{
		SetParametersFromDefinitions(document.Parameters);
		AnswerDefinition = document.Answer;
		ExpectedAnswer = document.Answer is { HasValue: true } answer
			? ConvertDefinitionValue("Answer", answer)
			: null;
	}

	/// <summary>
	/// Create a SimpleExtendedExpression from a document and tidied expression with custom options.
	/// This version automatically sets parameters from the document.
	/// </summary>
	/// <param name="tidiedExpression">The expression with all comments and parameter definitions already removed.</param>
	/// <param name="document">The parsed document containing parameter definitions.</param>
	/// <param name="expressionOptions">The expression options to use.</param>
	/// <param name="cultureInfo">The culture info for parsing values.</param>
	/// <param name="timeProvider">Optional time source for time-dependent functions; defaults to the system clock.</param>
	public SimpleExtendedExpression(
		string tidiedExpression,
		ExtendedExpressionDocument document,
		ExpressionOptions expressionOptions,
		CultureInfo cultureInfo,
		TimeProvider? timeProvider = null) : base(ValidateTidiedExpression(tidiedExpression), expressionOptions, cultureInfo, timeProvider)
	{
		SetParametersFromDefinitions(document.Parameters);
		AnswerDefinition = document.Answer;
		ExpectedAnswer = document.Answer is { HasValue: true } answer
			? ConvertDefinitionValue("Answer", answer)
			: null;
	}

	private static string ValidateTidiedExpression(string tidiedExpression)
	{
		ArgumentNullException.ThrowIfNull(tidiedExpression);

		if (tidiedExpression.Contains('\n') || tidiedExpression.Contains('\r'))
		{
			throw new FormatException("SimpleExtendedExpression requires a single-line, pre-tidied expression. Newline characters are not permitted.");
		}

		var trimmedStart = tidiedExpression.AsSpan().TrimStart();
		if (trimmedStart.StartsWith("//", StringComparison.Ordinal)
			|| trimmedStart.StartsWith("/*", StringComparison.Ordinal))
		{
			throw new FormatException("SimpleExtendedExpression requires a pre-tidied expression and cannot start with comment markers (// or /*). Use ExtendedExpressionDocumentParser or ExtendedExpression first.");
		}

		return tidiedExpression;
	}
}
