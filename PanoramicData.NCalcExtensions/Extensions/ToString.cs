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
	internal static void Evaluate(FunctionEventArgs functionArgs, CultureInfo cultureInfo)
	{
		var parameterCount = functionArgs.Parameters.Count;
		switch (parameterCount)
		{
			case 1:
				var parameter1 = functionArgs.Parameters.Evaluate(0);
				functionArgs.Result = parameter1 switch
				{
					null => null,
					object @object => @object.ToString()
				};
				break;
			case 2:
				var parameter1a = functionArgs.Parameters.Evaluate(0);
				var parameter2 = functionArgs.Parameters.Evaluate(1) as string
					?? throw new FormatException($"{ExtensionFunction.ToString} function -  requires a string as the second parameter.");
				functionArgs.Result = parameter1a switch
				{
					null => null,
					byte value => value.ToString(parameter2, cultureInfo),
					int value => value.ToString(parameter2, cultureInfo),
					uint value => value.ToString(parameter2, cultureInfo),
					long value => value.ToString(parameter2, cultureInfo),
					ulong value => value.ToString(parameter2, cultureInfo),
					short value => value.ToString(parameter2, cultureInfo),
					ushort value => value.ToString(parameter2, cultureInfo),
					float value => value.ToString(parameter2, cultureInfo),
					double value => value.ToString(parameter2, cultureInfo),
					DateTime value => value.ToString(parameter2, cultureInfo),
					DateTimeOffset value => value.ToString(parameter2, cultureInfo),
					object @object => @object.ToString()
				};
				break;
			default:
				throw new FormatException($"{ExtensionFunction.ToString} function -  requires one or two parameters.");
		}
	}
}
