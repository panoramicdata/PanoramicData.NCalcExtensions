namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("in")]
	[Description("Determines whether a value is in a set of other values.")]
	bool InFn(
		[Description("The list.")]
		IEnumerable<object?> list,
		[Description("The value to be looked for within the list.")]
		object item
	);
}

internal static class In
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		if (functionArgs.Parameters.Count < 2)
		{
			throw new FormatException($"{ExtensionFunction.In}() requires at least two parameters.");
		}

		try
		{
			var item = functionArgs.Parameters.Evaluate(0);

			// When exactly two parameters are provided and the second is a list, use it as the haystack directly.
			// String is explicitly excluded as it implements IEnumerable<char> but should be treated as a scalar.
			List<object?> list;
			if (functionArgs.Parameters.Count == 2)
			{
				var secondArg = functionArgs.Parameters.Evaluate(1);
				list = secondArg switch
				{
					string s => [s],
					IEnumerable<object?> enumerable => [.. enumerable],
					System.Collections.IEnumerable nonGenericEnumerable => [.. nonGenericEnumerable.Cast<object?>()],
					_ => [secondArg]
				};
			}
			else
			{
				list = [.. Enumerable.Range(1, functionArgs.Parameters.Count - 1).Select(functionArgs.Parameters.Evaluate)];
			}

			functionArgs.Result = list.Contains(item);
		}
		catch (Exception e) when (e is not NCalcExtensionsException)
		{
			throw new FormatException($"{ExtensionFunction.In}() parameters malformed.");
		}
	}
}
