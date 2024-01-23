namespace PanoramicData.NCalcExtensions.Test;

public class JoinTests : NCalcTest
{

	[Fact]
	public void Join_Simple_Succeeds()
	{
		var expression = new ExtendedExpression("join(list('a', 'b', 'c'), ', ')");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("a, b, c");
	}

	[Fact]
	public void Join_ContainingNulls_Succeeds()
	{
		var expression = new ExtendedExpression("join(list('a', null, 'c'), ',')");
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("a,,c");
	}

	[Fact]
	public void Join_Array_Succeeds()
	{
		var expression = new ExtendedExpression("join(array, ', ')");
		expression.Parameters["array"] = new[] { "a", "b", "c" };
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("a, b, c");
	}

	[Theory]
	[InlineData("join()")]
	[InlineData("join(1)")]
	public void Join_InsufficientParameters_ThrowsException(string expression)
=> Assert.Throws<FormatException>(() =>
{
	var e = new ExtendedExpression(expression);
	e.Evaluate();
});

	[Theory]
	[InlineData("join(split('a b c', ' '), ',')", "a,b,c")]
	[InlineData("join(split('a b c', ' '), ', ')", "a, b, c")]
	[InlineData("join(list('a', 'b', 'c'), ', ')", "a, b, c")]
	public void Switch_ReturnsExpected(string expression, object? expectedOutput)
		=> Assert.Equal(expectedOutput, new ExtendedExpression(expression).Evaluate());
}
