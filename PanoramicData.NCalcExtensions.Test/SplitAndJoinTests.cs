namespace PanoramicData.NCalcExtensions.Test;

public class SplitAndJoinTests : NCalcTest
{
	[Theory]
	[InlineData("split()")]
	[InlineData("split('a b c')")]
	public void Switch_InsufficientParameters_ThrowsException(string expression)
		=> Assert.Throws<FormatException>(() =>
		{
			var e = new ExtendedExpression(expression);
			e.Evaluate();
		});

	[Theory]
	[InlineData("join()")]
	[InlineData("join(1)")]
	public void Join_InsufficientParameters_ThrowsException(string expression)
		=> Assert.Throws<FormatException>(() =>
		{
			var e = new ExtendedExpression(expression);
			e.Evaluate();
		});

	[Theory]
	[InlineData("join(split('a b c', ' '), ',')", "a,b,c")]
	[InlineData("join(split('a b c', ' '), ', ')", "a, b, c")]
	public void Switch_ReturnsExpected(string expression, object? expectedOutput)
		=> Assert.Equal(expectedOutput, new ExtendedExpression(expression).Evaluate());
}
