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
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var enumerable = functionArgs.Parameters[0].Evaluate() as IList
			?? throw new FormatException($"First {ExtensionFunction.Last} parameter must be an IEnumerable.");

		// If there is only 1 parameter, return the last element of the enumerable
		if (functionArgs.Parameters.Length == 1)
		{
			functionArgs.Result = JValueHelper.UnwrapJValue(enumerable[enumerable.Count - 1]);
			return;
		}

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.Last} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.Last} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);

		var reversedEnumerable = enumerable.Cast<object?>().Reverse();

		foreach (var value in reversedEnumerable)
		{
			if (lambda.Evaluate(value) as bool? == true)
			{
				functionArgs.Result = JValueHelper.UnwrapJValue(value);
				return;
			}
		}

		throw new FormatException($"No matching element found.");
	}
}