using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class LambdaTests
{
	[Theory]
	[InlineData("n * 2", 5, 10)]
	[InlineData("n * 3", 2, 6)]
	[InlineData("n * 3", 5, 15)]
	[InlineData("n * 3", 10, 30)]
	[InlineData("n + 1", 5, 6)]
	[InlineData("n + n", 7, 14)]
	[InlineData("n * n + 2 * n + 1", 3, 16)] // 9 + 6 + 1
	[InlineData("n * n + 2 * n + 1", 5, 36)] // 25 + 10 + 1
	public void Lambda_Evaluate_MathExpressions_Works(string lambdaExpression, int input, int expected)
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("n", lambdaExpression, parameters);
		lambda.Evaluate(input).Should().Be(expected);
	}

	[Fact]
	public void Lambda_Evaluate_StringExpression_Works()
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("s", "toUpper(s)", parameters);
		
		var result = lambda.Evaluate("hello");
		result.Should().Be("HELLO");
	}

	[Theory]
	[InlineData(10, true)]
	[InlineData(3, false)]
	public void Lambda_Evaluate_BooleanExpression_Works(int input, bool expected)
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("n", "n > 5", parameters);
		
		var result = lambda.Evaluate(input);
		result.Should().Be(expected);
	}

	[Fact]
	public void Lambda_Evaluate_WithExistingParameters_Works()
	{
		var parameters = new Dictionary<string, object?>
		{
			["threshold"] = 100
		};
		var lambda = new Lambda("n", "n > threshold", parameters);
		
		var result = lambda.Evaluate(150);
		result.Should().Be(true);
	}

	[Fact]
	public void Lambda_Evaluate_ReplacesExistingPredicateValue()
	{
		var parameters = new Dictionary<string, object?>
		{
			["n"] = 999 // Should be replaced
		};
		var lambda = new Lambda("n", "n + 1", parameters);
		
		var result = lambda.Evaluate(5);
		result.Should().Be(6);
	}

	[Theory]
	[InlineData(4, "even")]
	[InlineData(7, "odd")]
	public void Lambda_Evaluate_ComplexExpression_Works(int input, string expected)
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("n", "if(n % 2 == 0, 'even', 'odd')", parameters);
		
		lambda.Evaluate(input).Should().Be(expected);
	}

	[Theory]
	[InlineData(5, 10)]
	[InlineData(42, 84)]
	public void Lambda_EvaluateTo_ReturnsCorrectType(int input, int expected)
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("n", "n * 2", parameters);
		
		var result = lambda.EvaluateTo<int, int>(input);
		result.Should().Be(expected);
	}

	[Fact]
	public void Lambda_EvaluateTo_String_Works()
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("n", "toString(n, '0')", parameters);
		
		var result = lambda.EvaluateTo<int, string>(42);
		result.Should().Be("42");
	}

	[Theory]
	[InlineData(15, true)]
	[InlineData(5, false)]
	public void Lambda_EvaluateTo_Boolean_Works(int input, bool expected)
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("n", "n > 10", parameters);
		
		var result = lambda.EvaluateTo<int, bool>(input);
		result.Should().Be(expected);
	}

	[Fact]
	public void Lambda_EvaluateTo_Double_Works()
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("n", "n / 2.0", parameters);
		
		var result = lambda.EvaluateTo<int, double>(10);
		result.Should().Be(5.0);
	}

	[Theory]
	[InlineData(5, 10L)]
	[InlineData(42, 84L)]
	public void Lambda_EvaluateTo_ConvertibleTypes_Works(int input, long expected)
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("n", "n * 2", parameters);
		
		var result = lambda.EvaluateTo<int, long>(input);
		result.Should().Be(expected);
	}

	[Fact]
	public void Lambda_EvaluateTo_IntToDouble_Converts()
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("n", "n", parameters);
		
		var result = lambda.EvaluateTo<int, double>(42);
		result.Should().Be(42.0);
	}

	[Fact]
	public void Lambda_Evaluate_WithNull_Works()
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("n", "isNull(n)", parameters);
		
		var result = lambda.Evaluate<object?>(null);
		result.Should().Be(true);
	}

	[Fact]
	public void Lambda_Evaluate_ListOperations_Works()
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("n", "count(list(1, 2, n))", parameters);
		
		var result = lambda.Evaluate(3);
		result.Should().Be(3);
	}

	[Fact]
	public void Lambda_EvaluateTo_Decimal_Works()
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("n", "n * 1.5", parameters);
		
		var result = lambda.EvaluateTo<int, decimal>(10);
		result.Should().Be(15m);
	}

	[Fact]
	public void Lambda_Evaluate_WithMultipleParameters_Works()
	{
		var parameters = new Dictionary<string, object?>
		{
			["multiplier"] = 3,
			["offset"] = 10
		};
		var lambda = new Lambda("n", "n * multiplier + offset", parameters);
		
		var result = lambda.Evaluate(5);
		result.Should().Be(25); // 5 * 3 + 10
	}

	[Fact]
	public void Lambda_EvaluateTo_Float_Works()
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("n", "n / 2.0", parameters);
		
		var result = lambda.EvaluateTo<int, float>(10);
		result.Should().BeApproximately(5.0f, 0.001f);
	}

	[Fact]
	public void Lambda_Evaluate_StringConcatenation_Works()
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("s", "s + ' World'", parameters);
		
		var result = lambda.Evaluate("Hello");
		result.Should().Be("Hello World");
	}

	[Theory]
	[InlineData(true, "yes")]
	[InlineData(false, "no")]
	public void Lambda_EvaluateTo_BoolToString_Works(bool input, string expected)
	{
		var parameters = new Dictionary<string, object?>();
		var lambda = new Lambda("b", "if(b, 'yes', 'no')", parameters);
		
		var result = lambda.EvaluateTo<bool, string>(input);
		result.Should().Be(expected);
	}
}
