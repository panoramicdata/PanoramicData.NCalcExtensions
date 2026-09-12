namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("IsNullOrWhiteSpace")]
	[Description("Determines whether a value is null, empty or whitespace.")]
	bool IsNullOrWhiteSpace(
		[Description("The value to be tested.")]
		object? value
	);
}

internal static class IsNullOrWhiteSpace
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
		=> NullOrBlank.Evaluate(functionArgs, ExtensionFunction.IsNullOrWhiteSpace, whiteSpaceIsBlank: true);
}
