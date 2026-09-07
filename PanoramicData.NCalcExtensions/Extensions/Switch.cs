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
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		if (functionArgs.Parameters.Count < 3)
		{
			throw new FormatException($"{ExtensionFunction.Switch}() requires at least three parameters.");
		}

		try
		{
			functionArgs.Result = SelectResult(functionArgs);
		}
		catch (Exception e) when (e is not (NCalcExtensionsException or FormatException))
		{
			throw new FormatException($"Could not evaluate {ExtensionFunction.Switch} function parameter 1 '{functionArgs.Parameters[0]}'.", e);
		}
	}

	/// <summary>
	/// Returns the value paired with the first case equal to the switch value, or the trailing
	/// default value if one was supplied.
	/// </summary>
	private static object? SelectResult(FunctionEventArgs functionArgs)
	{
		var value = functionArgs.Parameters.Evaluate(0);

		var pairCount = (functionArgs.Parameters.Count - 1) / 2;
		for (var pairIndex = 0; pairIndex < pairCount * 2; pairIndex += 2)
		{
			var caseIndex = 1 + pairIndex;
			if (IsMatch(functionArgs.Parameters.Evaluate(caseIndex), value))
			{
				return functionArgs.Parameters.Evaluate(caseIndex + 1);
			}
		}

		// An even parameter count means the last parameter is a default value rather than a case.
		return functionArgs.Parameters.Count % 2 == 0
			? functionArgs.Parameters.Evaluate(functionArgs.Parameters.Count - 1)
			: throw new FormatException($"Default {ExtensionFunction.Switch} condition occurred, but no default value was specified.");
	}

	/// <summary>
	/// Whether a case matches the switch value. Two nulls match.
	/// </summary>
	private static bool IsMatch(object? caseValue, object? value)
		=> caseValue is null ? value is null : caseValue.Equals(value);
}
