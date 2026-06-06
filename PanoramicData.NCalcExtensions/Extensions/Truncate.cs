namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("truncate")]
	[Description("Truncates a string to a maximum length, optionally appending an ellipsis.")]
	string Truncate(
		[Description("The string to truncate.")]
		string text,
		[Description("The maximum length of the output string (must be >= 0).")]
		int maxLength,
		[Description("Optional string to append when truncated (e.g. '...'). Defaults to empty string.")]
		string ellipsis
	);
}

internal static class TruncateFunction
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var text = functionArgs.Parameters[0].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.Truncate}() requires a string parameter 1 and an integer parameter 2.");

			if (functionArgs.Parameters[1].Evaluate() is not int maxLength)
			{
				throw new FormatException($"{ExtensionFunction.Truncate}() requires a string parameter 1 and an integer parameter 2.");
			}

			if (maxLength < 0)
			{
				throw new NCalcExtensionsException($"{ExtensionFunction.Truncate}() requires a maxLength >= 0.");
			}

			var ellipsis = functionArgs.Parameters.Length > 2
				? functionArgs.Parameters[2].Evaluate() as string
					?? throw new FormatException($"{ExtensionFunction.Truncate}() parameter 3 must be a string.")
				: string.Empty;

			if (text.Length <= maxLength)
			{
				functionArgs.Result = text;
				return;
			}

			// If ellipsis is longer than maxLength, just clip without ellipsis
			var trimmedLength = maxLength - ellipsis.Length;
			functionArgs.Result = trimmedLength <= 0
				? text[..maxLength]
				: text[..trimmedLength] + ellipsis;
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"{ExtensionFunction.Truncate}() requires a string parameter 1 and an integer parameter 2.");
		}
	}
}
