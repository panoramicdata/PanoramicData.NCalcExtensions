using System.ComponentModel;

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
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsSet}() requires one parameter.");
		}

		functionArgs.Result = functionArgs.Parameters[0].Parameters.Keys.Any(p => p == functionArgs.Parameters[0].Evaluate() as string);
	}
}
