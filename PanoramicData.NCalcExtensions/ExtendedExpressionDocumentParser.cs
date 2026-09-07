namespace PanoramicData.NCalcExtensions;

/// <summary>
/// Parser for extracting documentation, parameters, and tidied expressions from multi-line NCalc expressions.
/// </summary>
public static class ExtendedExpressionDocumentParser
{
	/// <summary>
	/// Parse a multi-line NCalc expression into an ExtendedExpressionDocument.
	/// </summary>
	/// <param name="expression">The original multi-line expression.</param>
	/// <returns>A document containing parsed documentation, parameters, and tidied expression.</returns>
	public static ExtendedExpressionDocument Parse(string expression)
	{
		var withoutMultilineComments = ExpressionCommentParsing.RemoveMultilineComments(expression, out var documentation);
		var analysis = AnalyzeLines(withoutMultilineComments);

		return new ExtendedExpressionDocument
		{
			OriginalExpression = expression,
			TidiedExpression = analysis.TidiedExpression,
			Documentation = documentation,
			Parameters = analysis.Parameters,
			Answer = analysis.Answer,
			Comments = analysis.Comments
		};
	}

	private static LineAnalysis AnalyzeLines(string expression)
	{
		var commentLines = new CommentAccumulator();
		var tidiedExpression = new StringBuilder(expression.Length);
		var lineStart = 0;
		var isFirstTidiedLine = true;
		var expressionSpan = expression.AsSpan();

		for (var i = 0; i <= expression.Length; i++)
		{
			if (i < expression.Length && expression[i] != '\n')
			{
				continue;
			}

			var lineLength = i - lineStart;
			var lineWithoutCarriageReturn = expressionSpan.Slice(lineStart, lineLength).TrimEnd('\r');

			if (!isFirstTidiedLine)
			{
				tidiedExpression.Append(' ');
			}

			if (ExpressionCommentParsing.TryGetCommentContent(lineWithoutCarriageReturn, out ReadOnlySpan<char> commentContent))
			{
				commentLines.Add(commentContent);
			}
			else
			{
				tidiedExpression.Append(lineWithoutCarriageReturn);
			}

			isFirstTidiedLine = false;
			lineStart = i + 1;
		}

		return new LineAnalysis(
			commentLines.Parameters,
			commentLines.Answer,
			commentLines.Comments,
			tidiedExpression.ToString().Trim());
	}

	/// <summary>
	/// Collects what an expression's comment lines contribute as it is scanned.
	/// </summary>
	private sealed class CommentAccumulator
	{
		public Dictionary<string, TypedDefinition> Parameters { get; } = [];

		public List<string> Comments { get; } = [];

		public TypedDefinition? Answer { get; private set; }

		/// <summary>
		/// Files one comment as a parameter definition, the answer definition, or a plain comment.
		/// </summary>
		public void Add(ReadOnlySpan<char> commentContent)
		{
			if (ExpressionCommentParsing.TryParseParameterDefinition(commentContent, out var parameterName, out var definition))
			{
				Parameters[parameterName] = definition;
				return;
			}

			// Only the first answer definition is taken; any later one is kept as a comment.
			if (Answer is null && ExpressionCommentParsing.TryParseAnswerDefinition(commentContent, out var answerDefinition))
			{
				Answer = answerDefinition;
				return;
			}

			Comments.Add(commentContent.ToString());
		}
	}

	private sealed record LineAnalysis(
		Dictionary<string, TypedDefinition> Parameters,
		TypedDefinition? Answer,
		List<string> Comments,
		string TidiedExpression);
}
