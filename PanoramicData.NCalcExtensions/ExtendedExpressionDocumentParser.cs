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
		var parameters = new Dictionary<string, TypedDefinition>();
		var comments = new List<string>();
		var tidiedExpression = new StringBuilder(expression.Length);
		TypedDefinition? answer = null;
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
				if (ExpressionCommentParsing.TryParseParameterDefinition(commentContent, out var parameterName, out var definition))
				{
					parameters[parameterName] = definition;
				}
				else if (answer is null && ExpressionCommentParsing.TryParseAnswerDefinition(commentContent, out var answerDefinition))
				{
					answer = answerDefinition;
				}
				else
				{
					comments.Add(commentContent.ToString());
				}
			}
			else
			{
				tidiedExpression.Append(lineWithoutCarriageReturn);
			}

			isFirstTidiedLine = false;
			lineStart = i + 1;
		}

		return new LineAnalysis(parameters, answer, comments, tidiedExpression.ToString().Trim());
	}

	private sealed record LineAnalysis(
		Dictionary<string, TypedDefinition> Parameters,
		TypedDefinition? Answer,
		List<string> Comments,
		string TidiedExpression);
}
