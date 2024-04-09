namespace PanoramicData.NCalcExtensions.Test;

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
	public void LastOrDefault_NoMatchingStringItem_Succeeds()
	{
		var expression = new ExtendedExpression("lastOrDefault(split('', ' '))");
		var result = expression.Evaluate() as string;

		result.Should().Be(string.Empty);
	}

	[Fact]
	public void LastOrDefault_MatchingStringItem_Succeeds()
	{
		var expression = new ExtendedExpression("lastOrDefault(split('a b c', ' '))");
		var result = expression.Evaluate() as string;

		result.Should().Be("c");
	}

	[Fact]
	public void FirstOrDefault_NoMatchingItem_Succeeds()
	{
		var expression = new ExtendedExpression("lastOrDefault(list(1, 5, 7, 3), 'n', 'n % 2 == 0')");
		var result = expression.Evaluate() as int?;

		result.Should().BeNull();
	}
}