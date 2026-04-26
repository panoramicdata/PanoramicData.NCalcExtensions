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
	public SimpleExtendedExpression(
		string tidiedExpression,
		ExpressionOptions expressionOptions,
		CultureInfo cultureInfo) : base(ValidateTidiedExpression(tidiedExpression), expressionOptions, cultureInfo)
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
	}

	/// <summary>
	/// Create a SimpleExtendedExpression from a document and tidied expression with custom options.
	/// This version automatically sets parameters from the document.
	/// </summary>
	/// <param name="tidiedExpression">The expression with all comments and parameter definitions already removed.</param>
	/// <param name="document">The parsed document containing parameter definitions.</param>
	/// <param name="expressionOptions">The expression options to use.</param>
	/// <param name="cultureInfo">The culture info for parsing values.</param>
	public SimpleExtendedExpression(
		string tidiedExpression,
		ExtendedExpressionDocument document,
		ExpressionOptions expressionOptions,
		CultureInfo cultureInfo) : base(ValidateTidiedExpression(tidiedExpression), expressionOptions, cultureInfo)
	{
		SetParametersFromDefinitions(document.Parameters);
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
