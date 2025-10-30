namespace PanoramicData.NCalcExtensions.Test;

public class EqualityTests
{
	private const string x = "a";
	private const string xDotY = "b";

	/// <summary>
	/// Issue is with the NCalc library itself
	/// Related:
	/// https://github.com/ncalc/ncalc/pull/489
	/// https://github.com/ncalc/ncalc/issues/471
	/// </summary>
	/// <param name="trueExpressionString"></param>
	[Theory]
	[InlineData("1 != ''")]
	[InlineData("'a' == 'a'")]
	[InlineData("x == 'a'")]
	[InlineData("[x.y] == 'b'")]
	[InlineData("[x.y] != 'a'")]
	[InlineData("[x.y] != x")]
	[InlineData("!isNull([x.y])")]
	[InlineData("!([x.y] == nullThing)")]
	[InlineData("[x.y] != ''")]
	[InlineData("x != ''")]
	[InlineData("'' != [x.y]")]

	public void Equality_Succeeds(string trueExpressionString)
	{
		var extendedExpression = new ExtendedExpression(trueExpressionString);
		extendedExpression.Parameters["nullThing"] = null;
		extendedExpression.Parameters["x"] = x;
		extendedExpression.Parameters["x.y"] = xDotY;
		var result = extendedExpression.Evaluate();
		result.Should().Be(true);
	}
}