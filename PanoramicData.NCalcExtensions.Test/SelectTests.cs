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
}
