namespace PanoramicData.NCalcExtensions.Extensions;

internal static class RegexIsMatch
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var input = functionArgs.Parameters[0].Evaluate();
		var regexExpression = functionArgs.Parameters[1].Evaluate();
		if (input is not string inputString)
		{
			throw new FormatException($"{ExtensionFunction.RegexIsMatch} function - first parameter should be a string.");
		}

		if (regexExpression is not string regexExpressionString)
		{
			throw new FormatException($"{ExtensionFunction.RegexIsMatch} function - second parameter should be a string.");
		}

		var regex = new Regex(regexExpressionString);
		functionArgs.Result = regex.IsMatch(inputString);
	}
}
