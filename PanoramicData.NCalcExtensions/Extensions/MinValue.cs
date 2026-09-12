namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("minValue")]
	[Description("Emits the minimum possible value for a given numeric or date/time type.")]
	object MinValue(
		[Description("Name of the data type.")]
		string type
	);
}

internal static class MinValue
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
		=> TypeLimit.Evaluate(functionArgs, ExtensionFunction.MinValue, isMax: false);
}
