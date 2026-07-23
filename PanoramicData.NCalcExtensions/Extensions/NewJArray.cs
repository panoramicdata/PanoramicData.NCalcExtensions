namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("jArray")]
	[Description("Creates and returns a new JArray instance with the given items.")]
	JArray NewJArray(
		[Description("Name of the data type.")]
		params object[] items
	);
}

internal static class NewJArray
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		var parameterIndex = 0;

		// Create an empty JArray
		var jArray = new JArray();
		while (parameterIndex < functionArgs.Parameters.Count)
		{
			var item = functionArgs.Parameters.Evaluate(parameterIndex++);
			jArray.Add(item is null ? JValue.CreateNull() : JToken.FromObject(item));
		}

		functionArgs.Result = jArray;
	}
}

