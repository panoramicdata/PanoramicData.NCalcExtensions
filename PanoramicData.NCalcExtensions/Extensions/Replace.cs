using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("replace")]
	[Description("Replace a string with another string.")]
	string Replace(
		[Description("The input string to be searched.")]
		string haystack,
		[Description("The smaller string being searched for.")]
		string needle,
		[Description("The new replacement text to substituted for each macth.")]
		string newNeedle
	);
}

internal static class Replace
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var haystack = (string)functionArgs.Parameters[0].Evaluate();
			var needle = (string)functionArgs.Parameters[1].Evaluate();
			var newNeedle = (string)functionArgs.Parameters[2].Evaluate();
			functionArgs.Result = haystack.Replace(needle, newNeedle);
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.Replace}() requires three string parameters.");
		}
	}
}
