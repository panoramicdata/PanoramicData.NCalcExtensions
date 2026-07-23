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
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		if (functionArgs.Parameters.Count != 3)
		{
			throw new FormatException($"{ExtensionFunction.If}() requires three parameters.");
		}

		try
		{
			if (functionArgs.Parameters.Evaluate(0) is not bool boolParam1)
			{
				throw new FormatException($"Could not evaluate {ExtensionFunction.If} function parameter 1 '{functionArgs.Parameters[0]}' as boolean.");
			}

			if (boolParam1)
			{
				try
				{
					functionArgs.Result = functionArgs.Parameters.Evaluate(1);
					return;
				}
				catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
				{
					throw new FormatException($"Could not evaluate {ExtensionFunction.If} function parameter 2 '{functionArgs.Parameters[1]}' due to {e.Message}.", e);
				}
			}

			try
			{
				functionArgs.Result = functionArgs.Parameters.Evaluate(2);
			}
			catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
			{
				throw new FormatException($"Could not evaluate {ExtensionFunction.If} function parameter 3 '{functionArgs.Parameters[2]}' due to {e.Message}.", e);
			}
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"Could not evaluate {ExtensionFunction.If} function parameter 1 '{functionArgs.Parameters[0]}'.", e);
		}
	}
}
