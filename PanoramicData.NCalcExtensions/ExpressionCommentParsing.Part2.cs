namespace PanoramicData.NCalcExtensions;

internal static partial class ExpressionCommentParsing
{
	internal static string RemoveMultilineComments(string expression)
		=> RemoveMultilineComments(expression, out _);

	internal static string RemoveMultilineComments(string expression, out string? documentation)
	{
		var result = new StringBuilder(expression.Length);
		documentation = null;
		var i = 0;

		while (i < expression.Length)
		{
			if (i < expression.Length - 1 && expression[i] == '/' && expression[i + 1] == '*')
			{
				// However long it is, a comment collapses to a single space.
				i = SkipComment(expression, i, ref documentation);
				result.Append(' ');
			}
			else
			{
				result.Append(expression[i]);
				i++;
			}
		}

		return result.ToString();
	}

	/// <summary>
	/// Skips the comment opening at <paramref name="start"/>, returning the index just past its
	/// terminator, or the end of the expression if it has none.
	/// </summary>
	/// <param name="documentation">
	/// Set to the comment's text if it is not already set and this comment is not blank, so that
	/// the first non-blank comment becomes the expression's documentation.
	/// </param>
	private static int SkipComment(string expression, int start, ref string? documentation)
	{
		var documentationStart = start + 2;
		var i = documentationStart;

		while (i < expression.Length - 1)
		{
			if (expression[i] == '*' && expression[i + 1] == '/')
			{
				documentation ??= ReadDocumentation(expression, documentationStart, i);
				i += 2;
				break;
			}

			i++;
		}

		// The loop above stops one character short of the end, so a comment that was never
		// terminated consumes the remainder of the expression.
		return i >= expression.Length - 1 && (i >= expression.Length || expression[i - 1] != '/')
			? expression.Length
			: i;
	}

	/// <summary>
	/// The trimmed comment text between the given indices, or null if it is blank.
	/// </summary>
	private static string? ReadDocumentation(string expression, int start, int end)
	{
		var documentationSpan = expression.AsSpan(start, end - start).Trim();
		return documentationSpan.IsEmpty ? null : documentationSpan.ToString();
	}
}
