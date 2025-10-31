﻿namespace PanoramicData.NCalcExtensions.Extensions;

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
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var returnNullIfNotFound = false; // False default

		const string SyntaxMessage = ExtensionFunction.JPath + " function - first parameter should be an object capable of being converted to a JObject and second a string jPath expression with optional third parameter " + nameof(returnNullIfNotFound) + ".";

		if (functionArgs.Parameters.Length > 3)
		{
			throw new FormatException(SyntaxMessage);
		}

		try
		{
			var jPathSourceObject = functionArgs.Parameters[0].Evaluate()
				?? throw new NCalcExtensionsException($"{ExtensionFunction.JPath} function - parameter 1 should not be null.");

			var jObject = jPathSourceObject is JObject jObj ? jObj : JObject.FromObject(jPathSourceObject);

			var jPathExpression = functionArgs.Parameters[1].Evaluate() as string
				?? throw new FormatException(SyntaxMessage);

			if (functionArgs.Parameters.Length >= 3)
			{
				if (functionArgs.Parameters[2].Evaluate() is bool returnNullValue)
				{
					returnNullIfNotFound = returnNullValue;
				}
				else
				{
					throw new FormatException($"{ExtensionFunction.JPath} function - parameter 3 should be a bool.");
				}
			}

			JToken? result;
			try
			{
				result = jObject.SelectToken(jPathExpression);
			}
			catch (Exception ex)
			{
				throw new NCalcExtensionsException($"{ExtensionFunction.JPath} function - An unknown issue occurred while trying to select jPathExpression value: {ex.Message}");
			}

			if (result is null)
			{
				if (returnNullIfNotFound)
				{
					functionArgs.Result = null;
					return;
				}
				// Got null, but we didn't ask to returnNullIfNotFound
				throw new NCalcExtensionsException($"{ExtensionFunction.JPath} function - jPath expression did not result in a match.");
			}

			// Try and get the result out of a JValue
			functionArgs.Result = result switch
			{
				JValue jValue => jValue.Value,
				JArray jArray => jArray,
				_ => JObject.FromObject(result)
			};
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException(SyntaxMessage + " ... " + e.Message);
		}
	}
}
