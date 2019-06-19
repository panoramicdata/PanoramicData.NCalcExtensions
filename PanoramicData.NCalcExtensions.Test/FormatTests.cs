using FluentAssertions;
using System;
using Xunit;

namespace PanoramicData.NCalcExtensions.Test
{
	public class FormatTests
	{
		[Fact]
		public void Format_InvalidFormat_Fails()
		{
			var expression = new ExtendedExpression("format(1, 1)");
			var exception = Assert.Throws<ArgumentException>(() => expression.Evaluate());
			exception.Message.Should().Be("Expected second argument to be a format string");
		}

		[Fact]
		public void Format_OneParameter_Fails()
		{
			var expression = new ExtendedExpression("format(1)");
			var exception = Assert.Throws<ArgumentException>(() => expression.Evaluate());
			exception.Message.Should().Be("Expected two arguments");
		}

		[Fact]
		public void Format_ZeroParameters_Fails()
		{
			var expression = new ExtendedExpression("format()");
			var exception = Assert.Throws<ArgumentException>(() => expression.Evaluate());
			exception.Message.Should().Be("Expected two arguments");
		}

		[Fact]
		public void Format_MoreThanTwoParameters_Fails()
		{
			var expression = new ExtendedExpression("format(1, '0', 'Third parameter')");
			var exception = Assert.Throws<ArgumentException>(() => expression.Evaluate());
			exception.Message.Should().Be("Expected two arguments");
		}

		[Fact]
		public void Format_IntFormat_Succeeds()
		{
			var expression = new ExtendedExpression("format(1, '00')");
			expression.Evaluate().Should().Be("01");
		}

		[Fact]
		public void Format_DoubleFormat_Succeeds()
		{
			var expression = new ExtendedExpression("format(1.0, '00')");
			expression.Evaluate().Should().Be("01");
		}

		[Fact]
		public void Format_StringFormat_Succeeds()
		{
			var expression = new ExtendedExpression("format('02', '0')");
			expression.Evaluate().Should().Be("2");
		}

		[Fact]
		public void Format_DateTimeFormat_Succeeds()
		{
			var expression = new ExtendedExpression("format('01/01/2019', 'yyyy-MM-dd')");
			expression.Evaluate().Should().Be("2019-01-01");
		}

		[Fact]
		public void Format_InvalidStringFormat_Succeeds()
		{
			var expression = new ExtendedExpression("format('XXX', 'yyyy-MM-dd')");
			var exception = Assert.Throws<FormatException>(() => expression.Evaluate());
			exception.Message.Should().Be("Could not parse 'XXX' as a number or date.");
		}
	}
}
