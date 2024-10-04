using System.Collections;
using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("length")]
	[Description("Determines length of a given string or IList.")]
	int Length(
		[Description("string or IList")]
		object stringOrList
	);
}

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
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
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