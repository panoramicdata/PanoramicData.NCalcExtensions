using Newtonsoft.Json;
using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("parse")]
	[Description("Returns the conversion of a string to a numeric type. Supported types are: \r\n\r\n"
			+ "- bool or System.Boolean\r\n"
			+ "- sbyte or System.SByte\r\n"
			+ "- byte or System.Byte\r\n"
			+ "- short or System.Int16\r\n"
			+ "- ushort or System.UInt16\r\n"
			+ "- int or System.Int32\r\n"
			+ "- uint or System.UInt32\r\n"
			+ "- long or System.Int64\r\n"
			+ "- ulong or System.UInt64\r\n"
			+ "- double or System.Double\r\n"
			+ "- float or System.Single\r\n"
			+ "- decimal or System.Decimal\r\n"
			+ "- JArray or Newtonsoft.Json.Linq.JArray (jArray also supported for backward compatibility)\r\n"
			+ "- JObject or Newtonsoft.Json.Linq.JObject (jObject also supported for backward compatibility)\r\n"
			+ "- Guid"
	)]
	object? Parse(
		[Description("The data type name to parse the value into (see function description for possible values).")]
		string type,
		[Description("The value to be parsed, given as a string.")]
		string value,
		[Description("(Optional) value to be returned if the parse operation fails.")]
		object? valueIfParseFails = null
	);
}

internal static class Parse
{
	internal static void Evaluate(IFunctionArgs functionArgs)
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
				"bool" or "System.Boolean" => bool.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
				"sbyte" or "System.SByte" => sbyte.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
				"byte" or "System.Byte" => byte.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
				"short" or "System.Int16" => short.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
				"ushort" or "System.UInt16" => ushort.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
				"int" or "System.Int32" => int.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
				"uint" or "System.UInt32" => uint.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
				"long" or "System.Int64" => long.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
				"ulong" or "System.UInt64" => ulong.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
				"double" or "System.Double" => double.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
				"float" or "System.Single" => float.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
				"decimal" or "System.Decimal" => decimal.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
				"Guid" or "System.Guid" => Guid.TryParse(text, out var result) ? result
					 : throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeString}'."),
				"JObject" or "jObject" or "Newtonsoft.Json.Linq.JObject" => ParseJObject(text),
				"JArray" or "jArray" or "Newtonsoft.Json.Linq.JArray" => ParseJArray(text),
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
