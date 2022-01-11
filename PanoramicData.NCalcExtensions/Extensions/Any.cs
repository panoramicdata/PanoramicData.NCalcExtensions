using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Any
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var value = functionArgs.Parameters.Select(p => p.Evaluate()).ToList<object?>().Collapse() ?? new List<object?>();

		if (value.Any(v => v as bool? is null))
		{
			throw new NCalcExtensionsException("All parameters to the any() function must be bools");
		}

		functionArgs.Result = value.Any(v => v as bool? == true);
	}
}
