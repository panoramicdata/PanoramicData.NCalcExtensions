using System.Collections.Generic;
using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("distinct")]
	[Description("Returns only distinct items from the input.")]
	List<object?> Distinct(
		[Description("The original list.")]
		IEnumerable<object?> list
	);
}

internal static class Distinct
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		var enumerable = functionArgs.Parameters[0].Evaluate() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.Distinct} parameter must be an IEnumerable.");

		functionArgs.Result = enumerable
			.Distinct()
			.ToList();
	}
}
