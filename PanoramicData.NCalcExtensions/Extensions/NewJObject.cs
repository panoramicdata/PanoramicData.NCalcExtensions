namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("jObject")]
	[Description("Creates and returns a new JObject instance with the given properties.")]
	JArray NewJObject(
		[Description("Interlaced keys and values. You must provide an even number of parameters, and names must evaluate to strings.")]
		params object[] props
	);
}

internal static class NewJObject
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length % 2 != 0)
		{
			throw new FormatException($"{ExtensionFunction.NewJObject}() requires an even number of parameters.");
		}

		var parameterIndex = 0;

		// Create an empty JObject
		var jObject = new JObject();
		while (parameterIndex < functionArgs.Parameters.Length)
		{
			if (functionArgs.Parameters[parameterIndex++].Evaluate() is not string key)
			{
				throw new FormatException($"{ExtensionFunction.NewJObject}() requires a string key.");
			}

			if (jObject.ContainsKey(key))
			{
				throw new FormatException($"{ExtensionFunction.NewJObject}() can only define property {key} once.");
			}

			var value = functionArgs.Parameters[parameterIndex++].Evaluate();
			jObject.Add(key, value is null ? JValue.CreateNull() : JToken.FromObject(value));
		}

		functionArgs.Result = jObject;
	}
}

internal static class SetProperties
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length % 2 != 1)
		{
			throw new FormatException($"{ExtensionFunction.SetProperties}() requires an odd number of parameters.");
		}

		var parameterIndex = 0;

		var original = functionArgs.Parameters[parameterIndex++].Evaluate();
		var originalAsJObject = original is JObject jObject
			? jObject
			: JObject.FromObject(original);

		// Create an empty JObject
		while (parameterIndex < functionArgs.Parameters.Length)
		{
			if (functionArgs.Parameters[parameterIndex++].Evaluate() is not string key)
			{
				throw new FormatException($"{ExtensionFunction.SetProperties}() requires a string key.");
			}

			if (originalAsJObject.ContainsKey(key))
			{
				throw new FormatException($"{ExtensionFunction.SetProperties}() can only define property {key} once.");
			}

			var value = functionArgs.Parameters[parameterIndex++].Evaluate();
			originalAsJObject.Add(key, value is null ? JValue.CreateNull() : JToken.FromObject(value));
		}

		functionArgs.Result = originalAsJObject;
	}
}

