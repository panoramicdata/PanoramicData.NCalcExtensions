namespace PanoramicData.NCalcExtensions.Test;

public class BangTests : NCalcTest
{
	[Fact]
	public void Bang_Succeeds()
	{
		const string expression = "!(1 == 2)";
		var e = new ExtendedExpression(expression);
		var result = e.Evaluate();
		result.Should().Be(true);
	}
}
