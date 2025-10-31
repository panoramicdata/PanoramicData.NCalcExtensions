namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("store")]
	[Description("Stores a value for use later in the pipeline. Returns true.")]
	bool Store(
		[Description("The key of the item being stored.")]
		string key,
		[Description("The value to be stored.")]
		object? value
	);
}

internal static class Store
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var key = functionArgs.Parameters[0].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.Store}() requires two parameters.");
			var value = functionArgs.Parameters[1].Evaluate();

			var storageDictionary = functionArgs.Parameters[0].Parameters[ExtendedExpression.StorageDictionaryParameterName] as Dictionary<string, object?>
				?? throw new FormatException($"{ExtensionFunction.Store}() requires a storage dictionary.");

			storageDictionary[key] = value;

			functionArgs.Result = true;
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.Store}() requires two parameters.");
		}
	}
}
