using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Length
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var value = functionArgs.Parameters[0].Evaluate();

			if (value is string a)
			{
				functionArgs.Result = a.Length;
			}
			else
			{
				functionArgs.Result = GetLength(value);
			}
		}
		catch (Exception)
		{
			throw new FormatException($"{ExtensionFunction.Length}() requires one string or IList parameter.");
		}
	}

	private static int GetLength(object value)
	{
		var a = value as IList;
		if (a is not null)
		{
			return a.Count;
		}

		throw new FormatException($"{ExtensionFunction.Length}() requires one string or IList parameter.");
	}
}