namespace PanoramicData.NCalcExtensions.Test;

public class FirstTests
{
	[Fact]
	public void First_Succeeds()
	{
		var expression = new ExtendedExpression("first(list(1, 5, 2, 3), 'n', 'n % 2 == 0')");
		var result = expression.Evaluate() as int?;

		result.Should().Be(2);
	}

	[Fact]
	public void FirstFromStored_Succeeds()
	{
		var expression = new ExtendedExpression("store('x', list(1, 5, 2, 3)) && first(retrieve('x'), 'n', 'n % 2 == 0') == 2");

		expression
			.Evaluate()
			.Should()
			.Be(true);
	}

	[Fact]
	public void First_NoMatchingItem_ThrowsException()
	{
		var expression = new ExtendedExpression("first(list(1, 5, 7, 3), 'n', 'n % 2 == 0')");

		expression
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();
	}
}
