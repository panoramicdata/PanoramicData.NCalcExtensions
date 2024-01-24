using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Join
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		List<string> input;
		string joinString;
		try
		{
			var firstParam = functionArgs.Parameters[0].Evaluate();
			if (firstParam == null)
			{
				input = [];
			}
			else if (firstParam is List<object> objList)
			{
				input = objList
					.Select(u => u?.ToString() ?? string.Empty)
					.ToList();
			}
			else
			{
				input = (firstParam as IEnumerable<string>).ToList();
			}

			joinString = (string)functionArgs.Parameters[1].Evaluate();
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.Join}() requires that the first parameter is a list or enumerable and that the second parameter is a string.");
		}

		functionArgs.Result = string.Join(joinString, input);
	}
}
