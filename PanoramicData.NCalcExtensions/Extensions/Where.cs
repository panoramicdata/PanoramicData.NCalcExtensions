using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("where")]
	[Description("Filters an IEnumerable to bring back only those items that match a condition.")]
	List<object?> Where(
		[Description("The original list.")]
		IList list,
		[Description("A string to represent the value to be evaluated")]
		string predicate,
		[Description("The string to evaluate")]
		string exprStr
	);
}

internal static class Where
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var list = functionArgs.Parameters[0].Evaluate() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.Where} parameter must be an IEnumerable.");

		var predicate = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.Where} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[2].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.Where} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);

		functionArgs.Result = list
			.Where(value =>
			{
				var result = lambda.Evaluate(value) as bool?;
				return result == true;
			})
			.Select(value => JValueHelper.UnwrapJValue(value))
			.ToList();
	}
}