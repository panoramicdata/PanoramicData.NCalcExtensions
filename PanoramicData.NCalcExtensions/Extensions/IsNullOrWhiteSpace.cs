namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("IsNullOrWhiteSpace")]
	[Description("Determines whether a value is null, empty or whitespace.")]
	bool IsNullOrWhiteSpace(
		[Description("The value to be tested.")]
		object? value
	);
}

internal static class IsNullOrWhiteSpace
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsNullOrWhiteSpace}() requires one parameter.");
		}

		try
		{
			var outputObject = functionArgs.Parameters[0].Evaluate();
			functionArgs.Result = outputObject is null ||
				outputObject is JToken { Type: JTokenType.Null } ||
				(outputObject is string outputString && string.IsNullOrWhiteSpace(outputString));
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException(e.Message);
		}
	}
}
