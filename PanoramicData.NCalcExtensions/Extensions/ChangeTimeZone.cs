using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("changeTimeZone")]
	[Description("Change a DateTime's time zone.")]
	DateTime ChangeTimeZone(
		[Description("The DateTime to change.")]
		DateTime dateTime,
		[Description("The source timezones name, example: 'UTC'")]
		string sourceTz,
		[Description("The target timezones name, example: 'Eastern Standard Time'")]
		string targetTz
	);
}

internal static class ChangeTimeZone
{
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		if (functionArgs.Parameters.Length != 3)
		{
			throw new ArgumentException($"{ExtensionFunction.ChangeTimeZone} function - Expected 3 arguments");
		}

		var argument1 = (functionArgs.Parameters[0].Evaluate() as DateTime?) ?? throw new ArgumentException($"{ExtensionFunction.ChangeTimeZone} function - parameter 1 should be a DateTime");
		var argument2 = (functionArgs.Parameters[1].Evaluate() as string) ?? throw new ArgumentException($"{ExtensionFunction.ChangeTimeZone} function - parameter 2 should be a string");
		var argument3 = (functionArgs.Parameters[2].Evaluate() as string) ?? throw new ArgumentException($"{ExtensionFunction.ChangeTimeZone} function - parameter 3 should be a string");

		functionArgs.Result = ToDateTime.ConvertTimeZone(argument1, argument2, argument3);
	}
}
