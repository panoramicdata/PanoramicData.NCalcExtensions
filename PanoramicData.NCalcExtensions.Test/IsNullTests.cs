using Xunit;

namespace PanoramicData.NCalcExtensions.Test
{
	public class IsNullTests
	{
		[Fact]
		public void IsNull_Example1_Succeeds()
		{
			var expression = new ExtendedExpression("isNull(1)");
			Assert.False(expression.Evaluate() as bool?);
		}

		[Fact]
		public void IsNull_Example2_Succeeds()
		{
			var expression = new ExtendedExpression("isNull('text')");
			Assert.False(expression.Evaluate() as bool?);
		}

		[Fact]
		public void IsNull_Example3_Succeeds()
		{
			var expression = new ExtendedExpression("isNull(bob)");
			expression.Parameters["bob"] = null;
			Assert.True(expression.Evaluate() as bool?);
		}

		[Fact]
		public void IsNull_Example4_Succeeds()
		{
			var expression = new ExtendedExpression("isNull(null)");
			Assert.True(expression.Evaluate() as bool?);
		}
	}
}
