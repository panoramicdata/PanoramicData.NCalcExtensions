namespace PanoramicData.NCalcExtensions.Extensions;

internal static class JPath
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		JObject jPathSourceObject;
		string jPathExpression;
		var returnNullIfNotFound = false; // False default obvs

		const string SyntaxMessage = ExtensionFunction.JPath + " function - first parameter should be a JObject and second a string jPath expression with optional third parameter " + nameof(returnNullIfNotFound) + ".";

		if (functionArgs.Parameters.Length > 3)
		{
			throw new FormatException(SyntaxMessage);
		}

		try
		{
			jPathSourceObject = (JObject)functionArgs.Parameters[0].Evaluate();
			jPathExpression = (string)functionArgs.Parameters[1].Evaluate();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException(SyntaxMessage);
		}

		if (functionArgs.Parameters.Length >= 3)
		{
			try
			{
				returnNullIfNotFound = (bool)functionArgs.Parameters[2].Evaluate();
			}
			catch (Exception)
			{
				throw new FormatException($"{ExtensionFunction.JPath} function - parameter 3 should be a bool.");
			}
		}

		JToken? result;
		try
		{
			result = jPathSourceObject.SelectToken(jPathExpression);
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
		if (result is JValue jValue)
		{
			functionArgs.Result = jValue.Value;
			return;
		}

		functionArgs.Result = JObject.FromObject(result);
	}
}
