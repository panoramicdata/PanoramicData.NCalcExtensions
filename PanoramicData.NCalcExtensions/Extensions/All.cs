namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("all")]
	[Description("Returns true if all values match the lambda expression, otherwise false.")]
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
		switch (functionArgs.Parameters.Length)
		{
			case 0:
				// Vacuous truth: all() is true when no values are provided.
				functionArgs.Result = true;
				return;
			case 1:
				var list = functionArgs.Parameters[0].Evaluate() as IEnumerable<object?>
					?? throw new FormatException($"First {ExtensionFunction.All} parameter must be an IEnumerable.");

				functionArgs.Result = list.All(value => JValueHelper.UnwrapJValue(value) as bool? == true);
				return;
			case 3:
				list = functionArgs.Parameters[0].Evaluate() as IEnumerable<object?>
					?? throw new FormatException($"First {ExtensionFunction.All} parameter must be an IEnumerable.");

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
				return;
			default:
				throw new FormatException($"{ExtensionFunction.All} takes either 0, 1 or 3 parameters.");
		}
	}
}