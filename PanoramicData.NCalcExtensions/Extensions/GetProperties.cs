namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("getProperties")]
	[Description("Gets a list of an object's properties.")]
	List<string> GetProperties(
		[Description("The source object whose properties are to be returned.")]
		object sourceObject
	);
}

internal static class GetProperties
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		object value;
		try
		{
			value = functionArgs.Parameters[0].Evaluate();
		}
		catch (Exception e)
		{
			throw new FormatException($"{ExtensionFunction.GetProperty}() requires one parameter.", e);
		}

		functionArgs.Result = value switch
		{
			JObject jObject => [.. jObject
				.Properties()
				.Select(p => p.Name)],
			JsonDocument jsonDocument when jsonDocument.RootElement.ValueKind == JsonValueKind.Object =>
				jsonDocument.RootElement
					.EnumerateObject()
					.Select(p => p.Name)
					.ToList(),
			JsonDocument jsonDocument => throw new FormatException($"JsonDocument root element must be an object to get properties. Found: {jsonDocument.RootElement.ValueKind}"),
			JsonElement jsonElement when jsonElement.ValueKind == JsonValueKind.Object =>
				[.. jsonElement
					.EnumerateObject()
					.Select(p => p.Name)],
			JsonElement jsonElement => throw new FormatException($"JsonElement must be an object to get properties. Found: {jsonElement.ValueKind}"),
			_ => [.. value
				.GetType()
				.GetProperties()
				.Select(p => p.Name)]
		};
	}
}