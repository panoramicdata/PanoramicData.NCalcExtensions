using Newtonsoft.Json;

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
	/// <summary>
	/// A type's TryParse method, as used by <see cref="AddValueType{T}"/>.
	/// </summary>
	private delegate bool TryParseValue<T>(string? text, out T value);

	/// <summary>
	/// The type names parse() accepts, mapped to the parser for each. Every parser throws
	/// FormatException for text it cannot parse, which is what selects the fallback value.
	/// </summary>
	/// <remarks>
	/// A lookup table rather than a 17-arm switch that repeated the same failure message once
	/// per supported type. Matching is case-sensitive and ordinal, as the switch was.
	/// </remarks>
	private static readonly FrozenDictionary<string, Func<string, object?>> Parsers = BuildParsers();

	private static FrozenDictionary<string, Func<string, object?>> BuildParsers()
	{
		var parsers = new Dictionary<string, Func<string, object?>>(StringComparer.Ordinal);
		AddValueType<bool>(parsers, bool.TryParse, "bool", "System.Boolean");
		AddValueType<sbyte>(parsers, sbyte.TryParse, "sbyte", "System.SByte");
		AddValueType<byte>(parsers, byte.TryParse, "byte", "System.Byte");
		AddValueType<short>(parsers, short.TryParse, "short", "System.Int16");
		AddValueType<ushort>(parsers, ushort.TryParse, "ushort", "System.UInt16");
		AddValueType<int>(parsers, int.TryParse, "int", "System.Int32");
		AddValueType<uint>(parsers, uint.TryParse, "uint", "System.UInt32");
		AddValueType<long>(parsers, long.TryParse, "long", "System.Int64");
		AddValueType<ulong>(parsers, ulong.TryParse, "ulong", "System.UInt64");
		AddValueType<double>(parsers, double.TryParse, "double", "System.Double");
		AddValueType<float>(parsers, float.TryParse, "float", "System.Single");
		AddValueType<decimal>(parsers, decimal.TryParse, "decimal", "System.Decimal");
		AddValueType<Guid>(parsers, Guid.TryParse, "Guid", "System.Guid");
		parsers["JObject"] = ParseJObject;
		parsers["jObject"] = ParseJObject;
		parsers["Newtonsoft.Json.Linq.JObject"] = ParseJObject;
		parsers["JArray"] = ParseJArray;
		parsers["jArray"] = ParseJArray;
		parsers["Newtonsoft.Json.Linq.JArray"] = ParseJArray;
		parsers["JsonDocument"] = ParseJsonDocument;
		parsers["jsonDocument"] = ParseJsonDocument;
		parsers["System.Text.Json.JsonDocument"] = ParseJsonDocument;
		parsers["JsonArray"] = ParseJsonArray;
		parsers["jsonArray"] = ParseJsonArray;
		return parsers.ToFrozenDictionary(StringComparer.Ordinal);
	}

	/// <summary>
	/// Registers <paramref name="tryParse"/> under each of <paramref name="typeNames"/>.
	/// </summary>
	/// <remarks>
	/// The failure message names the type as the caller spelled it, so each alias gets its own
	/// parser rather than sharing one.
	/// </remarks>
	private static void AddValueType<T>(
		Dictionary<string, Func<string, object?>> parsers,
		TryParseValue<T> tryParse,
		params string[] typeNames)
	{
		foreach (var typeName in typeNames)
		{
			parsers[typeName] = text => tryParse(text, out var value)
				? value
				: throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{typeName}'.");
		}
	}

	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		if (functionArgs.Parameters.Count < 2)
		{
			throw new FormatException($"{ExtensionFunction.Parse} function - requires at least two string parameters.");
		}

		var typeString = functionArgs.Parameters.Evaluate(0) as string
			?? throw new FormatException($"{ExtensionFunction.Parse} function - first parameter should be a string.");
		var text = functionArgs.Parameters.Evaluate(1) as string
			?? throw new FormatException($"{ExtensionFunction.Parse} function - second parameter should be a string.");

		try
		{
			functionArgs.Result = Parsers.TryGetValue(typeString, out var parse)
				? parse(text)
				: throw new FormatException($"type '{typeString}' not supported.");
		}
		catch (FormatException e)
		{
			// A third parameter is the value to fall back to when the text does not parse.
			if (functionArgs.Parameters.Count >= 3)
			{
				functionArgs.Result = functionArgs.Parameters.Evaluate(2);
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

	private static JsonDocument ParseJsonDocument(string text)
	{
		try
		{
			return JsonDocument.Parse(text);
		}
		catch (System.Text.Json.JsonException)
		{
			throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to type '{nameof(JsonDocument)}'.");
		}
	}

	private static JsonDocument ParseJsonArray(string text)
	{
		try
		{
			var jsonDocument = JsonDocument.Parse(text);
			if (jsonDocument.RootElement.ValueKind != JsonValueKind.Array)
			{
				throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' is not a valid JSON array.");
			}

			return jsonDocument;
		}
		catch (System.Text.Json.JsonException)
		{
			throw new FormatException($"{ExtensionFunction.Parse} function - parameter '{text}' could not be parsed to JSON array.");
		}
	}
}
