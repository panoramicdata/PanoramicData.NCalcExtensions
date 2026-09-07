namespace PanoramicData.NCalcExtensions.Test;

public class IfTests : NCalcTest
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
		=> Test("if(true, 'success', 'failure')").Should().Be("success");

	[Fact]
	public void If_FalseCondition_ReturnsFalseValue()
		=> Test("if(false, 'success', 'failure')").Should().Be("failure");

	[Fact]
	public void If_NumericComparison_ReturnsCorrectBranch()
		=> Test("if(5 > 3, 100, 200)").Should().Be(100);

	[Fact]
	public void If_WithNullTrueValue_ReturnsNull()
		=> Test("if(true, null, 'value')").Should().BeNull();

	[Fact]
	public void If_WithNullFalseValue_ReturnsNull()
		=> Test("if(false, 'value', null)").Should().BeNull();

	[Theory]
	[InlineData("if()")]
	[InlineData("if(true)")]
	[InlineData("if(true, 'value')")]
	[InlineData("if(true, 'value1', 'value2', 'value3')")]
	public void If_WrongParameterCount_ThrowsException(string expression) => new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*requires three parameters*");

	[Theory]
	[InlineData("if(1, 'true', 'false')")]
	[InlineData("if('text', 'true', 'false')")]
	[InlineData("if(null, 'true', 'false')")]
	public void If_NonBooleanCondition_ThrowsException(string expression) => new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*parameter 1*");

	[Fact]
	public void If_ComplexExpression_InTrueBranch_Evaluates()
		=> Test("if(true, 2 + 2, 3 + 3)").Should().Be(4);

	[Fact]
	public void If_ComplexExpression_InFalseBranch_Evaluates()
		=> Test("if(false, 2 + 2, 3 + 3)").Should().Be(6);

	[Fact]
	public void If_NestedIf_TrueBranch_EvaluatesCorrectly()
		=> Test("if(true, if(true, 'inner-true', 'inner-false'), 'outer-false')").Should().Be("inner-true");

	[Fact]
	public void If_NestedIf_FalseBranch_EvaluatesCorrectly()
		=> Test("if(false, 'outer-true', if(true, 'inner-true', 'inner-false'))").Should().Be("inner-true");

	[Fact]
	public void If_WithFunctionCall_InTrueBranch_Evaluates()
		=> Test("if(true, toUpper('hello'), toLower('WORLD'))").Should().Be("HELLO");

	[Fact]
	public void If_WithFunctionCall_InFalseBranch_Evaluates()
		=> Test("if(false, toUpper('hello'), toLower('WORLD'))").Should().Be("world");

	[Fact]
	public void If_WithVariable_InCondition_Evaluates()
	{
		var expression = new ExtendedExpression("if(x > 10, 'big', 'small')");
		expression.Parameters["x"] = 15;
		expression.Evaluate().Should().Be("big");
	}

	[Fact]
	public void If_WithListOperation_InTrueBranch_Evaluates()
		=> Test("if(true, length(list(1,2,3)), 0)").Should().Be(3);

	[Fact]
	public void If_ReturningDifferentTypes_HandlesCorrectly()
		=> Test("if(true, 123, 'string')").Should().Be(123);
}
