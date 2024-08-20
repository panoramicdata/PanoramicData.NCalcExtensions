using System.Collections.Generic;
using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("count")]
	[Description("Counts the number of items. Optionally, only count those that match a lambda.")]
	int Count(
		[Description("The original list")]
		IEnumerable<object?> list,
		[Description("(Optional) a string to represent the value to be evaluated")]
		string? predicate = null,
		[Description("(Optional, but must be provided if predicate is) the string to evaluate")]
		string? exprStr = null
	);
}

internal static class Count
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var listObject = functionArgs.Parameters[0].Evaluate();
		var listEnumerable = listObject as IEnumerable<object?>;

		if (functionArgs.Parameters.Length == 1)
		{
			if (listObject is string text)
			{
				functionArgs.Result = text.Length;
				return;
			}

			if (listEnumerable is null)
			{
				throw new FormatException($"{ExtensionFunction.Count}() requires IEnumerable parameter.");
			}

			functionArgs.Result = listEnumerable.Count();
			return;
		}

		if (listEnumerable is null)
		{
			throw new FormatException($"{ExtensionFunction.Count}() requires IEnumerable parameter.");
		}

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.Count} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.Count} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);

		functionArgs.Result = listEnumerable
			.Count(value =>
			{
				var result = lambda.Evaluate(value) as bool?;
				return result == true;
			});
	}
}