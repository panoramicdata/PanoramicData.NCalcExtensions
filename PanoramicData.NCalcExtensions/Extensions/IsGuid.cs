namespace PanoramicData.NCalcExtensions.Extensions;

internal static class IsGuid
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsGuid}() requires one parameter.");
		}

		var value = functionArgs.Parameters[0].Evaluate();
		functionArgs.Result = value is Guid || (value is string && Guid.TryParse(value.ToString(), out var _));
	}
}
