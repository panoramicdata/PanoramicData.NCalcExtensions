namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("repeat")]
	[Description("Repeats a string a specified number of times.")]
	string Repeat(
		[Description("The string to repeat.")]
		string text,
		[Description("The number of times to repeat (must be >= 0).")]
		int count
	);
}

internal static class RepeatFunction
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		try
		{
			var text = functionArgs.Parameters.Evaluate(0) as string
				?? throw new FormatException($"{ExtensionFunction.Repeat}() requires a string parameter 1 and an integer parameter 2.");

			if (functionArgs.Parameters.Evaluate(1) is not int count)
			{
				throw new FormatException($"{ExtensionFunction.Repeat}() requires a string parameter 1 and an integer parameter 2.");
			}

			if (count < 0)
			{
				throw new NCalcExtensionsException($"{ExtensionFunction.Repeat}() requires a count >= 0.");
			}

			functionArgs.Result = string.Concat(Enumerable.Repeat(text, count));
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"{ExtensionFunction.Repeat}() requires a string parameter 1 and an integer parameter 2.");
		}
	}
}
