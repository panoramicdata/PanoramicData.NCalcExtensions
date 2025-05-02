using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("toLower")]
	[Description("Converts a string to lower case.")]
	string ToLower(
		[Description("The string value to be converted.")]
		string value
	);
}


internal static class ToLower
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		string param1;
		try
		{
			param1 = (string)functionArgs.Parameters[0].Evaluate();
			functionArgs.Result = param1.ToLowerInvariant();
		}
		catch (Exception e) when (e is not NCalcExtensionsException)
		{
			throw new FormatException($"{ExtensionFunction.ToLower} function -  requires one string parameter.");
		}
	}
}
