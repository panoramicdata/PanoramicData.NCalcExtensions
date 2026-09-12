namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("maxValue")]
	[Description("Emits the maximum possible value for a given numeric or date/time type.")]
	object MaxValue(
		[Description("Name of the data type.")]
		string type
	);
}

internal static class MaxValue
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
		=> TypeLimit.Evaluate(functionArgs, ExtensionFunction.MaxValue, isMax: true);
}
