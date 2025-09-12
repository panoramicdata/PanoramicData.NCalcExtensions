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
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		object value;
		string property;
		try
		{
			value = functionArgs.Parameters[0].Evaluate();
			property = (string)functionArgs.Parameters[1].Evaluate();
		}
		catch (Exception e)
		{
			throw new FormatException($"{ExtensionFunction.GetProperty}() requires two parameters.", e);
		}

		switch (value)
		{
			case JObject jObject:
				{
					var jToken = jObject[property];

					functionArgs.Result = jToken?.Type switch
					{
						null or JTokenType.Null or JTokenType.Undefined => null,
						JTokenType.Object => jToken.ToObject<JObject>(),
						JTokenType.Array => jToken.ToObject<JArray>(),
						JTokenType.Constructor => jToken.ToObject<JConstructor>(),
						JTokenType.Property => jToken.ToObject<JProperty>(),
						JTokenType.Comment => jToken.ToObject<JValue>(),
						JTokenType.Integer => jToken.ToObject<int>(),
						JTokenType.Float => jToken.ToObject<float>(),
						JTokenType.String => jToken.ToObject<string>(),
						JTokenType.Boolean => jToken.ToObject<bool>(),
						JTokenType.Date => jToken.ToObject<DateTime>(),
						JTokenType.Raw or JTokenType.Bytes => jToken.ToObject<JValue>(),
						JTokenType.Guid => jToken.ToObject<Guid>(),
						_ => throw new NotSupportedException("Unsupported JTokenType: " + jToken.Type)
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
					var propertyInfo = type.GetProperty(property) ?? throw new FormatException($"Could not find property {property} on type {type.Name}");
					functionArgs.Result = propertyInfo.GetValue(value);
					break;
				}
		}
	}

	private static object? ConvertJsonElement(JsonElement jsonElement)
	{
		return jsonElement.ValueKind switch
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
	}

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