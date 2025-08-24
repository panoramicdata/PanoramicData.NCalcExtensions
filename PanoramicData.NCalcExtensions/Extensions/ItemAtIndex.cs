using System.Collections;
using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("itemAtIndex")]
	[Description("Determines the item at the given index. The first index is 0.")]
	object ItemAtIndex(
		[Description("The list of items to be searched.")]
		IList input,
		[Description("Index of item to be returned.")]
		int index
	);
}


internal static class ItemAtIndex
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		IList input;
		int index;
		try
		{
			input = (IList)functionArgs.Parameters[0].Evaluate();

			// Use Convert.ToInt32 instead of (int) cast because otherwise this would fail with
			// an InvalidCastException when doing this: itemAtIndex(list(1, 2, 3, 4), cast(1, 'System.Int64'))
			try
			{
				index = Convert.ToInt32(functionArgs.Parameters[1].Evaluate());
			}
			catch (OverflowException)
			{
				throw new FormatException($"{ExtensionFunction.ItemAtIndex}() index parameter is outside the range of valid integer values (0 - {int.MaxValue}).");
			}

			if (index < 0)
			{
				throw new FormatException($"{ExtensionFunction.ItemAtIndex}() requires two parameters. The first should be an IList and the second should be a non-negative integer.");
			}
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException($"{ExtensionFunction.ItemAtIndex}() requires two parameters. The first should be an IList and the second should be a non-negative integer.");
		}

		functionArgs.Result = input[index];
	}
}
