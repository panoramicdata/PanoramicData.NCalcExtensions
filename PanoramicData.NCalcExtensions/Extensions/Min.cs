namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("min")]
	[Description("Emits the minimum value, ignoring nulls.")]
	object? Min(
		[Description("The list of values")]
		IEnumerable<object?> list,
		[Description("(Optional) a string to represent the value to be evaluated")]
		string? predicate = null,
		[Description("(Optional, but must be provided if predicate is) the string to evaluate")]
		string? exprStr = null
	);
}

internal static class Min
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
		=> Extremum.Evaluate(functionArgs, ExtensionFunction.Min, isMax: false);
}
