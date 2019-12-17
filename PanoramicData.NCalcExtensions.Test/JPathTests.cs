using FluentAssertions;
using Newtonsoft.Json.Linq;
using PanoramicData.NCalcExtensions.Exceptions;
using System;
using Xunit;

namespace PanoramicData.NCalcExtensions.Test
{
	public class JPathTests : NCalcTest
	{
		[Theory]
		[InlineData("name", "bob")]
		[InlineData("numbers[0]", 1)]
		[InlineData("numbers[1]", 2)]
		[InlineData("kvps[?(@.key==\\'key1\\')].value", "value1")]
		[InlineData("kvps[?(@.key==\\'key2\\')].value", "value2")]
		public void JPath_ValidPath_Works(string path, object expectedResult)
		{
			var expression = new ExtendedExpression($"jPath(source, '{path}')");
			expression.Parameters["source"] = JObject.Parse("{ \"name\": \"bob\", \"numbers\": [1, 2], \"kvps\": [ { \"key\": \"key1\", \"value\": \"value1\" }, { \"key\": \"key2\", \"value\": \"value2\" } ] }");
			var result = expression.Evaluate();
			result.Should().Be(
				expectedResult,
				because: "the jPath is valid, present and returns a single result"
			);
		}

		[Fact]
		public void JPath_SourceObjectIsAString_NotAJObject()
		{
			var expression = new ExtendedExpression("jPath(source, 'name')");
			expression.Parameters["source"] = "SomeRandomString";
			var exception = Assert.Throws<FormatException>(() => expression.Evaluate());
			exception.Message.Should().Be(
				"jPath function - first parameter should be a JObject and second a string jPath expression with optional third parameter returnNullIfNotFound.",
				because: "the source is not a JObject"
			);
		}

		[Fact]
		public void JPath_PathNotPresent_PathError()
		{
			var expression = new ExtendedExpression("jPath(source, 'size')");
			expression.Parameters["source"] = JObject.Parse("{ \"name\": \"bob\", \"numbers\": [1, 2] }");
			var exception = Assert.Throws<NCalcExtensionsException>(() => expression.Evaluate());
			exception.Message.Should().Be(
				"jPath function - jPath expression did not result in a match.",
				because: "the selected item is an array"
			);
		}

		[Fact]
		public void JPath_PathNotPresent_PathError_ReturnsNull()
		{
			var expression = new ExtendedExpression("jPath(source, 'size', True)");
			expression.Parameters["source"] = JObject.Parse("{ \"name\": \"bob\", \"numbers\": [1, 2] }");
			var result = expression.Evaluate();
			result.Should().BeNull(
				because: "the requested jPath is not present and the third parameter 'returnNullIfNotFound' is set to True"
			);
		}

		[Fact]
		public void JPath_BadExpression_RequiresSingleValueError()
		{
			var expression = new ExtendedExpression("jPath(source, 'numbers')");
			expression.Parameters["source"] = JObject.Parse("{ \"name\": \"bob\", \"numbers\": [1, 2] }");
			var exception = Assert.Throws<FormatException>(() => expression.Evaluate());
			exception.Message.Should().Be(
				"jPath function - jPath expression should identify a single value in the source object. Result type found: Array",
				because: "the entry is an array of numbers, which is not a single result"
			);
		}
	}
}
