namespace PanoramicData.NCalcExtensions.Extensions;

internal static class List
{
	internal static void Evaluate(FunctionArgs functionArgs)
		=> functionArgs.Result = functionArgs.Parameters.Select(p => p.Evaluate()).ToList();
}
