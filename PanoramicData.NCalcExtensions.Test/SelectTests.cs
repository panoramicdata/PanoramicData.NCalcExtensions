using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class SelectTests : NCalcTest
{
	[Fact]
	public void Select_Succeeds()
	{
		var result = new ExtendedExpression($"select(list(1, 2, 3, 4, 5), 'n', 'n + 1')")
			.Evaluate();

		// The result should be 2, 3, 4, 5, 6
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<int> { 2, 3, 4, 5, 6 });
	}

	[Fact]
	public void Select_IntoJObjects_Succeeds()
	{
		var result = new ExtendedExpression($"select(list(jObject('a', 1, 'b', '2'), jObject('a', 3, 'b', '4')), 'n', 'n', 'JObject')")
			.Evaluate();

		result.Should().NotBeNull();
		result.Should().BeOfType<List<JObject>>();
	}

	[Fact]
	public void SelectWithListOfLists_Succeeds()
	{
		var result = new ExtendedExpression("select(list(list(1,2),list(2,3),list(3,4)), 'X', 'itemAtIndex(X,0) * itemAtIndex(X,1)')")
			.Evaluate();

		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<int> { 2, 6, 12 });
	}
}
