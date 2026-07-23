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
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		var list = functionArgs.Parameters.Evaluate(0) as IList
			?? throw new FormatException($"{ExtensionFunction.Skip}() requires an IList and an integer parameter.");
		
		if (functionArgs.Parameters.Evaluate(1) is not int numberToSkip)
		{
			throw new FormatException($"{ExtensionFunction.Skip}() requires an IList and an integer parameter.");
		}
		
		functionArgs.Result = list.Cast<object?>()
			.Skip(numberToSkip)
			.Select(value => JValueHelper.UnwrapJValue(value))
			.ToList();
	}
}
