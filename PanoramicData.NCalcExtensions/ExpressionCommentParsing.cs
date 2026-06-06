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
				var documentationStart = i + 2;
				i += 2;
				while (i < expression.Length - 1)
				{
					if (expression[i] == '*' && expression[i + 1] == '/')
					{
						if (documentation is null)
						{
							var documentationSpan = expression.AsSpan(documentationStart, i - documentationStart).Trim();
							documentation = documentationSpan.IsEmpty ? null : documentationSpan.ToString();
						}
						i += 2;
						break;
					}
					i++;
				}
				if (i >= expression.Length - 1 && (i >= expression.Length || expression[i - 1] != '/'))
				{
					i = expression.Length;
				}
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

		string typeName;
		string? value = null;
		var hasValue = false;

		var valueSeparatorIndex = definitionContent.IndexOf(':');
		if (valueSeparatorIndex >= 0)
		{
			var typeNameSpan = definitionContent[..valueSeparatorIndex].Trim();
			var valueSpan = definitionContent[(valueSeparatorIndex + 1)..].Trim();
			typeName = typeNameSpan.ToString();
			value = valueSpan.ToString();
			hasValue = true;
		}
		else
		{
			typeName = definitionContent.ToString();
		}

		if (!TypedDefinitionTypeResolver.TryResolve(typeName, out _))
		{
			return false;
		}

		if (!hasValue)
		{
			definition = TypedDefinition.FromTypeOnly(typeName);
			return true;
		}

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