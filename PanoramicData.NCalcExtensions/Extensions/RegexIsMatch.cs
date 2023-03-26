namespace PanoramicData.NCalcExtensions.Extensions;

internal static class RegexIsMatch
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var input = (string)functionArgs.Parameters[0].Evaluate();
			var regexExpression = (string)functionArgs.Parameters[1].Evaluate();
			var regex = new Regex(regexExpression);
			functionArgs.Result = regex.IsMatch(input);
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.RegexIsMatch}() requires two string parameters.");
		}
	}
}
