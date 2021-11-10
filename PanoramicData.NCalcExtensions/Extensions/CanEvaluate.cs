using NCalc;

namespace PanoramicData.NCalcExtensions.Extensions
{
	internal static class CanEvaluate
	{
		internal static void Evaluate(FunctionArgs functionArgs)
		{
			try
			{
				foreach (var parameter in functionArgs.Parameters)
				{
					var outputObject = parameter.Evaluate();
				}
				functionArgs.Result = true;
			}
			catch
			{
				functionArgs.Result = false;
			}
		}
	}
}