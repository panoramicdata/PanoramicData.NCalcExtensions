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
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		var list = functionArgs.Parameters.Evaluate(0) as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.Where} parameter must be an IEnumerable.");

		var predicate = functionArgs.Parameters.Evaluate(1) as string
			?? throw new FormatException($"Second {ExtensionFunction.Where} parameter must be a string.");

		var lambdaString = functionArgs.Parameters.Evaluate(2) as string
			?? throw new FormatException($"Third {ExtensionFunction.Where} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Context.StaticParameters);
		var result = list switch
		{
			ICollection<object?> collection => new List<object?>(collection.Count),
			ICollection collection => new List<object?>(collection.Count),
			_ => new List<object?>()
		};

		foreach (var value in list)
		{
			if (lambda.Evaluate(value) as bool? == true)
			{
				result.Add(JValueHelper.UnwrapJValue(value));
			}
		}

		functionArgs.Result = result;
	}
}