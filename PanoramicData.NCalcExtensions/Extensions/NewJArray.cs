namespace PanoramicData.NCalcExtensions.Extensions;

internal static class NewJArray
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var parameterIndex = 0;

		// Create an empty JObject
		var jArray = new JArray();
		while (parameterIndex < functionArgs.Parameters.Length)
		{
			var item = functionArgs.Parameters[parameterIndex++].Evaluate();
			jArray.Add(item is null ? JValue.CreateNull() : JObject.FromObject(item));
		}

		functionArgs.Result = jArray;
	}
}

