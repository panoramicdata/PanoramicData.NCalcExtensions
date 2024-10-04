namespace PanoramicData.NCalcExtensions.Extensions;

internal static class ToString
{
	internal static void Evaluate(FunctionArgs functionArgs, CultureInfo cultureInfo)
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
