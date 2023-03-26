using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class DistinctTests : NCalcTest
{
	[Fact]
	public void Distinct_Succeeds()
	{
		var result = new ExtendedExpression($"distinct(list(1, 2, 3, 3, 3))")
			.Evaluate();

		// The result should be 1, 2, 3
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<int> { 1, 2, 3 });
	}
}
