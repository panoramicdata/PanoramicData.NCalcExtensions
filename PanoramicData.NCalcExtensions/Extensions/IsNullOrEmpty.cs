namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("isNullOrEmpty")]
	[Description("Determines whether a value is null or empty.")]
	bool IsNullOrEmpty(
		[Description("The value to be tested.")]
		object? value
	);
}

internal static class IsNullOrEmpty
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
		=> NullOrBlank.Evaluate(functionArgs, ExtensionFunction.IsNullOrEmpty, whiteSpaceIsBlank: false);
}
