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
	public void Join_Array_Succeeds()
	{
		var expression = new ExtendedExpression("join(array, ', ')");
		expression.Parameters["array"] = new[] { "a", "b", "c" };
		var result = expression.Evaluate();
		result.Should().BeOfType<string>();
		result.Should().Be("a, b, c");
	}
}
