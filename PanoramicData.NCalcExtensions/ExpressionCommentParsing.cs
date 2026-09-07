namespace PanoramicData.NCalcExtensions;

internal static class ExpressionCommentParsing
{
	internal const string AnswerDefinitionName = "answer";

	internal static Dictionary<string, TypedDefinition> ExtractParameterDefinitions(string expression)
	{
		var parameters = new Dictionary<string, TypedDefinition>();

		foreach (var line in RemoveMultilineComments(expression).Split('\n'))
		{
			if (!TryGetCommentContent(line.AsSpan(), out ReadOnlySpan<char> commentContent))
			{
				continue;
			}

			if (TryParseParameterDefinition(commentContent, out var parameterName, out var definition))
			{
				parameters[parameterName] = definition;
			}
		}

		return parameters;
	}

	internal static TypedDefinition? ExtractAnswerDefinition(string expression)
	{
		foreach (var line in RemoveMultilineComments(expression).Split('\n'))
		{
			if (!TryGetCommentContent(line.AsSpan(), out ReadOnlySpan<char> commentContent))
			{
				continue;
			}

			if (TryParseAnswerDefinition(commentContent, out var definition))
			{
				return definition;
			}
		}

		return null;
	}

	internal static bool TryGetCommentContent(string line, out string commentContent)
		=> TryGetCommentContent(line.AsSpan(), out commentContent);

	internal static bool TryGetCommentContent(ReadOnlySpan<char> line, out string commentContent)
	{
		if (!TryGetCommentContent(line, out ReadOnlySpan<char> commentContentSpan))
		{
			commentContent = string.Empty;
			return false;
		}

		commentContent = commentContentSpan.ToString();
		return true;
	}

	internal static bool TryGetCommentContent(ReadOnlySpan<char> line, out ReadOnlySpan<char> commentContent)
	{
		var trimmedLine = line.TrimEnd('\r').TrimStart();
		if (!trimmedLine.StartsWith("//", StringComparison.Ordinal))
		{
			commentContent = ReadOnlySpan<char>.Empty;
			return false;
		}

		commentContent = trimmedLine[2..].TrimStart();
		return true;
	}

	internal static bool IsParameterDefinitionComment(string commentContent)
	{
		return TryParseParameterDefinition(commentContent, out _, out _);
	}

	internal static bool IsAnswerDefinitionComment(string commentContent)
	{
		return TryParseAnswerDefinition(commentContent, out _);
	}

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

	internal static bool TryParseParameterDefinition(
		string commentContent,
		out string parameterName,
		out TypedDefinition definition)
		=> TryParseParameterDefinition(commentContent.AsSpan(), out parameterName, out definition);

	internal static bool TryParseParameterDefinition(
		ReadOnlySpan<char> commentContent,
		out string parameterName,
		out TypedDefinition definition)
	{
		if (!TryParseTypedDefinition(commentContent, out parameterName, out definition))
		{
			return false;
		}

		return !parameterName.Equals(AnswerDefinitionName, StringComparison.OrdinalIgnoreCase);
	}

	internal static bool TryParseAnswerDefinition(
		string commentContent,
		out TypedDefinition definition)
		=> TryParseAnswerDefinition(commentContent.AsSpan(), out definition);

	internal static bool TryParseAnswerDefinition(
		ReadOnlySpan<char> commentContent,
		out TypedDefinition definition)
	{
		if (!TryParseTypedDefinition(commentContent, out var name, out definition))
		{
			return false;
		}

		return name.Equals(AnswerDefinitionName, StringComparison.OrdinalIgnoreCase);
	}

	private static bool TryParseTypedDefinition(
		ReadOnlySpan<char> commentContent,
		out string name,
		out TypedDefinition definition)
	{
		name = string.Empty;
		definition = null!;

		var nameSeparatorIndex = commentContent.IndexOf(':');
		if (nameSeparatorIndex <= 0)
		{
			return false;
		}

		var nameSpan = commentContent[..nameSeparatorIndex].Trim();
		var definitionContent = commentContent[(nameSeparatorIndex + 1)..].Trim();
		if (nameSpan.IsEmpty || definitionContent.IsEmpty)
		{
			return false;
		}

		name = nameSpan.ToString();

		SplitTypeAndValue(definitionContent, out var typeName, out var value);

		if (!TypedDefinitionTypeResolver.TryResolve(typeName, out _))
		{
			return false;
		}

		if (value is null)
		{
			definition = TypedDefinition.FromTypeOnly(typeName);
			return true;
		}

		return TryDefineFromValue(name, typeName, value, out definition);
	}

	/// <summary>
	/// Splits a definition's content, which is either "type" or "type: value".
	/// </summary>
	/// <param name="value">
	/// Null when no value was given at all, which is distinct from a value that is present but
	/// blank — that is rejected rather than treated as type-only.
	/// </param>
	private static void SplitTypeAndValue(ReadOnlySpan<char> definitionContent, out string typeName, out string? value)
	{
		var valueSeparatorIndex = definitionContent.IndexOf(':');
		if (valueSeparatorIndex < 0)
		{
			typeName = definitionContent.ToString();
			value = null;
			return;
		}

		typeName = definitionContent[..valueSeparatorIndex].Trim().ToString();
		value = definitionContent[(valueSeparatorIndex + 1)..].Trim().ToString();
	}

	/// <summary>
	/// Builds the definition for a "type: value" definition.
	/// </summary>
	private static bool TryDefineFromValue(string name, string typeName, string value, out TypedDefinition definition)
	{
		definition = null!;

		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}

		definition = value.Equals("null", StringComparison.OrdinalIgnoreCase)
			? TypedDefinition.FromNull(typeName)
			: TypedDefinition.FromLiteral(typeName, value);

		return !string.IsNullOrWhiteSpace(name)
			&& !string.IsNullOrWhiteSpace(typeName);
	}
}