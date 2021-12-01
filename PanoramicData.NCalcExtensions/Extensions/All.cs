using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Extensions;

internal static class All
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var value = functionArgs.Parameters.Select(p => p.Evaluate()).ToList<object?>().Collapse() ?? new List<object?>();

		if (value.Any(v => v as bool? is null))
		{
			throw new NCalcExtensionsException("All parameters to the all() function must be bools");
		}
		if (value.Count == 0)
		{
			throw new NCalcExtensionsException("All needs parameters");
		}
		functionArgs.Result = value.All(v => v as bool? == true);
	}
}
