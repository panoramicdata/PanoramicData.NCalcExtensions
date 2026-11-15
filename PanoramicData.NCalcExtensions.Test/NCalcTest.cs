namespace PanoramicData.NCalcExtensions.Test;

public abstract class NCalcTest
{
	protected static object? Test(string expressionText)
		=> new ExtendedExpression(expressionText).Evaluate();

	/// <summary>
	/// Evaluates <paramref name="expressionText"/> with one named parameter in scope.
	/// </summary>
	protected static object? Test(string expressionText, string parameterName, object? parameterValue)
		=> WithParameter(expressionText, parameterName, parameterValue).Evaluate();

	/// <summary>
	/// Asserts that <paramref name="expressionText"/> fails with <typeparamref name="TException"/>,
	/// or a type derived from it, carrying a message matching <paramref name="messagePattern"/>.
	/// </summary>
	protected static void TestShouldThrow<TException>(string expressionText, string messagePattern)
		where TException : Exception
		=> AssertThrows<TException>(new ExtendedExpression(expressionText), messagePattern, exactly: false);

	/// <summary>
	/// As <see cref="TestShouldThrow{TException}(string, string)"/>, with one named parameter in
	/// scope.
	/// </summary>
	protected static void TestShouldThrow<TException>(
		string expressionText,
		string parameterName,
		object? parameterValue,
		string messagePattern)
		where TException : Exception
		=> AssertThrows<TException>(WithParameter(expressionText, parameterName, parameterValue), messagePattern, exactly: false);

	/// <summary>
	/// Asserts that <paramref name="expressionText"/> fails with exactly
	/// <typeparamref name="TException"/>, carrying a message matching
	/// <paramref name="messagePattern"/>.
	/// </summary>
	protected static void TestShouldThrowExactly<TException>(string expressionText, string messagePattern)
		where TException : Exception
		=> AssertThrows<TException>(new ExtendedExpression(expressionText), messagePattern, exactly: true);

	/// <summary>
	/// As <see cref="TestShouldThrowExactly{TException}(string, string)"/>, with one named
	/// parameter in scope.
	/// </summary>
	protected static void TestShouldThrowExactly<TException>(
		string expressionText,
		string parameterName,
		object? parameterValue,
		string messagePattern)
		where TException : Exception
		=> AssertThrows<TException>(WithParameter(expressionText, parameterName, parameterValue), messagePattern, exactly: true);

	private static ExtendedExpression WithParameter(string expressionText, string parameterName, object? parameterValue)
	{
		var expression = new ExtendedExpression(expressionText);
		expression.Parameters[parameterName] = parameterValue;
		return expression;
	}

	private static void AssertThrows<TException>(ExtendedExpression expression, string messagePattern, bool exactly)
		where TException : Exception
	{
		var assertion = expression.Invoking(x => x.Evaluate()).Should();

		if (exactly)
		{
			assertion.ThrowExactly<TException>().WithMessage(messagePattern);
		}
		else
		{
			assertion.Throw<TException>().WithMessage(messagePattern);
		}
	}
}
