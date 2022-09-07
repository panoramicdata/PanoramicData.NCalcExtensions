using Newtonsoft.Json;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Parse
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 2)
		{
			throw new FormatException($"{ExtensionFunction.Parse} function - requires two string parameters.");
		}

		var typeString = functionArgs.Parameters[0].Evaluate() as string
			?? throw new FormatException($"{ExtensionFunction.Parse} function - first parameter should be a string.");
		var text = functionArgs.Parameters[1].Evaluate() as string
			?? throw new FormatException($"{ExtensionFunction.Parse} function - second parameter should be a string.");

		functionArgs.Result = typeString switch
		{
			"sbyte" => sbyte.TryParse(text, out var result) ? result
				 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
			"byte" => byte.TryParse(text, out var result) ? result
				 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
			"short" => short.TryParse(text, out var result) ? result
				 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
			"ushort" => ushort.TryParse(text, out var result) ? result
				 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
			"int" => int.TryParse(text, out var result) ? result
				 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
			"uint" => uint.TryParse(text, out var result) ? result
				 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
			"long" => long.TryParse(text, out var result) ? result
				 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
			"ulong" => ulong.TryParse(text, out var result) ? result
				 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
			"double" => double.TryParse(text, out var result) ? result
				 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
			"float" => float.TryParse(text, out var result) ? result
				 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
			"decimal" => decimal.TryParse(text, out var result) ? result
				 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
			"jObject" => ParseJson(text),
			_ => throw new FormatException($"type '{typeString}' not supported.")
		};
	}

	private static JObject ParseJson(string text)
	{
		try
		{
			return JObject.Parse(text);
		}
		catch (JsonReaderException e)
		{
			throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{nameof(JObject)}'.");
		}
	}
}
