using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("isNull")]
	[Description("Determines whether a value is null.")]
	bool IsNull(
		[Description("The value to be tested.")]
		object? value
	);
}

internal static class IsNull
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsNull}() requires one parameter.");
		}

		try
		{
			var outputObject = functionArgs.Parameters[0].Evaluate();
			functionArgs.Result = outputObject is null || outputObject is JToken { Type: JTokenType.Null };
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException(e.Message);
		}
	}
}
