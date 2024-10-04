using System.Collections.Generic;
using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("all")]
	[Description("Returns true if any values match the lambda expression, otherwise false.")]
	bool All(
		[Description("The original list")]
		IEnumerable<object?> list,
		[Description("(Optional) a string to represent the value to be evaluated")]
		string? predicate = null,
		[Description("(Optional, but must be provided if predicate is) the string to evaluate")]
		string? exprStr = null
	);
}

internal static class All
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var list = functionArgs.Parameters[0].Evaluate() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.All} parameter must be an IEnumerable.");

		if (functionArgs.Parameters.Length == 1)
		{
			functionArgs.Result = list.All(value => value as bool? == true);
			return;
		}

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.All} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.All} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);

		functionArgs.Result = list
			.All(value =>
			{
				var result = lambda.Evaluate(value) as bool?;
				return result == true;
			});
	}
}