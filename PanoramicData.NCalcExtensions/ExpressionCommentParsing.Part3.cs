namespace PanoramicData.NCalcExtensions;

internal static partial class ExpressionCommentParsing
{
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

		if (!TypedDefinitionTypeResolver.TryResolve(typeName, out var resolvedType))
		{
			return false;
		}

		if (value is null)
		{
			definition = TypedDefinition.FromTypeOnly(typeName);
			return true;
		}

		if (!value.Equals("null", StringComparison.OrdinalIgnoreCase)
			&& !TypedDefinitionTypeResolver.IsValidValue(resolvedType, value))
		{
			return false;
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
