namespace PanoramicData.NCalcExtensions.Extensions;

internal static class TypeOf
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var parameter1 = functionArgs.Parameters.Length == 1
			 ? functionArgs.Parameters[0].Evaluate()
			 : throw new FormatException($"{ExtensionFunction.TypeOf} function - requires one parameter.");

		functionArgs.Result = parameter1 != null ? parameter1.GetType().Name : (object?)null;
	}
}
