namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("nullCoalesce")]
	[Description("Returns the first parameter that is not null, otherwise: null.")]
	object? NullCoalesce(
		[Description("Any number of objects.")]
		params object[] items
	);
}

internal static class NullCoalesce
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		for (var parameterIndex = 0; parameterIndex < functionArgs.Parameters.Count; parameterIndex++)
		{
			var result = functionArgs.Parameters.Evaluate(parameterIndex);
			if (result is not null)
			{
				functionArgs.Result = result;
				return;
			}
		}

		functionArgs.Result = null;
	}
}
