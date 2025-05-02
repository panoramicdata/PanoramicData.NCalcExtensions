using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("cast")]
	[Description("Cast an object to another (e.g. float to decimal).\r\n\r\nNote: The method requires that conversion of value to target type be supported.")]
	object Cast(
		[Description("The object to be cast.")]
		object inputObject,
		[Description("The full name of the data type to cast to, example: 'System.Decimal'")]
		string typeName
	);
}

internal static class Cast
{
	internal static void Evaluate(IFunctionArgs functionArgs, CultureInfo cultureInfo)
	{
		const int castParameterCount = 2;
		if (functionArgs.Parameters.Length != castParameterCount)
		{
			throw new ArgumentException($"{ExtensionFunction.Cast} function - Expected {castParameterCount} arguments");
		}

		var inputObject = functionArgs.Parameters[0].Evaluate();
		if (functionArgs.Parameters[1].Evaluate() is not string castTypeString)
		{
			throw new ArgumentException($"{ExtensionFunction.Cast} function - Expected second argument to be a string.");
		}

		var castType = Type.GetType(castTypeString)
			?? throw new ArgumentException($"{ExtensionFunction.Cast} function - Expected second argument to be a valid .NET type e.g. System.Decimal.");

		var result = Convert.ChangeType(inputObject, castType, cultureInfo);
		functionArgs.Result = result;
		return;
	}
}
