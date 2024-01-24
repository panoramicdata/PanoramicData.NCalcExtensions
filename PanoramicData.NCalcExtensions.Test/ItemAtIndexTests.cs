namespace PanoramicData.NCalcExtensions.Test;

public class ItemAtIndexTests : NCalcTest
{
	[Theory]
	[InlineData("itemAtIndex()")]
	[InlineData("itemAtIndex('a b c')")]
	[InlineData("itemAtIndex('a b c', null)")]
	[InlineData("itemAtIndex('a b c', 'xxx')")]
	public void ItemAtIndex_InsufficientParameters_ThrowsException(string expression)
		=> new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<FormatException>();

	[Theory]
	[InlineData("itemAtIndex(split('a b c', ' '), 1)", "b")]
	public void ItemAtIndex_ReturnsExpected(string expression, object? expectedOutput)
		=> new ExtendedExpression(expression).Evaluate().Should().Be(expectedOutput);
}