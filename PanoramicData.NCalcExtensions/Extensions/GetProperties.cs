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
	internal static void Evaluate(IFunctionArgs functionArgs)
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
			JObject jObject => jObject
				.Properties()
				.Select(p => p.Name)
				.ToList(),
			_ => [.. value
				.GetType()
				.GetProperties()
				.Select(p => p.Name)]
		};
	}
}