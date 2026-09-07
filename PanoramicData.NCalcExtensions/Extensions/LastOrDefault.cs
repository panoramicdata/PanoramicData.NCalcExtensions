using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("lastOrDefault")]
	[Description("Returns the last item in a list that matches a lambda or null if no items match. Note that items are processed in reverse order.")]
	object? LastOrDefault(
		[Description("The list")]
		IList list,
		[Description("A string to represent the value to be evaluated")]
		string predicate,
		[Description("The lambda expression as a string")]
		string exprStr
	);
}

internal static class LastOrDefault
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
		=> ElementSelection.Evaluate(
			functionArgs,
			ExtensionFunction.LastOrDefault,
			fromEnd: true,
			allowMissing: true);
}
