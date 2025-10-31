namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("endsWith")]
	[Description("Determines whether a string ends with another string.")]
	bool EndsWith(
		[Description("The main text being examined.")]
		string longString,
		[Description("The shorter text to look for at the end of the main text.")]
		string shortString
	);
}

internal static class EndsWith
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var param1 = functionArgs.Parameters[0].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.EndsWith} function requires two string parameters.");
			var param2 = functionArgs.Parameters[1].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.EndsWith} function requires two string parameters.");

			functionArgs.Result = param1.EndsWith(param2, StringComparison.InvariantCulture);
		}
		catch (Exception e) when (e is not NCalcExtensionsException)
		{
			throw new FormatException($"{ExtensionFunction.EndsWith} function requires two string parameters.");
		}
	}
}
