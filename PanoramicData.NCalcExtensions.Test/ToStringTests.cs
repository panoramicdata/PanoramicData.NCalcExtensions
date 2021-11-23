namespace PanoramicData.NCalcExtensions.Test;

public class ToStringTests
{
	[Fact]
	public void ToString_IsNull_ReturnsNull()
	{
		var expression = new ExtendedExpression("toString(null)");
		Assert.Null(expression.Evaluate());
	}
	[Fact]
	public void ToString_Int_Succeeds()
	{
		var expression = new ExtendedExpression("toString(1)");
		Assert.Equal("1", expression.Evaluate());
	}
}
