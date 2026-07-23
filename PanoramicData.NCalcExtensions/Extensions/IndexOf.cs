namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("indexOf")]
	[Description("Determines the first position of a string within another string.")]
	int IndexOf(
		[Description("The main text to be searched.")]
		string longString,
		[Description("The shorter text to be looked for within the given main text.")]
		string shortString
	);
}

internal static class IndexOf
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		try
		{
			var param1 = functionArgs.Parameters.Evaluate(0) as string
				?? throw new FormatException($"{ExtensionFunction.IndexOf}() requires two string parameters.");
			var param2 = functionArgs.Parameters.Evaluate(1) as string
				?? throw new FormatException($"{ExtensionFunction.IndexOf}() requires two string parameters.");

			functionArgs.Result = param1.IndexOf(param2, StringComparison.InvariantCulture);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"{ExtensionFunction.IndexOf}() requires two string parameters.");
		}
	}
}
