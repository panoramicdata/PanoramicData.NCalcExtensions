namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("throw")]
	[Description("Throws an NCalcExtensionsException. Useful in an if().")]
	void Throw(
		[Description("(Optional) the exception message.")]
		string? message = null
	);
}

internal static class Throw
{
	internal static Exception Evaluate(IFunctionArgs functionArgs)
	{
		switch (functionArgs.Parameters.Length)
		{
			case 0:
				return new NCalcExtensionsException();
			case 1:
				if (functionArgs.Parameters[0].Evaluate() is not string exceptionMessageText)
				{
					return new FormatException($"{ExtensionFunction.Throw} function - parameter must be a string.");
				}

				return new NCalcExtensionsException(exceptionMessageText);

			default:
				return new FormatException($"{ExtensionFunction.Throw} function - takes zero or one parameters.");
		}
	}
}
