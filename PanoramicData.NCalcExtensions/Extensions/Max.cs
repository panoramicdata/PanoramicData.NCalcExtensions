namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("max")]
	[Description("Emits the maximum value, ignoring nulls.")]
	object? Max(
		[Description("The list of values")]
		IEnumerable<object?> list,
		[Description("(Optional) a string to represent the value to be evaluated")]
		[Optional] string? predicate,
		[Description("(Optional, but must be provided if predicate is) the string to evaluate")]
		[Optional] string? exprStr
	);
}

internal static class Max
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
		=> Extremum.Evaluate(functionArgs, ExtensionFunction.Max, isMax: true);
}
