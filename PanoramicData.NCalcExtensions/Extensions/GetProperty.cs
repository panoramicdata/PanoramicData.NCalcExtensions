using System.ComponentModel;

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

		if (value is JObject jObject)
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
		}
		else
		{
			var type = value.GetType();
			var propertyInfo = type.GetProperty(property) ?? throw new FormatException($"Could not find property {property} on type {type.Name}");
			functionArgs.Result = propertyInfo.GetValue(value);
		}
	}
}