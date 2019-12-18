using NCalc;
using PanoramicData.NCalcExtensions.Exceptions;
using System;

namespace PanoramicData.NCalcExtensions.Extensions
{
	internal static class IsInfinite
	{
		internal static void Evaluate(FunctionArgs functionArgs)
		{
			if (functionArgs.Parameters.Length != 1)
			{
				throw new FormatException($"{ExtensionFunction.IsInfinite}() requires one parameter.");
			}
			try
			{
				var outputObject = functionArgs.Parameters[0].Evaluate();
				functionArgs.Result =
					outputObject is double x && (
						double.IsPositiveInfinity(x)
						|| double.IsNegativeInfinity(x)
					);
				return;
			}
			catch (NCalcExtensionsException)
			{
				throw;
			}
			catch (FormatException)
			{
				throw;
			}
			catch (Exception e)
			{
				throw new FormatException(e.Message);
			}
		}
	}
}