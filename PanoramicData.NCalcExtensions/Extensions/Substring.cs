namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Substring
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		string input;
		int startIndex;
		try
		{
			input = (string)functionArgs.Parameters[0].Evaluate();
			startIndex = (int)functionArgs.Parameters[1].Evaluate();
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Substring}() requires a string parameter and one or two numeric parameters.");
		}

		if (functionArgs.Parameters.Length > 2)
		{
			var length = (int)functionArgs.Parameters[2].Evaluate();
			functionArgs.Result = input.Substring(startIndex, Math.Min(length, input.Length - startIndex));
			return;
		}

		functionArgs.Result = input.Substring(startIndex);
	}
}
