namespace PanoramicData.NCalcExtensions.Extensions;

internal static class Try
{
	internal static void Evaluate(FunctionArgs functionArgs)
	{
		ExtendedExpression.CheckParameterCount(nameof(Try), functionArgs, 1, 2);

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
