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

	[InlineData("itemAtIndex(list(1, 2, 3, 4, 5), 1)", 2)]
	public void ItemAtIndexWithListInts_ReturnsExpected(string expression, object? expectedOutput)
		=> new ExtendedExpression(expression).Evaluate().Should().Be(expectedOutput);

	[Theory]
	[InlineData("itemAtIndex(list(1, 2, 3, 4), cast(1, 'System.Int64'))", 2)]
	public void ItemAtIndexWithInt64IndexValue_ReturnsExpected(string expression, object? expectedOutput)
		=> new ExtendedExpression(expression).Evaluate().Should().Be(expectedOutput);

	[Theory]
	[InlineData($"itemAtIndex(list(1, 2, 3, 4), 2147483648)")]
	public void ItemAtIndexWithInt64IndexValueTooLarge_ThrowsException(string expression)
		=> new ExtendedExpression(expression)
			.Invoking(e => e.Evaluate())
			.Should()
			.ThrowExactly<FormatException>();
}