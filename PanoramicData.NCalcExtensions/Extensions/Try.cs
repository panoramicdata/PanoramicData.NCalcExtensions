using System.ComponentModel;

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
	internal static void Evaluate(IFunctionArgs functionArgs)
	{
		ExpressionHelper.CheckParameterCount(nameof(Try), functionArgs, 1, 2);

		try
		{
			functionArgs.Result = functionArgs.Parameters[0].Evaluate();
		}
		catch (Exception e)
		{
			if (functionArgs.Parameters.Length >= 2)
			{
				functionArgs.Parameters[1].Parameters["exception"] = e;
				functionArgs.Parameters[1].Parameters["exception_message"] = e.Message;
				functionArgs.Parameters[1].Parameters["exception_typeName"] = e.GetType().Name;
				functionArgs.Parameters[1].Parameters["exception_typeFullName"] = e.GetType().FullName;
				functionArgs.Parameters[1].Parameters["exception_type"] = e.GetType();
				functionArgs.Result = functionArgs.Parameters[1].Evaluate();
			}
			else
			{
				functionArgs.Result = null;
			}
		}
	}
}
