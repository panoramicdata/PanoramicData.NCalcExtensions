using System.Collections;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("flatten")]
	[Description("Flattens a list of lists into a single flat list.")]
	IList Flatten(
		[Description("The list of lists to flatten.")]
		IList listOfLists
	);
}

internal static class FlattenFunction
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var raw = functionArgs.Parameters[0].Evaluate()
			?? throw new FormatException($"First {ExtensionFunction.Flatten} parameter cannot be null.");

		if (raw is not IEnumerable<object?> outerList)
		{
			throw new FormatException($"{ExtensionFunction.Flatten}() requires a list of lists as parameter 1.");
		}

		var result = new List<object?>();
		foreach (var item in outerList)
		{
			if (item is IEnumerable<object?> inner)
			{
				result.AddRange(inner);
			}
			else if (item is IEnumerable nonGenericInner and not string)
			{
				foreach (var innerItem in nonGenericInner)
				{
					result.Add(innerItem);
				}
			}
			else
			{
				result.Add(item);
			}
		}

		functionArgs.Result = result;
	}
}
