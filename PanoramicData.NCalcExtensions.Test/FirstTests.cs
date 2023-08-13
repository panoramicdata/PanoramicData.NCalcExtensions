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
	public void First_NoMatchingItem_ThrowsException()
	{
		var expression = new ExtendedExpression("first(list(1, 5, 7, 3), 'n', 'n % 2 == 0')");

		expression
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();
	}
}
