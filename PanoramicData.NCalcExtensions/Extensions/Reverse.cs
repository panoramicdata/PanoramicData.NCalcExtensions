using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("reverse")]
	[Description("Reverses an IEnumerable.")]
	List<object?> Reverse(
		[Description("The list of items to be reversed.")]
		IList list
	);
}

internal static class Reverse
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		var enumerable = functionArgs.Parameters.Evaluate(0) as IList
			?? throw new FormatException($"First {ExtensionFunction.Reverse} parameter must be an IEnumerable.");

		functionArgs.Result = enumerable.Cast<object?>()
			.Reverse()
			.Select(value => JValueHelper.UnwrapJValue(value))
			.ToList();
	}
}
