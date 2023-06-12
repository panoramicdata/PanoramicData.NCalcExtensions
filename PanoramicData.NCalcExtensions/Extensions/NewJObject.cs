namespace PanoramicData.NCalcExtensions.Extensions;

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
			throw new FormatException($"{ExtensionFunction.NewJObject}() requires an odd number of parameters.");
		}

		var parameterIndex = 0;

		var original = functionArgs.Parameters[parameterIndex++].Evaluate();
		JObject originalAsJObject;

		if (original is JObject jObject)
		{
			originalAsJObject = jObject;
		}
		else
		{
			originalAsJObject = JObject.FromObject(original);
		}

		// Create an empty JObject
		while (parameterIndex < functionArgs.Parameters.Length)
		{
			if (functionArgs.Parameters[parameterIndex++].Evaluate() is not string key)
			{
				throw new FormatException($"{ExtensionFunction.NewJObject}() requires a string key.");
			}

			if (originalAsJObject.ContainsKey(key))
			{
				throw new FormatException($"{ExtensionFunction.NewJObject}() can only define property {key} once.");
			}

			var value = functionArgs.Parameters[parameterIndex++].Evaluate();
			originalAsJObject.Add(key, value is null ? JValue.CreateNull() : JToken.FromObject(value));
		}

		functionArgs.Result = originalAsJObject;
	}
}

