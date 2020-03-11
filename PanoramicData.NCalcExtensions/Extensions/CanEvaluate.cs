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
#pragma warning disable CA1031 // Do not catch general exception types
			catch
#pragma warning restore CA1031 // Do not catch general exception types
			{
				functionArgs.Result = false;
			}
		}
	}
}