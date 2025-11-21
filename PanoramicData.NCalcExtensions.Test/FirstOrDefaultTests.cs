namespace PanoramicData.NCalcExtensions.Test;

public class FirstOrDefaultTests
{
	[Fact]
	public void FirstOrDefault_MatchingItem_Succeeds()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(1, 5, 2, 3), 'n', 'n % 2 == 0')");
		var result = expression.Evaluate() as int?;

		result.Should().Be(2);
	}

	[Fact]
	public void FirstOrDefault_NoMatchingItem_Succeeds()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(1, 5, 7, 3), 'n', 'n % 2 == 0')");
		var result = expression.Evaluate() as int?;

		result.Should().BeNull();
	}

	[Fact]
	public void FirstOrDefault_OneParameter_ReturnsFirstElement()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(1, 2, 3))");
		expression.Evaluate().Should().Be(1);
	}

	[Fact]
	public void FirstOrDefault_OneParameter_EmptyList_ReturnsNull()
	{
		var expression = new ExtendedExpression("firstOrDefault(list())");
		expression.Evaluate().Should().BeNull();
	}

	[Fact]
	public void FirstOrDefault_NullFirstParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("firstOrDefault(null, 'n', 'n > 0')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*First*must be an IEnumerable*");
	}

	[Fact]
	public void FirstOrDefault_NullSecondParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(1,2,3), null, 'n > 0')");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*Second*must be a string*");
	}

	[Fact]
	public void FirstOrDefault_NullThirdParameter_ThrowsException()
	{
		var expression = new ExtendedExpression("firstOrDefault(list(1,2,3), 'n', null)");
		expression.Invoking(e => e.Evaluate()).Should().ThrowExactly<FormatException>()
			.WithMessage("*Third*must be a string*");
	}
}
