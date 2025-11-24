using System.Collections.Generic;

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

	[Theory]
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

	[Fact]
	public void ItemAtIndexWith2ListsAndMultiply_Succeeds()
	{
		var result = new ExtendedExpression("itemAtIndex(list(1,2,3,4),0) * itemAtIndex(list(1,2,3,4),1)")
			.Evaluate();

		result.Should().NotBeNull();
		result.Should().Be(2);
	}

	[Fact]
	public void ItemAtIndexSelectWithListTable_Succeeds()
	{
		var result = new ExtendedExpression("select(list(list(1,2),list(2,3),list(3,4)), 'X', 'itemAtIndex(X,0) * itemAtIndex(X,1)')")
			.Evaluate();

		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<int> { 2, 6, 12 });
	}

	[Fact]
	public void ItemAtIndexSelectWithJArrayTableAndCast_Succeeds()
	{
		var result = new ExtendedExpression(
				@$"select
				(
					jArray
					(
						jArray(1,2),
						jArray(2,3),
						jArray(3,4)
					),
					'X',
					'
						cast(itemAtIndex(X,0), \'System.Int32\') *
						cast(itemAtIndex(X,1), \'System.Int32\')
					'
				)")
			.Evaluate();

		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<int> { 2, 6, 12 });
	}

	[Fact]
	public void ItemAtIndexWithJArrayEmptyString_ReturnsString()
	{
		var expression = new ExtendedExpression("itemAtIndex(jArray('a', ''), 1)");
		expression.Evaluate().Should().BeOfType<string>();
	}

	[Fact]
	public void ItemAtIndexWithJArrayEmptyString_MatchesEmptyString()
	{
		var expression = new ExtendedExpression("itemAtIndex(jArray('a', ''), 1) == ''");
		expression.Evaluate().Should().Be(true);
	}
}