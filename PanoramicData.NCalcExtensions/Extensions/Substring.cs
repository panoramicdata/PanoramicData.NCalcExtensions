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

	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var input = functionArgs.Parameters[0].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.Substring}() requires a string parameter and one or two numeric parameters.");

			if (functionArgs.Parameters[1].Evaluate() is not int startIndex)
			{
				throw new FormatException($"{ExtensionFunction.Substring}() requires a string parameter and one or two numeric parameters.");
			}

			int? length = null;
			if (functionArgs.Parameters.Length > 2)
			{
				var thirdArg = functionArgs.Parameters[2].Evaluate();
				if (thirdArg is not int lengthValue)
				{
					throw new FormatException($"{ExtensionFunction.Substring}() requires a string parameter and one or two numeric parameters.");
				}

				length = lengthValue;
			}

			var mode = functionArgs.Parameters.Length > 3
				? functionArgs.Parameters[3].Evaluate() as string
				: null;

			var modeNormalised = mode?.Trim().ToLowerInvariant() ?? ModeError;

			// Validate negative length regardless of mode
			if (length < 0)
			{
				throw new FormatException($"{ExtensionFunction.Substring}() requires a string parameter and one or two numeric parameters.");
			}

			// Check bounds
			var outOfBounds = startIndex < 0 || startIndex > input.Length;
			if (outOfBounds)
			{
				switch (modeNormalised)
				{
					case ModeEmpty:
						functionArgs.Result = string.Empty;
						return;
					case ModeNull:
						functionArgs.Result = null;
						return;
					case ModeClip:
						startIndex = Math.Clamp(startIndex, 0, input.Length);
						break;
					case ModeError:
					default:
						throw new FormatException(
							$"{ExtensionFunction.Substring}() start index {startIndex} is out of bounds for a string of length {input.Length}. " +
							$"Use mode 'Clip', 'Empty' or 'Null' to handle out-of-bounds starts without error.");
				}
			}

			functionArgs.Result = length.HasValue
				? input.Substring(startIndex, Math.Min(length.Value, input.Length - startIndex))
				: input[startIndex..];
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"{ExtensionFunction.Substring}() requires a string parameter and one or two numeric parameters.");
		}
	}
}

