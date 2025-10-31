﻿namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("retrieve")]
	[Description("Retrieves a value from storage.")]
	object? Retrieve(
		[Description("The key of the item to be retrieved.")]
		string key
	);
}

internal static class Retrieve
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var key = functionArgs.Parameters[0].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.Retrieve}() requires one string parameter.");

			var storageDictionary = functionArgs.Parameters[0].Parameters[ExtendedExpression.StorageDictionaryParameterName] as Dictionary<string, object?>
				?? throw new FormatException($"{ExtensionFunction.Retrieve}() requires a storage dictionary.");

			functionArgs.Result = storageDictionary.TryGetValue(key, out var value)
				? value
				: throw new FormatException("Key not found");
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.Retrieve}() requires one string parameter.");
		}
	}
}
