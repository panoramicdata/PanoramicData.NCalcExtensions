using PanoramicData.NCalcExtensions.Helpers;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("capitalize")]
	[Description("Capitalizes a text string.")]
	string Capitalize(
		[Description("The text to capitalize.")]
		string text
	);
}

internal static class Capitalize
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var param1 = functionArgs.Parameters[0].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.Capitalize} function - requires one string parameter.");

			functionArgs.Result = param1.ToLowerInvariant().UpperCaseFirst();
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.Capitalize} function - requires one string parameter.");
		}
	}
}
