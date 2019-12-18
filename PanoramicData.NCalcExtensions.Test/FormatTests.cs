using FluentAssertions;
using System;
using System.Globalization;
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
			exception.Message.Should().Be("format function - expected second argument to be a format string");
		}

		[Theory]
		[InlineData("format()")]
		[InlineData("format(1)")]
		[InlineData("format(1, 2, 3)")]
		public void Format_NotTwoParameters_Fails(string inputText)
		{
			var expression = new ExtendedExpression(inputText);
			var exception = Assert.Throws<ArgumentException>(() => expression.Evaluate());
			exception.Message.Should().Be("format function - expected two arguments");
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
		public void Format_DateTimeFormat_Succeeds()
		{
			var expression = new ExtendedExpression("format(dateTime('UTC', 'yyyy-MM-dd'), 'yyyy-MM-dd')");
			expression.Evaluate().Should().Be(DateTime.UtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
		}

		[Fact]
		public void Format_StringFormat_Succeeds()
		{
			var expression = new ExtendedExpression("format('02', '0')");
			expression.Evaluate().Should().Be("2");
		}

		[Fact]
		public void Format_DateTimeStringFormat_Succeeds()
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
