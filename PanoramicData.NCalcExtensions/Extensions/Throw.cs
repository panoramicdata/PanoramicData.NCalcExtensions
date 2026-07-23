namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("throw")]
	[Description("Throws an NCalcExtensionsException. Useful in an if().")]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Method name matches NCalc function name 'throw()' for API consistency")]
	void Throw(
		[Description("(Optional) the exception message.")]
		string? message = null
	);
}

internal static class Throw
{
	internal static Exception Evaluate(FunctionEventArgs functionArgs)
	{
		switch (functionArgs.Parameters.Count)
		{
			case 0:
				return new NCalcExtensionsException();
			case 1:
				if (functionArgs.Parameters.Evaluate(0) is not string exceptionMessageText)
				{
					return new FormatException($"{ExtensionFunction.Throw} function - parameter must be a string.");
				}

				return new NCalcExtensionsException(exceptionMessageText);

			default:
				return new FormatException($"{ExtensionFunction.Throw} function - takes zero or one parameters.");
		}
	}
}
