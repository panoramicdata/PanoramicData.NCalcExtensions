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
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		try
		{
			var value = functionArgs.Parameters[0].Evaluate();

			functionArgs.Result = value is string a ? a.Length : (object)GetLength(value);
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