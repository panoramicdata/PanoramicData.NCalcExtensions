namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("substring")]
	[Description("Retrieves part of a string. If more characters are requested than available at the end of the string, just the available characters are returned.")]
	string Substring(
		[Description("The original string.")]
		string inputString,
		[Description("The starting index (Zero based).")]
		int startIndex,
		[Description("(Optional) Number of characters to return.")]
		int? length = null,
		[Description("(Optional) Out-of-bounds handling strategy: 'Error' (default), 'Empty', 'Null', or 'Clip'.")]
		string? mode = null
	);
}

internal static class Substring
{
	private const string ModeError = "error";
	private const string ModeEmpty = "empty";
	private const string ModeNull = "null";
	private const string ModeClip = "clip";

	private const string SyntaxMessage =
		ExtensionFunction.Substring + "() requires a string parameter and one or two numeric parameters.";

	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		try
		{
			var input = functionArgs.Parameters.Evaluate(0) as string
				?? throw new FormatException(SyntaxMessage);

			if (functionArgs.Parameters.Evaluate(1) is not int startIndex)
			{
				throw new FormatException(SyntaxMessage);
			}

			var length = ReadLength(functionArgs);
			var mode = ReadMode(functionArgs);

			// A negative length is rejected regardless of mode.
			if (length < 0)
			{
				throw new FormatException(SyntaxMessage);
			}

			functionArgs.Result = Cut(input, startIndex, length, mode);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException(SyntaxMessage);
		}
	}

	/// <summary>
	/// The optional length parameter, or null if it was not supplied.
	/// </summary>
	private static int? ReadLength(FunctionEventArgs functionArgs)
	{
		if (functionArgs.Parameters.Count <= 2)
		{
			return null;
		}

		return functionArgs.Parameters.Evaluate(2) is int lengthValue
			? lengthValue
			: throw new FormatException(SyntaxMessage);
	}

	/// <summary>
	/// The optional out-of-bounds mode, normalised, defaulting to <see cref="ModeError"/>.
	/// </summary>
	private static string ReadMode(FunctionEventArgs functionArgs)
	{
		var mode = functionArgs.Parameters.Count > 3
			? functionArgs.Parameters.Evaluate(3) as string
			: null;

		return mode?.Trim().ToLowerInvariant() ?? ModeError;
	}

	private static string? Cut(string input, int startIndex, int? length, string mode)
		=> startIndex < 0 || startIndex > input.Length
			? OutOfBounds(input, startIndex, length, mode)
			: Slice(input, startIndex, length);

	/// <summary>
	/// Applies the out-of-bounds mode for a start index outside the string.
	/// </summary>
	private static string? OutOfBounds(string input, int startIndex, int? length, string mode) => mode switch
	{
		ModeEmpty => string.Empty,
		ModeNull => null,
		ModeClip => Slice(input, Math.Clamp(startIndex, 0, input.Length), length),
		_ => throw new FormatException(
			$"{ExtensionFunction.Substring}() start index {startIndex} is out of bounds for a string of length {input.Length}. " +
			$"Use mode 'Clip', 'Empty' or 'Null' to handle out-of-bounds starts without error."),
	};

	/// <summary>
	/// The substring from <paramref name="startIndex"/>, clamped to the characters available.
	/// </summary>
	private static string Slice(string input, int startIndex, int? length)
		=> length.HasValue
			? input.Substring(startIndex, Math.Min(length.Value, input.Length - startIndex))
			: input[startIndex..];

}

