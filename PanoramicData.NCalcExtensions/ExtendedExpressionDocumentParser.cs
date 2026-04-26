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
		var documentation = ExtractDocumentation(expression);
		var withoutMultilineComments = ExpressionCommentParsing.RemoveMultilineComments(expression);
		var parameters = ExtractParameterDefinitions(withoutMultilineComments);
		var comments = ExtractComments(withoutMultilineComments);
		var tidiedExpression = TidyExpression(withoutMultilineComments);

		return new ExtendedExpressionDocument
		{
			OriginalExpression = expression,
			TidiedExpression = tidiedExpression,
			Documentation = documentation,
			Parameters = parameters,
			Comments = comments
		};
	}

	/// <summary>
	/// Extract documentation from /* */ multi-line comments.
	/// Returns the first multi-line comment block found, or null if none exists.
	/// </summary>
	private static string? ExtractDocumentation(string expression)
	{
		var i = 0;
		while (i < expression.Length - 1)
		{
			if (expression[i] == '/' && expression[i + 1] == '*')
			{
				i += 2;
				var docStart = i;
				
				while (i < expression.Length - 1)
				{
					if (expression[i] == '*' && expression[i + 1] == '/')
					{
						var documentation = expression.Substring(docStart, i - docStart).Trim();
						return string.IsNullOrWhiteSpace(documentation) ? null : documentation;
					}
					i++;
				}
				
				// If we reach here, comment was not closed properly
				if (i < expression.Length && expression[i] == '*')
				{
					i++;
				}
			}
			else
			{
				i++;
			}
		}

		return null;
	}

	/// <summary>
	/// Extract parameter definitions from comment lines in the format:
	/// // parameterName:TypeName:value
	/// </summary>
	private static Dictionary<string, (string TypeName, string Value)> ExtractParameterDefinitions(string expression)
	{
		return ExpressionCommentParsing.ExtractParameterDefinitions(expression);
	}

	/// <summary>
	/// Extract regular comment lines (// comments that are not parameter definitions).
	/// </summary>
	private static List<string> ExtractComments(string expression)
	{
		var comments = new List<string>();

		foreach (var line in expression.Split('\n'))
		{
			if (ExpressionCommentParsing.TryGetCommentContent(line, out var commentContent)
				&& !ExpressionCommentParsing.IsParameterDefinitionComment(commentContent))
			{
				comments.Add(commentContent);
			}
		}

		return comments;
	}

	/// <summary>
	/// Tidy the expression by removing all comments and parameter definitions.
	/// </summary>
	private static string TidyExpression(string expression)
	{
		return string.Join(
			" ",
			expression
				.Split('\n')
				.Select(line => line.TrimEnd('\r'))
				.Where(line =>
				{
					var trimmedLine = line.TrimStart();
					// Keep lines that don't start with //
					if (!trimmedLine.StartsWith("//", StringComparison.Ordinal))
					{
						return true;
					}

					// Remove all comment lines
					return false;
				})
		).Trim();  // Trim the final result to remove leading/trailing spaces
	}
}
