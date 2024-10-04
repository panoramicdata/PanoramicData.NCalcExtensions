using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("isGuid")]
	[Description("Determines whether a value is a GUID, or is a string that can be converted to a GUID.")]
	bool IsGuid(
		[Description("The value to be tested.")]
		object? value
	);
}

internal static class IsGuid
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsGuid}() requires one parameter.");
		}

		var value = functionArgs.Parameters[0].Evaluate();
		functionArgs.Result = value is Guid || (value is string && Guid.TryParse(value.ToString(), out var _));
	}
}
