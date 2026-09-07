namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("any")]
	[Description("Returns true if any values match the lambda expression, otherwise false.")]
	bool Any(
		[Description("The original list")]
		IEnumerable<object?> list,
		[Description("(Optional) a string to represent the value to be evaluated")]
		string? predicate = null,
		[Description("(Optional, but must be provided if predicate is) the string to evaluate")]
		string? exprStr = null
	);
}

internal static class Any
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
		=> functionArgs.Result = functionArgs.Parameters.Count switch
		{
			0 => throw new FormatException($"At least one parameter must be provided for {ExtensionFunction.Any}."),
			1 => GetList(functionArgs).Any(),
			3 => AnyMatchesLambda(functionArgs),
			_ => throw new FormatException($"{ExtensionFunction.Any} takes either 1 or 3 parameters.")
		};

	private static IEnumerable<object?> GetList(FunctionEventArgs functionArgs)
		=> functionArgs.Parameters.Evaluate(0) as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.Any} parameter must be an IEnumerable.");

	/// <summary>
	/// Whether the lambda in parameters 2 and 3 evaluates to true for any value in the list.
	/// </summary>
	private static bool AnyMatchesLambda(FunctionEventArgs functionArgs)
	{
		var list = GetList(functionArgs);

		var predicate = functionArgs.Parameters.Evaluate(1) as string
			?? throw new FormatException($"Second {ExtensionFunction.Any} parameter must be a string.");

		var lambdaString = functionArgs.Parameters.Evaluate(2) as string
			?? throw new FormatException($"Third {ExtensionFunction.Any} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Context.StaticParameters);

		return list.Any(value => lambda.Evaluate(value) as bool? == true);
	}
}
