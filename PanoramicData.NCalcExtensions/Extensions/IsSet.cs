namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("isSet")]
	[Description("Determines whether a parameter is set.")]
	bool IsSet(
		[Description("The parameter to test for.")]
		string parameterName
	);
}

internal static class IsSet
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		if (functionArgs.Parameters.Count != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsSet}() requires one parameter.");
		}

		functionArgs.Result = functionArgs.Context.StaticParameters.Keys.Any(p => p == functionArgs.Parameters.Evaluate(0) as string);
	}
}
