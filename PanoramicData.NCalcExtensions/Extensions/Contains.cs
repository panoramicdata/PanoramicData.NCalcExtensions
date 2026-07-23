namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("contains")]
	[Description("Determines whether one string contains another.")]
	bool Concat(
		[Description("The text to be searched.")]
		string text,
		[Description("The text to be searched for.")]
		string searchText
	);
}

internal static class Contains
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		try
		{
			var haystack = functionArgs.Parameters.Evaluate(0) as string
				?? throw new FormatException($"{ExtensionFunction.Contains}() requires two string parameters.");
			var needle = functionArgs.Parameters.Evaluate(1) as string
				?? throw new FormatException($"{ExtensionFunction.Contains}() requires two string parameters.");

			functionArgs.Result = haystack.Contains(needle);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"{ExtensionFunction.Contains}() requires two string parameters.");
		}
	}
}
