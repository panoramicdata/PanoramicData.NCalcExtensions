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
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		var listObject = functionArgs.Parameters.Evaluate(0);

		if (functionArgs.Parameters.Count == 1 && listObject is string text)
		{
			functionArgs.Result = text.Length;
			return;
		}

		var listEnumerable = listObject as IEnumerable<object?>
			?? throw new FormatException($"{ExtensionFunction.Count}() requires IEnumerable parameter.");

		if (functionArgs.Parameters.Count == 1)
		{
			functionArgs.Result = listEnumerable.Count();
			return;
		}

		var lambda = Parameters.GetLambda(functionArgs, ExtensionFunction.Count);

		functionArgs.Result = listEnumerable.Count(value => lambda.Evaluate(value) as bool? == true);
	}
}
