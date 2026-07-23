namespace PanoramicData.NCalcExtensions.Extensions;

/// <summary>
/// Used to provide IntelliSense in Monaco editor
/// </summary>
public partial interface IFunctionPrototypes
{
	[DisplayName("try")]
	[Description("If a function throws an exception, return an alternate value.")]
	object? TryFn(
		[Description("The expression / function to attempt.")]
		object expression,
		[Description("(Optional) The result to return if an exception is thrown (null is returned if this parameter is omitted and an exception is thrown).")]
		object? returnResult = null
	);
}

internal static class Try
{
	internal static void Evaluate(FunctionEventArgs functionArgs)
	{
		ExtendedExpression.CheckParameterCount(nameof(Try), functionArgs, 1, 2);

		try
		{
			functionArgs.Result = functionArgs.Parameters.Evaluate(0);
		}
		catch (Exception e)
		{
			if (functionArgs.Parameters.Count >= 2)
			{
				functionArgs.Context.StaticParameters["exception"] = e;
				functionArgs.Context.StaticParameters["exception_message"] = e.Message;
				functionArgs.Context.StaticParameters["exception_typeName"] = e.GetType().Name;
				functionArgs.Context.StaticParameters["exception_typeFullName"] = e.GetType().FullName;
				functionArgs.Context.StaticParameters["exception_type"] = e.GetType();
				functionArgs.Result = functionArgs.Parameters.Evaluate(1);
			}
			else
			{
				functionArgs.Result = null;
			}
		}
	}
}
