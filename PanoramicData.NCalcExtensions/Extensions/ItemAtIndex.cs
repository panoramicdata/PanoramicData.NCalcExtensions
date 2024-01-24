using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class ItemAtIndex
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		IList input;
		int index;
		try
		{
			input = (IList)functionArgs.Parameters[0].Evaluate();
			index = (int)functionArgs.Parameters[1].Evaluate();
			if (index < 0)
			{
				throw new FormatException($"{ExtensionFunction.ItemAtIndex}() requires two parameters.  The first should be an IList and the second should be a non-negative integer.");
			}
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.ItemAtIndex}() requires two parameters.  The first should be an IList and the second should be a non-negative integer.");
		}

		functionArgs.Result = input[index];
	}
}
