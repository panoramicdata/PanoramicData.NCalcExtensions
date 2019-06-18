using Xunit;

namespace PanoramicData.NCalcExtensions.Test
{
	public class SubstringTests
	{
		[Fact]
		public void Substring_TwoParameters_Succeeds()
		{
			var expression = new ExtendedExpression("substring('ABC', 1)");
			Assert.Equal("BC", expression.Evaluate() as string);
		}

		[Fact]
		public void Substring_ThreeParameters_Succeeds()
		{
			var expression = new ExtendedExpression("substring('ABC', 1, 1)");
			Assert.Equal("B", expression.Evaluate() as string);
		}
	}
}
