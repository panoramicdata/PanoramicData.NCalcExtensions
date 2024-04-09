using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class LastTests
{
	[Fact]
	public void Last_Succeeds()
	{
		var expression = new ExtendedExpression("last(list(1, 5, 2, 3, 4, 1), 'n', 'n % 2 == 0')");
		var result = expression.Evaluate() as int?;

		result.Should().Be(4);
	}

	[Fact]
	public void Last_SplittingString_Succeeds()
	{
		var expression = new ExtendedExpression("last(split('a b c', ' '))");
		var result = expression.Evaluate() as string;

		result.Should().Be("c");
	}

	[Fact]
	public void LastFromStored_Succeeds()
	{
		var expression = new ExtendedExpression("store('x', list(1, 5, 2, 3, 4, 1)) && last(retrieve('x'), 'n', 'n % 2 == 0') == 4");

		expression
			.Evaluate()
			.Should()
			.Be(true);
	}

	[Fact]
	public void Last_NoMatchingItem_ThrowsException()
	{
		var expression = new ExtendedExpression("last(list(1, 5, 7, 3), 'n', 'n % 2 == 0')");

		expression
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();
	}
}

public class ReverseTests
{
	[Fact]
	public void Reverse_Succeeds()
	{
		var expression = new ExtendedExpression("reverse(list(1, 2, 3, 4, 5, 5))");
		var result = expression.Evaluate() as List<object?>;

		result.Should().BeEquivalentTo(new List<object?> { 5, 5, 4, 3, 2, 1 });
	}

	[Fact]
	public void Reverse_InvalidType_ThrowsException()
	{
		var expression = new ExtendedExpression("reverse(1)");

		expression
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>();
	}
}