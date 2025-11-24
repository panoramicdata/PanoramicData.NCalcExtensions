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
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var enumerable = functionArgs.Parameters[0].Evaluate() as IList
			?? throw new FormatException($"First {ExtensionFunction.LastOrDefault} parameter must be an IEnumerable.");

		// If there is only 1 parameter, return the last element of the enumerable
		if (functionArgs.Parameters.Length == 1)
		{
			functionArgs.Result = enumerable.Count == 0 ? null : JValueHelper.UnwrapJValue(enumerable[enumerable.Count - 1]);
			return;
		}

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.LastOrDefault} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.LastOrDefault} parameter must be a string.");

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

		functionArgs.Result = null;
	}
}

