using System;
using Xunit;

namespace PanoramicData.NCalcExtensions.Test
{
	public class SwitchTests : NCalcTest
	{
		[Theory]
		[InlineData("switch()")]
		[InlineData("switch(1)")]
		[InlineData("switch(1, 2)")]
		public void Switch_InsufficientParameters_ThrowsException(string expression)
			=> Assert.Throws<FormatException>(() =>
			{
				var e = new ExtendedExpression(expression);
				e.Evaluate();
			});

		[Theory]
		[InlineData("switch('yes', 'yes', 1)", 1)]
		[InlineData("switch('yes', 'yes', 1, 'no', 2)", 1)]
		[InlineData("switch('yes', 'yes', '1', 'no', '2')", "1")]
		[InlineData("switch('no', 'yes', 1, 'no', 2)", 2)]
		[InlineData("switch('no', 'yes', '1', 'no', '2')", "2")]
		[InlineData("switch('blah', 'yes', 1, 'no', 2, 3)", 3)]
		[InlineData("switch('blah', 'yes', 1, 'no', 2, '3')", "3")]
		public void Switch_ReturnsExpected(string expression, object expectedOutput)
			=> Assert.Equal(expectedOutput, new ExtendedExpression(expression).Evaluate());

		[Theory]
		[InlineData("switch('blah', 'yes', 1, 'no', 2)")]
		public void Switch_MissingDefault_ThrowsException(string expression)
			=> Assert.Throws<FormatException>(() =>
			{
				var e = new ExtendedExpression(expression);
				e.Evaluate();
			});
	}
}
