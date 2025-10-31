namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("toUpper")]
	[Description("Converts a string to upper case.")]
	string ToUpper(
		[Description("The string value to be converted.")]
		string value
	);
}

internal static class ToUpper
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var param1 = functionArgs.Parameters[0].Evaluate() as string
				?? throw new FormatException($"{ExtensionFunction.ToUpper} function - requires one string parameter.");

			functionArgs.Result = param1.ToUpperInvariant();
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.ToUpper} function - requires one string parameter.");
		}
	}
}
