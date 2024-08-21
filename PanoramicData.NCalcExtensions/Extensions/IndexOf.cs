using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("indexOf")]
	[Description("Determines the first position of a string within another string.")]
	int IndexOf(
		[Description("The main text to be searched.")]
		string longString,
		[Description("The shorter text to be looked for within the given main text.")]
		string shortString
	);
}

internal static class IndexOf
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		try
		{
			var param1 = (string)functionArgs.Parameters[0].Evaluate();
			var param2 = (string)functionArgs.Parameters[1].Evaluate();
			functionArgs.Result = param1.IndexOf(param2, StringComparison.InvariantCulture);
		}
		catch (Exception e) when (e is not NCalcExtensionsException)
		{
			throw new FormatException($"{ExtensionFunction.IndexOf}() requires two string parameters.");
		}
	}
}
