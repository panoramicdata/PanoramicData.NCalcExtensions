namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Switch
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length < 3)
		{
			throw new FormatException($"{ExtensionFunction.Switch}() requires at least three parameters.");
		}

		object valueParam;
		try
		{
			valueParam = functionArgs.Parameters[0].Evaluate();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception)
		{
			throw new FormatException($"Could not evaluate {ExtensionFunction.Switch} function parameter 1 '{functionArgs.Parameters[0].ParsedExpression}'.");
		}

		// Determine the pair count
		var pairCount = (functionArgs.Parameters.Length - 1) / 2;
		for (var pairIndex = 0; pairIndex < pairCount * 2; pairIndex += 2)
		{
			var caseIndex = 1 + pairIndex;
			var @case = functionArgs.Parameters[caseIndex].Evaluate();
			if (@case.Equals(valueParam))
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
}
