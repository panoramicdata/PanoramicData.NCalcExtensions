using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("parseInt")]
	[Description("Returns an integer version of a string.")]
	int ParseInt(
		[Description("The integer value to be parsed, represented as a string.")]
		string integerAsString
	);
}

internal static class ParseInt
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.ParseInt} function - requires one string parameter.");
		}

		var param1 = functionArgs.Parameters[0].Evaluate() as string
			?? throw new FormatException($"{ExtensionFunction.ParseInt} function - requires one string parameter.");
		if (!int.TryParse(param1, out var result))
		{
			throw new FormatException($"{ExtensionFunction.ParseInt} function - parameter could not be parsed to an integer.");
		}

		functionArgs.Result = result;
	}
}
