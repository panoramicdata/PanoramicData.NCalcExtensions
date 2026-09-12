namespace PanoramicData.NCalcExtensions.Test;

public abstract class NCalcTest
{
	protected static object? Test(string expressionText)
	{
		var expression = new ExtendedExpression(expressionText);
		return expression.Evaluate();
	}

	/// <summary>
	/// Evaluates <paramref name="expressionText"/> with one named parameter in scope.
	/// </summary>
	protected static object? Test(string expressionText, string parameterName, object? parameterValue)
	{
		var expression = new ExtendedExpression(expressionText);
		expression.Parameters[parameterName] = parameterValue;
		return expression.Evaluate();
	}

	/// <summary>
	/// Asserts that <paramref name="expressionText"/> fails with <typeparamref name="TException"/>,
	/// or a type derived from it, carrying a message matching <paramref name="messagePattern"/>.
	/// </summary>
	protected static void TestShouldThrow<TException>(string expressionText, string messagePattern)
		where TException : Exception
		=> new ExtendedExpression(expressionText)
			.Invoking(expression => expression.Evaluate())
			.Should().Throw<TException>()
			.WithMessage(messagePattern);

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
	{
		var expression = new ExtendedExpression(expressionText);
		expression.Parameters[parameterName] = parameterValue;

		expression.Invoking(x => x.Evaluate())
			.Should().Throw<TException>()
			.WithMessage(messagePattern);
	}

	/// <summary>
	/// Asserts that <paramref name="expressionText"/> fails with exactly
	/// <typeparamref name="TException"/>, carrying a message matching
	/// <paramref name="messagePattern"/>.
	/// </summary>
	protected static void TestShouldThrowExactly<TException>(string expressionText, string messagePattern)
		where TException : Exception
		=> new ExtendedExpression(expressionText)
			.Invoking(expression => expression.Evaluate())
			.Should().ThrowExactly<TException>()
			.WithMessage(messagePattern);

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
	{
		var expression = new ExtendedExpression(expressionText);
		expression.Parameters[parameterName] = parameterValue;

		expression.Invoking(x => x.Evaluate())
			.Should().ThrowExactly<TException>()
			.WithMessage(messagePattern);
	}
}
