using System.Collections.Generic;
using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("dictionary")]
	[Description("Emits a Dictionary<string, object?>.\r\n\r\nExample: dictionary('TRUE', true, 'FALSE', false)")]
	Dictionary<string, object?> Dictionary(
		[Description("Interlaced keys and values. You must provide an even number of parameters, and keys must evaluate to strings.")]
		params object[] parameters
	);
}

internal static class Dictionary
{
	/// <summary>
	/// Create a Dictionary<string, object?> from a set of parameters
	/// </summary>
	/// <param name="functionArgs">The arguments from the user, provided to us by NCalc</param>
	/// <exception cref="FormatException">An odd number of parameters were provided, or one of the keys is not a string</exception>
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		ExpressionHelper.CheckParameterCount(ExtensionFunction.Dictionary, functionArgs, 2, int.MaxValue - 1);          // Between 2 and a lot
		if (functionArgs.Parameters.Length % 2 != 0)
		{
			throw new FormatException($"{ExtensionFunction.Dictionary}: An even number of parameters must be provided.");
		}

		string? key = null;
		var dictionary = new Dictionary<string, object?>();
		for (var index = 0; index < functionArgs.Parameters.Length; index++)
		{
			var parameterValue = functionArgs.Parameters[index].Evaluate();
			if (index % 2 == 0)
			{
				// Even parameter numbers are the key
				if (parameterValue is not string)
				{
					throw new FormatException($"{ExtensionFunction.Dictionary}: parameter {index} must be a string.");
				}

				key = (string)parameterValue;
			}
			else
			{
				// Odd parameter numbers are the value, to be added using the key
				dictionary[key!] = parameterValue;
			}
		}

		functionArgs.Result = dictionary;
	}
}
