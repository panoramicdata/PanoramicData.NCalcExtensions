using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("isNaN")]
	[Description("Determines whether a value is not a number.")]
	bool IsNaN(
		[Description("The value to be tested.")]
		object? value
	);
}

internal static class IsNaN
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 1)
		{
			throw new FormatException($"{ExtensionFunction.IsNaN}() requires one parameter.");
		}

		try
		{
			var outputObject = functionArgs.Parameters[0].Evaluate();
			functionArgs.Result = outputObject switch
			{
				double d => double.IsNaN(d),
				float f => float.IsNaN(f),
				int or long or short or byte or sbyte or uint or ulong or ushort or decimal => false,
				_ => true
			};
			return;
		}
		catch (Exception e) when (e is not NCalcExtensionsException or FormatException)
		{
			throw new FormatException(e.Message);
		}
	}
}
