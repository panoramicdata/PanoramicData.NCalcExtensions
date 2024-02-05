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
}
public class LastOrDefaultTests
{
	[Fact]
	public void LastOrDefault_MatchingItem_Succeeds()
	{
		var expression = new ExtendedExpression("lastOrDefault(list(1, 5, 2, 3, 4, 1), 'n', 'n % 2 == 0')");
		var result = expression.Evaluate() as int?;

		result.Should().Be(4);
	}

	[Fact]
	public void FirstOrDefault_NoMatchingItem_Succeeds()
	{
		var expression = new ExtendedExpression("lastOrDefault(list(1, 5, 7, 3), 'n', 'n % 2 == 0')");
		var result = expression.Evaluate() as int?;

		result.Should().BeNull();
	}
}