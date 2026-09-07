using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("last")]
	[Description("Returns the last item in a list that matches a lambda or throws a FormatException if no items match. Note that items are processed in reverse order.")]
	object? Last(
		[Description("The list of items.")]
		IList list,
		[Description("A string to represent the value to be evaluated")]
		string? predicate = null,
		[Description("The string to evaluate")]
		string? exprStr = null
	);
}

internal static class Last
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
		=> ElementSelection.Evaluate(
			functionArgs,
			ExtensionFunction.Last,
			fromEnd: true,
			allowMissing: false);
}
