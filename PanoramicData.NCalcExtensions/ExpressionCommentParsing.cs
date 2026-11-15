namespace PanoramicData.NCalcExtensions;

internal static partial class ExpressionCommentParsing
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
}
