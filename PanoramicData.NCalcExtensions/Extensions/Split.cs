namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Split
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		string input;
		string splitString;
		try
		{
			input = (string)functionArgs.Parameters[0].Evaluate();
			splitString = (string)functionArgs.Parameters[1].Evaluate();
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Split}() requires two string parameters.");
		}

		if (splitString.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.Split}()'s second parameter must be a single character.");
		}

		functionArgs.Result = input.Split(splitString[0]).ToList();
	}
}