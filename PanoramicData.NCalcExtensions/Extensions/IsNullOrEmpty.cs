﻿namespace PanoramicData.NCalcExtensions.Extensions;

internal static class IsNullOrEmpty
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsNullOrEmpty}() requires one parameter.");
		}

		try
		{
			var outputObject = functionArgs.Parameters[0].Evaluate();
			functionArgs.Result = outputObject is null ||
				outputObject is JToken { Type: JTokenType.Null } ||
				(outputObject is string outputString && outputString == string.Empty);
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException(e.Message);
		}
	}
}
