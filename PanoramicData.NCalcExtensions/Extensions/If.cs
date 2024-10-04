using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("if")]
	[Description("Return one of two values, depending on the input function.")]
	object IfFn(
		[Description("The condition to be evaluated.")]
		object condition,
		[Description("The value to return if the condition evaluates to true.")]
		object trueOutput,
		[Description("The value to return if the condition evaluates to false.")]
		object falseOutput
	);
}

internal static class If
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		bool boolParam1;
		if (functionArgs.Parameters.Length != 3)
		{
			throw new FormatException($"{ExtensionFunction.If}() requires three parameters.");
		}

		try
		{
			boolParam1 = (bool)functionArgs.Parameters[0].Evaluate();
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"Could not evaluate {ExtensionFunction.If} function parameter 1 '{functionArgs.Parameters[0].ParsedExpression}'.");
		}

		if (boolParam1)
		{
			try
			{
				functionArgs.Result = functionArgs.Parameters[1].Evaluate();
				return;
			}
			catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
			{
				throw new FormatException($"Could not evaluate {ExtensionFunction.If} function parameter 2 '{functionArgs.Parameters[1].ParsedExpression}' due to {e.Message}.", e);
			}
		}

		try
		{
			functionArgs.Result = functionArgs.Parameters[2].Evaluate();
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"Could not evaluate {ExtensionFunction.If} function parameter 3 '{functionArgs.Parameters[2].ParsedExpression}' due to {e.Message}.", e);
		}
	}
}
