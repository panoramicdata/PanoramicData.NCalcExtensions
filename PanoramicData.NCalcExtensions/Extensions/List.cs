namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("list")]
	[Description("Emits a List<object?> and collapses down lists of lists to a single list.")]
	List<object> List(
		[Description("The list parameters.")]
		IEnumerable<object?> parameters
	);
}

internal static class List
{
	internal static void Evaluate(IFunctionArgs functionArgs)
		=> functionArgs.Result = functionArgs.Parameters.Select(p => p.Evaluate()).ToList();
}
