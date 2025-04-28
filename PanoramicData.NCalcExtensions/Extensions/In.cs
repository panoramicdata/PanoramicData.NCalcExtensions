using System.Collections.Generic;
using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("in")]
	[Description("Determines whether a value is in a set of other values.")]
	bool InFn(
		[Description("The list.")]
		IEnumerable<object?> list,
		[Description("The value to be looked for within the list.")]
		object item
	);
}

internal static class In
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length < 2)
		{
			throw new FormatException($"{ExtensionFunction.In}() requires at least two parameters.");
		}

		try
		{
			var item = functionArgs.Parameters[0].Evaluate();
			var list = functionArgs.Parameters.Skip(1).Select(p => p.Evaluate()).ToList();
			functionArgs.Result = list.Contains(item);
		}
		catch (Exception e) when (e is not NCalcExtensionsException)
		{
			throw new FormatException($"{ExtensionFunction.In}() parameters malformed.");
		}
	}
}
