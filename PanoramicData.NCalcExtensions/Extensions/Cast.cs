namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Cast
{
	internal static void Evaluate(FunctionArgs functionArgs)
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

		var castType = Type.GetType(castTypeString);
		if (castType == null)
		{
			throw new ArgumentException($"{ExtensionFunction.Cast} function - Expected second argument to be a valid .NET type e.g. System.Decimal.");
		}

		var result = Convert.ChangeType(inputObject, castType, CultureInfo.InvariantCulture);
		functionArgs.Result = result;
		return;
	}
}
