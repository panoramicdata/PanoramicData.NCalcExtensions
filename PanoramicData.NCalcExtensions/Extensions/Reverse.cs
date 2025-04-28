using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

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
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		var enumerable = functionArgs.Parameters[0].Evaluate() as IList
			?? throw new FormatException($"First {ExtensionFunction.Reverse} parameter must be an IEnumerable.");

		functionArgs.Result = enumerable.Cast<object?>().Reverse().ToList();
	}
}
