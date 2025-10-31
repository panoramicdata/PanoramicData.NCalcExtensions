namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("startsWith")]
	[Description("Determines whether a string starts with another string.")]
	bool StartsWith(
		[Description("The string to be searched.")]
		string longString,
		[Description("The string to serarch for.")]
		string shortString
	);
}

internal static class StartsWith
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 2)
		{
			throw new FormatException($"{ExtensionFunction.StartsWith}() requires two parameters.");
		}

		try
		{
			var param1 = functionArgs.Parameters[0].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.StartsWith}() parameter 1 is not a string");
			var param2 = functionArgs.Parameters[1].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.StartsWith}() parameter 2 is not a string");

			functionArgs.Result = param1.StartsWith(param2, StringComparison.InvariantCulture);
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"Unexpected exception in {ExtensionFunction.StartsWith}(): {e.Message}", e);
		}
	}
}
