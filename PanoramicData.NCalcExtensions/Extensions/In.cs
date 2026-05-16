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
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length < 2)
		{
			throw new FormatException($"{ExtensionFunction.In}() requires at least two parameters.");
		}

		try
		{
			var item = functionArgs.Parameters[0].Evaluate();

			// When exactly two parameters are provided and the second is a list, use it as the haystack directly.
			// String is explicitly excluded as it implements IEnumerable<char> but should be treated as a scalar.
			List<object?> list;
			if (functionArgs.Parameters.Length == 2)
			{
				var secondArg = functionArgs.Parameters[1].Evaluate();
				list = secondArg switch
				{
					string s => [s],
					IEnumerable<object?> enumerable => enumerable.ToList(),
					System.Collections.IEnumerable nonGenericEnumerable => nonGenericEnumerable.Cast<object?>().ToList(),
					_ => [secondArg]
				};
			}
			else
			{
				list = [.. functionArgs.Parameters.Skip(1).Select(p => p.Evaluate())];
			}

			functionArgs.Result = list.Contains(item);
		}
		catch (Exception e) when (e is not NCalcExtensionsException)
		{
			throw new FormatException($"{ExtensionFunction.In}() parameters malformed.");
		}
	}
}
