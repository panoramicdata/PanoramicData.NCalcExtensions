namespace PanoramicData.NCalcExtensions.Extensions;

internal static class GetProperty
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		object value = functionArgs.Parameters[0].Evaluate();
		string property = (string)functionArgs.Parameters[1].Evaluate();

		switch (value)
		{
			case JObject jObject:
				var jToken = jObject[property];

				functionArgs.Result = (jToken?.Type) switch
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
					_ => throw new NotSupportedException("Unsupported JTokenType: " + jToken.Type),
				};
				break;
			default:
				var type = value.GetType();
				var propertyInfo = type.GetProperty(property) ?? throw new FormatException($"Could not find property {property} on type {type.Name}");
				functionArgs.Result = propertyInfo.GetValue(value);
				break;
		}
	}
}