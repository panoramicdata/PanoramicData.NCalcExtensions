using System.Collections.Concurrent;
using System.Reflection;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("getProperty")]
	[Description("Gets an object's property.")]
	object GetProperty(
		[Description("The source object whose property is to be returned.")]
		object sourceObject,
		[Description("The name of the property to be returned.")]
		string propertyName
	);
}

internal static class GetProperty
{
	private static readonly ConcurrentDictionary<(Type Type, string PropertyName), PropertyInfo> PropertyCache = new();

	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		try
		{
			var value = functionArgs.Parameters.Evaluate(0)
				?? throw new FormatException($"{ExtensionFunction.GetProperty}() first parameter cannot be null.");
			var property = functionArgs.Parameters.Evaluate(1) as string
				?? throw new FormatException($"{ExtensionFunction.GetProperty}() requires two parameters.");

			functionArgs.Result = value switch
			{
				JObject jObject => FromJObject(jObject, property),
				IDictionary<string, object?> dictionary => dictionary[property],
				JsonDocument jsonDocument => FromJsonObject(jsonDocument.RootElement, property, "JsonDocument root element"),
				JsonElement jsonElement => FromJsonObject(jsonElement, property, "JsonElement"),
				_ => FromClrProperty(value, property),
			};
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException or NotSupportedException))
		{
			throw new FormatException($"{ExtensionFunction.GetProperty}() error: {e.Message}", e);
		}
	}

	/// <summary>
	/// The property's value from a JObject, unwrapped to a CLR value where there is one. Null if
	/// the property is absent, JSON null, or undefined.
	/// </summary>
	private static object? FromJObject(JObject jObject, string property)
	{
		var jToken = jObject[property];

		if (jToken is null || jToken.Type is JTokenType.Null or JTokenType.Undefined)
		{
			return null;
		}

		return jToken.Type switch
		{
			JTokenType.Object => (JObject)jToken,
			JTokenType.Array => (JArray)jToken,
			JTokenType.Constructor => (JConstructor)jToken,
			JTokenType.Property => (JProperty)jToken,
			JTokenType.Comment => (JValue)jToken,
			JTokenType.Integer => jToken.Value<int>(),
			JTokenType.Float => jToken.Value<float>(),
			JTokenType.String => jToken.Value<string>(),
			JTokenType.Boolean => jToken.Value<bool>(),
			JTokenType.Date => jToken.Value<DateTime>(),
			JTokenType.Raw or JTokenType.Bytes => (JValue)jToken,
			JTokenType.Guid => jToken.Value<Guid>(),
			_ => throw new NotSupportedException("Unsupported JTokenType: " + jToken.Type)
		};
	}

	/// <summary>
	/// The property's value from a System.Text.Json object, or null if the property is absent.
	/// </summary>
	/// <param name="description">How to name the source in the "must be an object" message.</param>
	private static object? FromJsonObject(JsonElement jsonElement, string property, string description)
	{
		if (jsonElement.ValueKind != JsonValueKind.Object)
		{
			throw new FormatException($"{description} must be an object to access property '{property}'.");
		}

		return jsonElement.TryGetProperty(property, out var propertyElement)
			? ConvertJsonElement(propertyElement)
			: null;
	}

	/// <summary>
	/// The property's value read by reflection, with the PropertyInfo cached per type and name.
	/// </summary>
	private static object? FromClrProperty(object value, string property)
	{
		var propertyInfo = PropertyCache.GetOrAdd(
			(value.GetType(), property),
			static key => key.Type.GetProperty(key.PropertyName)
				?? throw new FormatException($"Could not find property {key.PropertyName} on type {key.Type.Name}"));

		return propertyInfo.GetValue(value);
	}

	private static object? ConvertJsonElement(JsonElement jsonElement) => jsonElement.ValueKind switch
	{
		JsonValueKind.Null => null,
		JsonValueKind.True => true,
		JsonValueKind.False => false,
		JsonValueKind.Number => ConvertJsonNumber(jsonElement),
		JsonValueKind.String => jsonElement.GetString(),
		JsonValueKind.Object => jsonElement,
		JsonValueKind.Array => jsonElement,
		_ => jsonElement
	};

	private static object ConvertJsonNumber(JsonElement jsonElement)
	{
		// Try to convert to the most appropriate numeric type, similar to JToken.ToObject<T>()
		if (jsonElement.TryGetInt32(out var intValue))
		{
			return intValue;
		}

		if (jsonElement.TryGetInt64(out var longValue))
		{
			return longValue;
		}

		if (jsonElement.TryGetDouble(out var doubleValue))
		{
			return doubleValue;
		}
		// Fallback to decimal for high precision numbers
		if (jsonElement.TryGetDecimal(out var decimalValue))
		{
			return decimalValue;
		}
		// Final fallback to double
		return jsonElement.GetDouble();
	}
}