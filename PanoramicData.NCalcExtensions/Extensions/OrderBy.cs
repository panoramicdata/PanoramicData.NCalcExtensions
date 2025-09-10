namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("orderBy")]
	[Description("Orders an IEnumerable by one or more lambda expressions.")]
	List<object?> OrderBy(
		[Description("The original list")]
		IEnumerable<object?> list,
		[Description("A string to represent the value to be evaluated")]
		string predicate,
		[Description("The first orderBy lambda expression")]
		string nCalcString1,
		[Description("(Optional) A second orderBy lambda expression")]
		string? nCalcString2 = null,
		[Description("(Optional) A third orderBy lambda expression")]
		string? nCalcString3 = null
	);
}

internal static class OrderBy
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var parameterIndex = 0;
		var list = functionArgs.Parameters[parameterIndex++].Evaluate() as IEnumerable<object?>
			?? throw new FormatException($"First {ExtensionFunction.OrderBy} parameter must be an IEnumerable.");

		var predicate = functionArgs.Parameters[parameterIndex++].Evaluate() as string
			?? throw new FormatException($"Second {ExtensionFunction.OrderBy} parameter must be a string.");

		var lambdaString = functionArgs.Parameters[parameterIndex++].Evaluate() as string
			?? throw new FormatException($"Third {ExtensionFunction.OrderBy} parameter must be a string.");

		var lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);

		IOrderedEnumerable<object?> orderable = list
			.OrderBy(value =>
			{
				var result = lambda.Evaluate(value);
				return result;
			}, ObjectKeyComparer.Instance);

		var parameterCount = functionArgs.Parameters.Length;

		while (parameterIndex < parameterCount)
		{
			lambdaString = functionArgs.Parameters[parameterIndex++].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.OrderBy} parameter {parameterIndex + 1} must be a string.");
			lambda = new Lambda(predicate, lambdaString, functionArgs.Parameters[0].Parameters);
			orderable = orderable
						.ThenBy(value =>
						{
							var result = lambda.Evaluate(value);
							return result;
						}, ObjectKeyComparer.Instance);
		}

		functionArgs.Result = orderable.ToList();
	}
}

internal sealed class ObjectKeyComparer : IComparer<object?>
{
	public static readonly ObjectKeyComparer Instance = new();

	public int Compare(object? x, object? y)
	{
		if (ReferenceEquals(x, y)) return 0;
		if (x is null) return -1;
		if (y is null) return 1;

		// Allow mixed numeric comparisons (e.g., int vs double)
		if (TryToDouble(x, out var dx) && TryToDouble(y, out var dy))
		{
			return dx.CompareTo(dy);
		}

		// Fall back to default behavior for non-numeric keys
		return Comparer<object>.Default.Compare(x, y);
	}

	private static bool TryToDouble(object value, out double result)
	{
		switch (value)
		{
			case byte b: result = b; return true;
			case sbyte sb: result = sb; return true;
			case short s: result = s; return true;
			case ushort us: result = us; return true;
			case int i: result = i; return true;
			case uint ui: result = ui; return true;
			case long l: result = l; return true;
			case ulong ul: result = ul; return true;
			case float f: result = f; return true;
			case double d: result = d; return true;
			case decimal m: result = (double)m; return true;
			default: result = 0; return false;
		}
	}
}
