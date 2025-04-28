using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("lastIndexOf")]
	[Description("Determines the last position of a string within another string. Returns -1 if not present.")]
	int LastIndexOf(
		[Description("The main text to be searched within.")]
		string longString,
		[Description("Shorter text to be located within the main text.")]
		string shortString
	);
}

internal static class LastIndexOf
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		try
		{
			var param1 = (string)functionArgs.Parameters[0].Evaluate();
			var param2 = (string)functionArgs.Parameters[1].Evaluate();
			functionArgs.Result = param1.LastIndexOf(param2, StringComparison.InvariantCulture);
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.LastIndexOf}() requires two string parameters.");
		}
	}
}
