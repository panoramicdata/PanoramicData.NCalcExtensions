namespace PanoramicData.NCalcExtensions;

internal static class ExpressionCommentParsing
{
	internal static Dictionary<string, (string TypeName, string Value)> ExtractParameterDefinitions(string expression)
	{
		var parameters = new Dictionary<string, (string, string)>();

		foreach (var line in RemoveMultilineComments(expression).Split('\n'))
		{
			if (!TryGetCommentContent(line, out var commentContent))
			{
				continue;
			}

			if (TryParseParameterDefinition(commentContent, out var parameterName, out var typeName, out var value))
			{
				parameters[parameterName] = (typeName, value);
			}
		}

		return parameters;
	}

	internal static bool TryGetCommentContent(string line, out string commentContent)
	{
		var trimmedLine = line.TrimEnd('\r').TrimStart();
		if (!trimmedLine.StartsWith("//", StringComparison.Ordinal))
		{
			commentContent = string.Empty;
			return false;
		}

		commentContent = trimmedLine.Substring(2).TrimStart();
		return true;
	}

	internal static bool IsParameterDefinitionComment(string commentContent)
	{
		return TryParseParameterDefinition(commentContent, out _, out _, out _);
	}

	internal static string RemoveMultilineComments(string expression)
	{
		var result = new StringBuilder();
		var i = 0;

		while (i < expression.Length)
		{
			if (i < expression.Length - 1 && expression[i] == '/' && expression[i + 1] == '*')
			{
				i += 2;
				while (i < expression.Length - 1)
				{
					if (expression[i] == '*' && expression[i + 1] == '/')
					{
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

	private static bool TryParseParameterDefinition(
		string commentContent,
		out string parameterName,
		out string typeName,
		out string value)
	{
		parameterName = string.Empty;
		typeName = string.Empty;
		value = string.Empty;

		var parts = commentContent.Split(':', 3);
		if (parts.Length != 3)
		{
			return false;
		}

		parameterName = parts[0].Trim();
		typeName = parts[1].Trim();
		value = parts[2].Trim();

		return !string.IsNullOrWhiteSpace(parameterName)
			&& !string.IsNullOrWhiteSpace(typeName)
			&& typeName.Contains('.', StringComparison.Ordinal)
			&& !string.IsNullOrWhiteSpace(value);
	}
}