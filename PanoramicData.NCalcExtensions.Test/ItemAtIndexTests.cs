namespace PanoramicData.NCalcExtensions.Test;

public class ItemAtIndexTests : NCalcTest
{
	[Theory]
	[InlineData("itemAtIndex()")]
	[InlineData("itemAtIndex('a b c')")]
	[InlineData("itemAtIndex('a b c', null)")]
	[InlineData("itemAtIndex('a b c', 'xxx')")]
	public void ItemAtIndex_InsufficientParameters_ThrowsException(string expression)
		=> Assert.Throws<FormatException>(() =>
		{
			var e = new ExtendedExpression(expression);
			e.Evaluate();
		});

	[Theory]
	[InlineData("itemAtIndex(split('a b c', ' '), 1)", "b")]
	public void ItemAtIndex_ReturnsExpected(string expression, object? expectedOutput)
		=> Assert.Equal(expectedOutput, new ExtendedExpression(expression).Evaluate());
}