namespace PanoramicData.NCalcExtensions.Extensions;

internal static class GetProperty
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		object value;
		string property;
		try
		{
			value = functionArgs.Parameters[0].Evaluate();
			property = (string)functionArgs.Parameters[1].Evaluate();
		}
		catch (NCalcExtensionsException)
		{
			throw;
		}
		catch (Exception e)
		{
			throw new FormatException($"{ExtensionFunction.GetProperty}() requires two parameters.", e);
		}

		var type = value.GetType();
		var propertyInfo = type.GetProperty(property) ?? throw new FormatException($"Could not find property {property} on type {type.Name}");
		functionArgs.Result = propertyInfo.GetValue(value);
	}
}