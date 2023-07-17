namespace PanoramicData.NCalcExtensions.Test;
public class CastTests
{
	[Theory]
	[InlineData("1", "System.Int32", 1)]
	[InlineData("1", "System.Int64", 1L)]
	[InlineData("1", "System.Double", 1.0)]
	[InlineData("1", "System.Decimal", 1.0)]
	[InlineData("1", "System.String", "1")]
	[InlineData("1", "System.Boolean", true)]
	public void Cast_Succeeds(string input,string type, object expected)
		=> new ExtendedExpression($"cast({input},'{type}')")
		.Evaluate()
		.Should()
		.Be(expected);
}
