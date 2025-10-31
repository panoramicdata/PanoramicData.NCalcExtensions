namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("switch")]
	[Description("Return one of a number of values, depending on the input function.")]
	object? Switch(
		[Description("The value being examined.")]
		object value,
		[Description("A set of pairs: case_n, output_n.\r\n\r\nNote: if present, a final value can be used as a default. If the default WOULD have been returned, but no default is present, an exception is thrown.")]
		params object[] pairs
	);
}

internal static class Switch
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length < 3)
		{
			throw new FormatException($"{ExtensionFunction.Switch}() requires at least three parameters.");
		}

		try
		{
			var valueParam = functionArgs.Parameters[0].Evaluate();

			// Determine the pair count
			var pairCount = (functionArgs.Parameters.Length - 1) / 2;
			for (var pairIndex = 0; pairIndex < pairCount * 2; pairIndex += 2)
			{
				var caseIndex = 1 + pairIndex;
				var @case = functionArgs.Parameters[caseIndex].Evaluate();
				if (@case?.Equals(valueParam) == true || (@case == null && valueParam == null))
				{
					functionArgs.Result = functionArgs.Parameters[caseIndex + 1].Evaluate();
					return;
				}
			}

			var defaultIsPresent = functionArgs.Parameters.Length % 2 == 0;
			if (defaultIsPresent)
			{
				functionArgs.Result = functionArgs.Parameters.Last().Evaluate();
				return;
			}

			throw new FormatException($"Default {ExtensionFunction.Switch} condition occurred, but no default value was specified.");
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"Could not evaluate {ExtensionFunction.Switch} function parameter 1 '{functionArgs.Parameters[0].ExpressionString}'.", e);
		}
	}
}
