using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("firstOrDefault")]
	[Description("Returns the first item in a list that matches a lambda or null if no items match.")]
	object? FirstOrDefault(
		[Description("The list")]
		IEnumerable<object?> list,
		[Description("A string to represent the value to be evaluated")]
		string predicate,
		[Description("The lambda expression as a string")]
		string exprStr
	);
}

internal static class FirstOrDefault
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		var enumerable = functionArgs.Parameters[0].Evaluate() as IList
			?? throw new FormatException($"First {ExtensionFunction.FirstOrDefault} parameter must be an IEnumerable.");

		// If there is only 1 parameter, return the first element of the enumerable
		if (functionArgs.Parameters.Length == 1)
		{
			functionArgs.Result = enumerable.Count == 0 ? null : enumerable[0];
			return;
		}

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.FirstOrDefault} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.FirstOrDefault} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);

		foreach (var value in enumerable)
		{
			if (lambda.Evaluate(value) as bool? != true)
			{
				continue;
			}

			functionArgs.Result = value;
			return;
		}

		functionArgs.Result = null;
	}
}
