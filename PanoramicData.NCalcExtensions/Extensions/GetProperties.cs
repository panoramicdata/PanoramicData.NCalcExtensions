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

		functionArgs.Result = value switch
		{
			JObject jObject => jObject
				.Properties()
				.Select(p => p.Name)
				.ToList(),
			_ => value
				.GetType()
				.GetProperties()
				.Select(p => p.Name)
				.ToList()
		};
	}
}