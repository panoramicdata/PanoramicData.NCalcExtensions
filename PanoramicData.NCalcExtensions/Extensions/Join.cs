namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("join")]
	[Description("Joins a list of strings into a single string.")]
	string Join(
		[Description("The list of strings.")]
		IList<string> input
	);
}

internal static class Join
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var firstParam = functionArgs.Parameters[0].Evaluate();
			var input = firstParam == null
				? new List<string>()
				: firstParam is List<object> objList
					? [.. objList.Select(u => u?.ToString() ?? string.Empty)]
					: [.. ((IEnumerable<string>)firstParam)];

			var joinString = functionArgs.Parameters[1].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.Join}() requires that the first parameter is a list or enumerable and that the second parameter is a string.");

			functionArgs.Result = string.Join(joinString, input);
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.Join}() requires that the first parameter is a list or enumerable and that the second parameter is a string.");
		}
	}
}
