using FluentAssertions;
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
		[InlineData("switch(1, 1, 'one', 2, 'two')", "one")]
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

		[Fact]
		public void Switch_ComparingIntegers_Works()
		{
			const string expression = "switch(incident_Priority, 4, 4, 1, 1, 21)";
			var e = new ExtendedExpression(expression);
			e.Parameters["incident_Priority"] = 1;
			var result = e.Evaluate();
			result.Should().Be(1);
		}
	}
}
