
namespace PanoramicData.NCalcExtensions;

public class DirectExpression
{
	public DirectExpression()
	{
		return;
	}

	internal ExtendedExpression getExtendedExpression(string expressionString)
	{
		ExtendedExpression expression = new ExtendedExpression(expressionString);

		if (expression.HasErrors())
		{
			throw new FormatException($"Could not evaluate expression: '{expressionString}' due to {expression.Error}.");
		}
        return expression;
	}

    internal object evaluate(ExtendedExpression expression)
    {
        return expression.Evaluate();
    }

    internal object evaluate(string expressionString)
    {
        ExtendedExpression expression = getExtendedExpression(expressionString);
        return evaluate(expression);
    }

    internal string getExpressionString(string function, string[] args)
    {
        string formattedArgs = String.Join(", ", args);

        return $"{function}({formattedArgs})";
    }

    public string Capitalize(string input)
    {
        string[] args = {input};
        object result = evaluate(getExpressionString("capitalize", args));
        return result.ToString();
    }

    public object Cast(object inputObject, Type desiredType)
    {
        string[] args = {inputObject.ToString(), $"'{desiredType}'"};
        object result = evaluate(getExpressionString("cast", args));
        return result;
    }

    public DateTime ChangeTimeZone(DateTime inputTime, string inputTimeZone, string outputTimeZone)
    {
        string[]args = {inputTime.ToString(), inputTimeZone, outputTimeZone};
        object result = evaluate(getExpressionString("changeTimeZone", args));
        return DateTime.Parse(result.ToString());
    }
}