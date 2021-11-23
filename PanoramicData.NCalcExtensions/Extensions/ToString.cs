namespace PanoramicData.NCalcExtensions.Extensions;

internal static class ToString
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var parameter1 = functionArgs.Parameters.Length == 1
			? functionArgs.Parameters[0].Evaluate()
			: throw new FormatException($"{ExtensionFunction.ToString} function -  requires one parameter.");

		functionArgs.Result = parameter1 switch
		{
			null => null,
			object @object => @object.ToString()
		};
	}
}
