using Newtonsoft.Json;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("tryParse")]
	[Description("Returns a boolean result of an attempted cast.")]
	object TryParse(
		[Description("The name of the data type.")]
		string type,
		[Description("The text value to be parsed.")]
		string text,
		[Description("The name of the output variable where the result will be stored.")]
		string key
	);
}

internal static class TryParse
{
	/// <summary>
	/// Parses <paramref name="text"/> for one supported type name, storing the outcome in
	/// <paramref name="store"/> under <paramref name="key"/> and returning whether it parsed.
	/// </summary>
	private delegate bool TryParseInto(string text, Dictionary<string, object?> store, string key);

	/// <summary>
	/// The type names tryParse() accepts, mapped to the parser for each.
	/// </summary>
	/// <remarks>
	/// A lookup table rather than a 15-case switch: choosing a parser by name is all the dispatch
	/// ever did. Matching is case-sensitive and ordinal, as the switch was.
	/// </remarks>
	private static readonly FrozenDictionary<string, TryParseInto> Parsers = BuildParsers();

	private static FrozenDictionary<string, TryParseInto> BuildParsers()
	{
		var parsers = new Dictionary<string, TryParseInto>(StringComparer.Ordinal)
		{
			["JObject"] = ForJsonToken<JObject>(),
			["jObject"] = ForJsonToken<JObject>(),
			["JArray"] = ForJsonToken<JArray>(),
			["jArray"] = ForJsonToken<JArray>(),
		};
		ParsableValueTypes.Visit(new ParserRegistration(parsers));
		return parsers.ToFrozenDictionary(StringComparer.Ordinal);
	}

	private sealed class ParserRegistration(Dictionary<string, TryParseInto> parsers) : IParsableValueTypeVisitor
	{
		public void Add<T>(TryParseValue<T> tryParse, params string[] typeNames)
		{
			foreach (var typeName in typeNames)
			{
				parsers[typeName] = ForValueType(tryParse);
			}
		}
	}

	internal static void Evaluate(FunctionEventArgs functionArgs, Dictionary<string, object?> dictionary)
	{
		if (functionArgs.Parameters.Count != 3)
		{
			throw new FormatException($"{ExtensionFunction.Parse} function - requires exactly three string parameters.");
		}

		var typeString = functionArgs.Parameters.Evaluate(0) as string
			?? throw new FormatException($"{ExtensionFunction.Parse} function - first parameter should be a string.");
		var text = functionArgs.Parameters.Evaluate(1) as string
			?? throw new FormatException($"{ExtensionFunction.Parse} function - second parameter should be a string.");
		var outputVariableName = functionArgs.Parameters.Evaluate(2) as string
			?? throw new FormatException($"{ExtensionFunction.Parse} function - third parameter should be a string.");

		if (!Parsers.TryGetValue(typeString, out var parse))
		{
			throw new FormatException($"type '{typeString}' not supported.");
		}

		functionArgs.Result = parse(text, dictionary, outputVariableName);
	}

	/// <summary>
	/// Builds a parser for a value type.
	/// </summary>
	/// <remarks>
	/// The parsed value is stored whether or not parsing succeeded, so a failed parse leaves the
	/// output variable holding the type's default. This is the long-standing behaviour.
	/// </remarks>
	private static TryParseInto ForValueType<T>(TryParseValue<T> tryParse) =>
		(text, store, key) =>
		{
			var success = tryParse(text, out var value);
			store[key] = value;
			return success;
		};

	/// <summary>
	/// Builds a parser for a JSON token type.
	/// </summary>
	/// <remarks>
	/// Unlike the value types, the output variable is left untouched unless the text parses to the
	/// expected token type. This is the long-standing behaviour.
	/// </remarks>
	private static TryParseInto ForJsonToken<T>() where T : JToken =>
		(text, store, key) =>
		{
			try
			{
				if (JsonConvert.DeserializeObject(text) is not T token)
				{
					return false;
				}

				store[key] = token;
				return true;
			}
			catch (JsonReaderException)
			{
				return false;
			}
		};
}
