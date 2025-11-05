using System.Collections;

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
			var value = functionArgs.Parameters[0].Evaluate()
				?? throw new FormatException($"{ExtensionFunction.Length}() requires one non-null string or IList parameter.");

			functionArgs.Result = value is string a ? a.Length : GetLength(value);
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.Length}() requires one string or IList parameter.");
		}
	}

	private static int GetLength(object value)
	{
		if (value is IList a)
		{
			return a.Count;
		}

		throw new FormatException($"{ExtensionFunction.Length}() requires one string or IList parameter.");
	}
}