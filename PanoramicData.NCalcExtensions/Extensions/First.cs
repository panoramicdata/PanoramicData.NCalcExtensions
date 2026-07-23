using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("first")]
	[Description("Returns the first item in a list that matches a lambda or throws a FormatException if no items match.")]
	object First(
		[Description("The list")]
		IEnumerable<object?> list,
		[Description("A string to represent the value to be evaluated")]
		string predicate,
		[Description("The lambda expression as a string")]
		string exprStr
	);
}

internal static class First
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		var enumerable = functionArgs.Parameters.Evaluate(0) as IList
			?? throw new FormatException($"First {ExtensionFunction.First} parameter must be an IEnumerable.");

		// If there is only 1 parameter, return the first element of the enumerable
		if (functionArgs.Parameters.Count == 1)
		{
			functionArgs.Result = JValueHelper.UnwrapJValue(enumerable[0]);
			return;
		}

		var predicate = functionArgs.Parameters.Evaluate(1) as string
			?? throw new FormatException($"Second {ExtensionFunction.First} parameter must be a string.");

		var lambdaString = functionArgs.Parameters.Evaluate(2) as string
			?? throw new FormatException($"Third {ExtensionFunction.First} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Context.StaticParameters);

		foreach (var value in enumerable)
		{
			if (lambda.Evaluate(value) as bool? != true)
			{
				continue;
			}

			functionArgs.Result = JValueHelper.UnwrapJValue(value);
			return;
		}

		throw new FormatException($"No matching element found.");
	}
}
