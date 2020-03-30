using NCalc;
using Newtonsoft.Json.Linq;
using PanoramicData.NCalcExtensions.Exceptions;
using System;
using System.Linq;

namespace PanoramicData.NCalcExtensions.Extensions
{
	internal static class IsNull
	{
		internal static void Evaluate(FunctionArgs functionArgs)
		{
			if (functionArgs.Parameters.Length != 1)
			{
				throw new FormatException($"{ExtensionFunction.ToString}() requires one parameter.");
			}
			try
			{
				var outputObject = functionArgs.Parameters[0].Evaluate();
				functionArgs.Result = outputObject is null || (outputObject as JToken)?.Type == JTokenType.Null;
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