namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("typeOf")]
	[Description("Determines the C# type of the object.")]
	string TypeOfFn(
		[Description("The value whose data type is to be returned.")]
		object value
	);
}

internal static class TypeOf
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		var parameter1 = functionArgs.Parameters.Length == 1
			 ? functionArgs.Parameters[0].Evaluate()
			 : throw new FormatException($"{ExtensionFunction.TypeOf} function - requires one parameter.");

		functionArgs.Result = parameter1 != null ? parameter1.GetType().Name : (object?)null;
	}
}
