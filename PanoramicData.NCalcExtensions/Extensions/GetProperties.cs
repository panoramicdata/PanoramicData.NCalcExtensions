namespace PanoramicData.NCalcExtensions.Extensions;

internal static class GetProperties
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		object value;
		try
		{
			value = functionArgs.Parameters[0].Evaluate();
		}
		catch (Exception e)
		{
			throw new FormatException($"{ExtensionFunction.GetProperty}() requires one parameter.", e);
		}

		if (value is JObject jObject)
		{
			functionArgs.Result = jObject.Properties().Select(p => p.Name).ToList();
		}
		else
		{
			var type = value.GetType();
			functionArgs.Result = type.GetProperties().Select(p => p.Name).ToList();
		}
	}
}