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

	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var value = functionArgs.Parameters[0].Evaluate()
				?? throw new FormatException($"{ExtensionFunction.GetProperty}() first parameter cannot be null.");
			var property = functionArgs.Parameters[1].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.GetProperty}() requires two parameters.");

			switch (value)
			{
				case JObject jObject:
					{
						var jToken = jObject[property];

						functionArgs.Result = jToken?.Type switch
						{
							null or JTokenType.Null or JTokenType.Undefined => null,
							JTokenType.Object => (JObject)jToken!,
							JTokenType.Array => (JArray)jToken!,
							JTokenType.Constructor => (JConstructor)jToken!,
							JTokenType.Property => (JProperty)jToken!,
							JTokenType.Comment => (JValue)jToken!,
							JTokenType.Integer => jToken!.Value<int>(),
							JTokenType.Float => jToken!.Value<float>(),
							JTokenType.String => jToken!.Value<string>(),
							JTokenType.Boolean => jToken!.Value<bool>(),
							JTokenType.Date => jToken!.Value<DateTime>(),
							JTokenType.Raw or JTokenType.Bytes => (JValue)jToken!,
							JTokenType.Guid => jToken!.Value<Guid>(),
							_ => throw new NotSupportedException("Unsupported JTokenType: " + jToken!.Type)
						};
						break;
					}

				case IDictionary<string, object?> dictionary:
					{
						functionArgs.Result = dictionary[property];
						break;
					}

				case JsonDocument jsonDocument:
					{
						if (jsonDocument.RootElement.ValueKind != JsonValueKind.Object)
						{
							throw new FormatException($"JsonDocument root element must be an object to access property '{property}'.");
						}

						if (jsonDocument.RootElement.TryGetProperty(property, out var jsonElement))
						{
							functionArgs.Result = ConvertJsonElement(jsonElement);
						}
						else
						{
							functionArgs.Result = null;
						}

						break;
					}

				case JsonElement jsonElement:
					{
						if (jsonElement.ValueKind != JsonValueKind.Object)
						{
							throw new FormatException($"JsonElement must be an object to access property '{property}'.");
						}

						if (jsonElement.TryGetProperty(property, out var propertyElement))
						{
							functionArgs.Result = ConvertJsonElement(propertyElement);
						}
						else
						{
							functionArgs.Result = null;
						}

						break;
					}

				default:
					{
						var type = value.GetType();
						var propertyInfo = PropertyCache.GetOrAdd(
							(type, property),
							static key => key.Type.GetProperty(key.PropertyName)
								?? throw new FormatException($"Could not find property {key.PropertyName} on type {key.Type.Name}"));
						functionArgs.Result = propertyInfo.GetValue(value);
						break;
					}
			}
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException or NotSupportedException))
		{
			throw new FormatException($"{ExtensionFunction.GetProperty}() error: {e.Message}", e);
		}
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