using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class SplitTests : NCalcTest
{
	[Theory]
	[InlineData("split()")]
	[InlineData("split('a b c')")]
	public void Split_InsufficientParameters_ThrowsException(string expression)
		=> new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<FormatException>();

	[Theory]
	[InlineData("split('a b c', '')")]
	[InlineData("split('x x x', '')")]
	public void Split_EmptySecondParameter_ThrowsException(string expression)
		=> new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<FormatException>();

	[Theory]
	[InlineData("split('a b c', ' ')")]
	[InlineData("split('aXXbXXc', 'XX')")]
	public void Split_ValidParameters_Succeeds(string expression)
	{
		var e = new ExtendedExpression(expression);
		var result = e.Evaluate();
		result.Should().BeOfType<List<string>>();
		var list = (List<string>)result;
		list.Should().HaveCount(3);
		list[0].Should().Be("a");
		list[1].Should().Be("b");
		list[2].Should().Be("c");
	}
}
