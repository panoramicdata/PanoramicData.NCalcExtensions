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
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.Split}() requires two string parameters.");
		}

		functionArgs.Result = splitString.Length switch
		{
			0 => throw new FormatException($"{ExtensionFunction.Split}() requires that the second parameter is not empty."),
			1 => [.. input.Split(splitString[0])],
			_ => input.Split(new[] { splitString }, StringSplitOptions.None).ToList()
		};
	}
}