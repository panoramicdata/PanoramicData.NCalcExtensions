using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("skip")]
	[Description("If the number of items to skip is greater than the number of items in the list, an empty list is returned.")]
	List<object?> Skip(
		[Description("The list to skip from.")]
		IList list,
		[Description("The number of items to skip.")]
		int count
	);
}

internal static class Skip
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var list = (IList)functionArgs.Parameters[0].Evaluate();
		var numberToSkip = (int)functionArgs.Parameters[1].Evaluate();
		functionArgs.Result = list.Cast<object?>().Skip(numberToSkip).ToList();
	}
}
