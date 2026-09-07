namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("jPath")]
	[Description("Selects a single value from a JObject using a JPath expression.")]
	object? JPath(
		[Description("Input JObject.")]
		JObject input,
		[Description("JPath expression")]
		string path
	);
}

internal static class JPath
{
	private const string SyntaxMessage = ExtensionFunction.JPath
		+ " function - first parameter should be an object capable of being converted to a JObject"
		+ " and second a string jPath expression with optional third parameter returnNullIfNotFound.";

	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		if (functionArgs.Parameters.Count > 3)
		{
			throw new FormatException(SyntaxMessage);
		}

		try
		{
			functionArgs.Result = SelectValue(functionArgs);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException(SyntaxMessage + " ... " + e.Message);
		}
	}

	/// <summary>
	/// Selects the jPath expression's value from the first parameter.
	/// </summary>
	private static object? SelectValue(FunctionEventArgs functionArgs)
	{
		var jPathSourceObject = functionArgs.Parameters.Evaluate(0)
			?? throw new NCalcExtensionsException($"{ExtensionFunction.JPath} function - parameter 1 should not be null.");

		var jObject = jPathSourceObject is JObject jObj ? jObj : JObject.FromObject(jPathSourceObject);

		var jPathExpression = functionArgs.Parameters.Evaluate(1) as string
			?? throw new FormatException(SyntaxMessage);

		var returnNullIfNotFound = ReadReturnNullIfNotFound(functionArgs);

		var result = SelectToken(jObject, jPathExpression);
		if (result is null)
		{
			// Got null, but we didn't ask to returnNullIfNotFound
			return returnNullIfNotFound
				? null
				: throw new NCalcExtensionsException($"{ExtensionFunction.JPath} function - jPath expression did not result in a match.");
		}

		// Try and get the result out of a JValue
		return result switch
		{
			JValue jValue => JValueHelper.UnwrapJValue(jValue),
			JArray jArray => jArray,
			_ => JObject.FromObject(result)
		};
	}

	/// <summary>
	/// The optional third parameter, which defaults to false.
	/// </summary>
	private static bool ReadReturnNullIfNotFound(FunctionEventArgs functionArgs)
	{
		if (functionArgs.Parameters.Count < 3)
		{
			return false;
		}

		return functionArgs.Parameters.Evaluate(2) is bool returnNullValue
			? returnNullValue
			: throw new FormatException($"{ExtensionFunction.JPath} function - parameter 3 should be a bool.");
	}

	private static JToken? SelectToken(JObject jObject, string jPathExpression)
	{
		try
		{
			return jObject.SelectToken(jPathExpression);
		}
		catch (Exception ex)
		{
			throw new NCalcExtensionsException($"{ExtensionFunction.JPath} function - An unknown issue occurred while trying to select jPathExpression value: {ex.Message}");
		}
	}

}
