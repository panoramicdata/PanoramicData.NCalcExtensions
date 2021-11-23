namespace PanoramicData.NCalcExtensions.Extensions;

internal static class RegexGroup
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var input = (string)functionArgs.Parameters[0].Evaluate();
			var regexExpression = (string)functionArgs.Parameters[1].Evaluate();
			var regexCaptureIndex = functionArgs.Parameters.Length == 3
				? (int)functionArgs.Parameters[2].Evaluate()
				: 0;
			var regex = new Regex(regexExpression);
			if (!regex.IsMatch(input))
			{
				functionArgs.Result = null;
			}
			else
			{
				var group = regex
					.Match(input)
					.Groups[1];
				functionArgs.Result = regexCaptureIndex >= group.Captures.Count
					? null
					: group.Captures[regexCaptureIndex].Value;
			}
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Replace}() requires three string parameters.");
		}
	}
}
