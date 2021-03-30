using Xunit;

namespace PanoramicData.NCalcExtensions.Test
{
	public class TypeOfTests
	{
		[Theory]
		[InlineData("String", "'text'")]
		[InlineData("Int32", "1")]
		[InlineData("Double", "1.1")]
		[InlineData(null, "null")]
		public void TypeOf_ReturnsExpected(string? expected, string input)
		{
			var expression = new ExtendedExpression($"typeOf({input})");
			Assert.Equal(expected, expression.Evaluate());
		}
	}
}
