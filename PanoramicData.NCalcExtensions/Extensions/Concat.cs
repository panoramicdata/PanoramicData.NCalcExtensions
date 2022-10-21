using System.Collections;
using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Concat
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var list = new List<object>();
		foreach (var parameter in functionArgs.Parameters)
		{
			var parameterValue = parameter.Evaluate();
			if (parameterValue is IList parameterValueAsIList)
			{
				foreach (var value in parameterValueAsIList)
				{
					list.Add(value);
				}
			}
			else
			{
				list.Add(parameterValue);
			}
		}

		functionArgs.Result = list;
	}
}