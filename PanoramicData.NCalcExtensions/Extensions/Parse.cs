using Newtonsoft.Json;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Parse
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length < 2)
		{
			throw new FormatException($"{ExtensionFunction.Parse} function - requires at least two string parameters.");
		}

		var parameterIndex = 0;
		var typeString = functionArgs.Parameters[parameterIndex++].Evaluate() as string
			?? throw new FormatException($"{ExtensionFunction.Parse} function - first parameter should be a string.");
		var text = functionArgs.Parameters[parameterIndex++].Evaluate() as string
			?? throw new FormatException($"{ExtensionFunction.Parse} function - second parameter should be a string.");
		try
		{
			functionArgs.Result = typeString switch
			{
				"bool" => bool.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
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
				"Guid" => Guid.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
				"JObject" or "jObject" => ParseJObject(text),
				"JArray" or "jArray" => ParseJArray(text),
				_ => throw new FormatException($"type '{typeString}' not supported.")
			};
		}
		catch (FormatException e)
		{
			if (functionArgs.Parameters.Length >= 3)
			{
				functionArgs.Result = functionArgs.Parameters[parameterIndex].Evaluate();
				return;
			}

			throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'.", e);
		}
	}

	private static JObject ParseJObject(string text)
	{
		try
		{
			return JObject.Parse(text);
		}
		catch (JsonReaderException)
		{
			throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{nameof(JObject)}'.");
		}
	}

	private static JArray ParseJArray(string text)
	{
		try
		{
			return JArray.Parse(text);
		}
		catch (JsonReaderException)
		{
			throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{nameof(JArray)}'.");
		}
	}
}
