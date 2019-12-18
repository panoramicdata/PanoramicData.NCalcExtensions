using NCalc;
using PanoramicData.NCalcExtensions.Exceptions;
using System;

namespace PanoramicData.NCalcExtensions.Extensions
{
	internal static class Substring
	{
		internal static void Evaluate(FunctionArgs functionArgs)
		{
			int startIndex;
			int length;
			string param1;
			try
			{
				param1 = (string)functionArgs.Parameters[0].Evaluate();
				startIndex = (int)functionArgs.Parameters[1].Evaluate();
			}
			catch (NCalcExtensionsException)
			{
				throw;
			}
			catch (Exception)
			{
				throw new FormatException($"{ExtensionFunction.Substring}() requires a string parameter and one or two numeric parameters.");
			}
			if (functionArgs.Parameters.Length > 2)
			{
				length = (int)functionArgs.Parameters[2].Evaluate();
				functionArgs.Result = param1.Substring(startIndex, length);
				return;
			}

			functionArgs.Result = param1.Substring(startIndex);
		}
	}
}