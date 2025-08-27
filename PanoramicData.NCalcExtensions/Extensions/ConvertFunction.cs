using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("convert")]
	[Description("Converts the output of parameter 1 into the result of parameter 2.\r\n\r\nNote: Can be used to return an empty string instead of the result of parameter 1, which can be useful when the return value is not useful. The result of parameter 1 is available as the variable \"value\".")]
	object ConvertFunction(
		[Description("Initial parameter to evaluate, will be returned in variable named 'value'.")]
		object firstExpr,
		[Description("Parameter evaluated after initial parameter has been evaluated, example: value + 1")]
		object secondExpr
	);
}

internal static class ConvertFunction
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 2)
		{
			throw new FormatException($"{ExtensionFunction.Convert}() requires two parameters.");
		}

		// Feed the result of the first parameter into the variables available to the second parameter
		var param1 = functionArgs.Parameters[0].Evaluate();
		functionArgs.Parameters[1].Parameters["value"] = param1;
		functionArgs.Result = functionArgs.Parameters[1].Evaluate();
	}
}
