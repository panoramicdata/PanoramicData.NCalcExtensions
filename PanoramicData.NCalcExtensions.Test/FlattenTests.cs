using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class FlattenTests
{
	[Fact]
	public void Flatten_ListOfLists_ReturnsFlatList()
	{
		var expression = new ExtendedExpression("flatten(x)");
		expression.Parameters.Add("x", new List<object?> {
			new List<object?> { 1, 2 },
			new List<object?> { 3, 4 }
		});
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<object?> { 1, 2, 3, 4 });
	}

	[Fact]
	public void Flatten_EmptyOuterList_ReturnsEmptyList()
	{
		var expression = new ExtendedExpression("flatten(x)");
		expression.Parameters.Add("x", new List<object?>());
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEmpty();
	}

	[Fact]
	public void Flatten_SomeEmptyInnerLists_SkipsEmpty()
	{
		var expression = new ExtendedExpression("flatten(x)");
		expression.Parameters.Add("x", new List<object?> {
			new List<object?> { 1, 2 },
			new List<object?>(),
			new List<object?> { 3 }
		});
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<object?> { 1, 2, 3 });
	}

	[Fact]
	public void Flatten_ListWithNonListItems_IncludesThemAsIs()
	{
		var expression = new ExtendedExpression("flatten(x)");
		expression.Parameters.Add("x", new List<object?> {
			new List<object?> { 1, 2 },
			42
		});
		var result = expression.Evaluate() as List<object?>;
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<object?> { 1, 2, 42 });
	}

	[Fact]
	public void Flatten_NullParameter_ThrowsException()
		=> new ExtendedExpression("flatten(null)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*flatten*cannot be null*");
}
