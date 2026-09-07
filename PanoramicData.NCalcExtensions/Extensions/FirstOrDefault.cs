namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("firstOrDefault")]
	[Description("Returns the first item in a list that matches a lambda or null if no items match.")]
	object? FirstOrDefault(
		[Description("The list")]
		IEnumerable<object?> list,
		[Description("A string to represent the value to be evaluated")]
		string predicate,
		[Description("The lambda expression as a string")]
		string exprStr
	);
}

internal static class FirstOrDefault
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
		=> ElementSelection.Evaluate(
			functionArgs,
			ExtensionFunction.FirstOrDefault,
			fromEnd: false,
			allowMissing: true);
}
