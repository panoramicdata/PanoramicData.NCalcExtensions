namespace PanoramicData.NCalcExtensions.Extensions;

internal static class ChangeTimeZone
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 3)
		{
			throw new ArgumentException($"{ExtensionFunction.ChangeTimeZone} function - Expected 3 arguments");
		}
		var argument1 = (functionArgs.Parameters[0].Evaluate() as DateTime?) ?? throw new ArgumentException($"{ExtensionFunction.ChangeTimeZone} function - parameter 1 should be a DateTime");
		var argument2 = (functionArgs.Parameters[1].Evaluate() as string) ?? throw new ArgumentException($"{ExtensionFunction.ChangeTimeZone} function - parameter 2 should be a string");
		var argument3 = (functionArgs.Parameters[2].Evaluate() as string) ?? throw new ArgumentException($"{ExtensionFunction.ChangeTimeZone} function - parameter 3 should be a string");

		functionArgs.Result = ToDateTime.ConvertTimeZone(argument1, argument2, argument3);
	}
}
