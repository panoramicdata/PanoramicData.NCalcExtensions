namespace PanoramicData.NCalcExtensions.Test;

public class IfTests
{
	[Theory]
	[InlineData("1 == 1", "yes", "no", "yes")]
	[InlineData("1 == 2", "yes", "no", "no")]
	public void If_InlineData_ResultsMatchExpectation(string expressionText, string trueValue, string falseValue, object expected)
	{
		var expression = new ExtendedExpression($"if({expressionText},'{trueValue}','{falseValue}')");
		var result = expression.Evaluate();
		result.Should().Be(expected);
	}

	// New comprehensive tests

	[Fact]
	public void If_TrueCondition_ReturnsTrueValue()
	{
		var expression = new ExtendedExpression("if(true, 'success', 'failure')");
		expression.Evaluate().Should().Be("success");
	}

	[Fact]
	public void If_FalseCondition_ReturnsFalseValue()
	{
		var expression = new ExtendedExpression("if(false, 'success', 'failure')");
		expression.Evaluate().Should().Be("failure");
	}

	[Fact]
	public void If_NumericComparison_ReturnsCorrectBranch()
	{
		var expression = new ExtendedExpression("if(5 > 3, 100, 200)");
		expression.Evaluate().Should().Be(100);
	}

	[Fact]
	public void If_WithNullTrueValue_ReturnsNull()
	{
		var expression = new ExtendedExpression("if(true, null, 'value')");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void If_WithNullFalseValue_ReturnsNull()
	{
		var expression = new ExtendedExpression("if(false, 'value', null)");
		expression.Evaluate().Should().BeNull();
	}

	[Theory]
	[InlineData("if()")]
	[InlineData("if(true)")]
	[InlineData("if(true, 'value')")]
	[InlineData("if(true, 'value1', 'value2', 'value3')")]
	public void If_WrongParameterCount_ThrowsException(string expression)
	{
		new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*requires three parameters*");
	}

	[Theory]
	[InlineData("if(1, 'true', 'false')")]
	[InlineData("if('text', 'true', 'false')")]
	[InlineData("if(null, 'true', 'false')")]
	public void If_NonBooleanCondition_ThrowsException(string expression)
	{
		new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*parameter 1*");
	}

	[Fact]
	public void If_ComplexExpression_InTrueBranch_Evaluates()
	{
		var expression = new ExtendedExpression("if(true, 2 + 2, 3 + 3)");
		expression.Evaluate().Should().Be(4);
	}

	[Fact]
	public void If_ComplexExpression_InFalseBranch_Evaluates()
	{
		var expression = new ExtendedExpression("if(false, 2 + 2, 3 + 3)");
		expression.Evaluate().Should().Be(6);
	}

	[Fact]
	public void If_NestedIf_TrueBranch_EvaluatesCorrectly()
	{
		var expression = new ExtendedExpression("if(true, if(true, 'inner-true', 'inner-false'), 'outer-false')");
		expression.Evaluate().Should().Be("inner-true");
	}

	[Fact]
	public void If_NestedIf_FalseBranch_EvaluatesCorrectly()
	{
		var expression = new ExtendedExpression("if(false, 'outer-true', if(true, 'inner-true', 'inner-false'))");
		expression.Evaluate().Should().Be("inner-true");
	}

	[Fact]
	public void If_WithFunctionCall_InTrueBranch_Evaluates()
	{
		var expression = new ExtendedExpression("if(true, toUpper('hello'), toLower('WORLD'))");
		expression.Evaluate().Should().Be("HELLO");
	}

	[Fact]
	public void If_WithFunctionCall_InFalseBranch_Evaluates()
	{
		var expression = new ExtendedExpression("if(false, toUpper('hello'), toLower('WORLD'))");
		expression.Evaluate().Should().Be("world");
	}

	[Fact]
	public void If_WithVariable_InCondition_Evaluates()
	{
		var expression = new ExtendedExpression("if(x > 10, 'big', 'small')");
		expression.Parameters["x"] = 15;
		expression.Evaluate().Should().Be("big");
	}

	[Fact]
	public void If_WithListOperation_InTrueBranch_Evaluates()
	{
		var expression = new ExtendedExpression("if(true, length(list(1,2,3)), 0)");
		expression.Evaluate().Should().Be(3);
	}

	[Fact]
	public void If_ReturningDifferentTypes_HandlesCorrectly()
	{
		var expression = new ExtendedExpression("if(true, 123, 'string')");
		expression.Evaluate().Should().Be(123);
	}
}
