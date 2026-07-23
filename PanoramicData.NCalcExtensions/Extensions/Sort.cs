namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("sort")]
	[Description("Sorts an IComparable ascending or descending.")]
	List<object?> Sort(
		[Description("The original list.")]
		IEnumerable<object?> list,
		[Description("(Optional) - 'asc' is the default, 'desc' is the other option.")]
		string direction = "asc"
	);
}

internal static class Sort
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		var parameterIndex = 0;
		var list = functionArgs.Parameters.Evaluate(parameterIndex++) as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.Sort} parameter must be an IEnumerable.");

		var direction = functionArgs.Parameters.Count > 1
			? functionArgs.Parameters.Evaluate(parameterIndex) as string ?? throw new FormatException($"Second {ExtensionFunction.Where} parameter must be a string.")
			: "asc";

		functionArgs.Result = direction.ToUpperInvariant() switch
		{
			"ASC" => [.. list.OrderBy(u => u)],
			"DESC" => list.OrderByDescending(u => u).ToList(),
			_ => throw new FormatException($"If provided, the second {ExtensionFunction.Sort} parameter must be either 'asc' or 'desc'.")
		};
	}
}
