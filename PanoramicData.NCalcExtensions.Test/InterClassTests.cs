using FluentAssertions;
using Xunit;

namespace PanoramicData.NCalcExtensions.Test
{
	public class InterClassTests
	{
		[Fact]
		public void AddingAnIntStringToAnInt_YieldsInt()
		{
			var expression = new ExtendedExpression("0 + '1'");
			expression.Evaluate().Should().BeOfType<decimal>();
			expression.Evaluate().Should().Be(1);
			expression.Evaluate().Should().NotBe("1");
		}

		[Fact]
		public void AddingAnIntToAnIntString_YieldsString()
		{
			var expression = new ExtendedExpression("'1' + 0");
			expression.Evaluate().Should().BeOfType<string>();
			expression.Evaluate().Should().Be("10");
			expression.Evaluate().Should().NotBe(10);
		}
	}
}
