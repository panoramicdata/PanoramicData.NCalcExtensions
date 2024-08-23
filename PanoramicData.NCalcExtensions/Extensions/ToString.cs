using System.ComponentModel;

namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("toString")]
	[Description("Converts any object to a string.")]
	string ToString(
		[Description("The string value to be converted.")]
		object value,
		[Description("(Optional) Format of the output.")]
		string? format = null
	);
}

internal static class ToString
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var parameterCount = functionArgs.Parameters.Length;
		switch (parameterCount)
		{
			case 1:
				var parameter1 = functionArgs.Parameters[0].Evaluate();
				functionArgs.Result = parameter1 switch
				{
					null => null,
					object @object => @object.ToString()
				};
				break;
			case 2:
				var parameter1a = functionArgs.Parameters[0].Evaluate();
				var parameter2 = functionArgs.Parameters[1].Evaluate() as string
					?? throw new FormatException($"{ExtensionFunction.ToString} function -  requires a string as the second parameter.");
				functionArgs.Result = parameter1a switch
				{
					null => null,
					byte value => value.ToString(parameter2, CultureInfo.InvariantCulture),
					int value => value.ToString(parameter2, CultureInfo.InvariantCulture),
					uint value => value.ToString(parameter2, CultureInfo.InvariantCulture),
					long value => value.ToString(parameter2, CultureInfo.InvariantCulture),
					ulong value => value.ToString(parameter2, CultureInfo.InvariantCulture),
					short value => value.ToString(parameter2, CultureInfo.InvariantCulture),
					ushort value => value.ToString(parameter2, CultureInfo.InvariantCulture),
					float value => value.ToString(parameter2, CultureInfo.InvariantCulture),
					double value => value.ToString(parameter2, CultureInfo.InvariantCulture),
					DateTime value => value.ToString(parameter2, CultureInfo.InvariantCulture),
					DateTimeOffset value => value.ToString(parameter2, CultureInfo.InvariantCulture),
					object @object => @object.ToString()
				};
				break;
			default:
				throw new FormatException($"{ExtensionFunction.ToString} function -  requires one or two parameters.");
		}
	}
}
