using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class SelectTests : NCalcTest
{
	[Theory]
	[InlineData("select(list(1, 2, 3, 4, 5), 'n', 'n + 1')", new[] { 2, 3, 4, 5, 6 })]
	[InlineData("select(list(5), 'n', 'n * 2')", new[] { 10 })]
	[InlineData("select(list(1, 2, 3), 'n', 'n * 2', 'object')", new[] { 2, 4, 6 })]
	[InlineData("select(list(1, 2, 3, 4, 5), 'n', 'if(n % 2 == 0, n * 10, n)')", new[] { 1, 20, 3, 40, 5 })]
	public void Select_Integers_Succeeds(string expressionText, int[] expected)
		=> Test(expressionText).Should().BeEquivalentTo(expected.ToList());

	[Fact]
	public void Select_Doubles_Works()
		=> Test("select(list(1.5, 2.5, 3.5), 'n', 'n * 2')").Should().BeEquivalentTo(new List<double> { 3.0, 5.0, 7.0 });

	[Fact]
	public void Select_Strings_Works()
		=> Test("select(list('a', 'b', 'c'), 's', 'toUpper(s)')").Should().BeEquivalentTo(new List<string> { "A", "B", "C" });

	[Fact]
	public void Select_EmptyList_ReturnsEmptyList()
		=> Test("select(list(), 'n', 'n + 1')").Should().BeEquivalentTo(new List<object>());

	[Fact]
	public void Select_WithNullInResult_Works()
		=> Test("select(list(1, 2, 3), 'n', 'if(n == 2, null, n)')").Should().BeOfType<List<object?>>().Which.Should().HaveCount(3);

	[Fact]
	public void Select_IntoJObjects_Succeeds()
		=> Test("select(list(jObject('a', 1, 'b', '2'), jObject('a', 3, 'b', '4')), 'n', 'n', 'JObject')").Should().BeOfType<List<JObject>>();

	[Fact]
	public void Select_JObject_WithNullValue_Works()
	{
		var list = Test("select(list(1, 2, 3), 'n', 'if(n == 2, null, jObject(\"value\", n))', 'JObject')")
			.Should().BeOfType<List<JObject?>>().Subject;

		list.Should().HaveCount(3);
		list[1].Should().BeNull();
	}

	[Fact]
	public void Select_JObject_FromNonJObjectValue_Works()
	{
		var list = Test("select(list(jObject('a', 1), jObject('a', 2)), 'obj', 'obj', 'JObject')")
			.Should().BeOfType<List<JObject?>>().Subject;

		list.Should().HaveCount(2);
		list[0]!["a"]?.Value<int>().Should().Be(1);
	}

	[Theory]
	[InlineData("select(null, 'n', 'n + 1')", "*First*must be an IList*")]
	[InlineData("select(123, 'n', 'n + 1')", "*First*must be an IList*")]
	[InlineData("select(list(1,2,3), null, 'n + 1')", "*Second*must be a string*")]
	[InlineData("select(list(1,2,3), 'n', null)", "*Third*must be a string*")]
	[InlineData("select(list(1,2,3), 'n', 'n', 'InvalidType')", "*Fourth*must be either 'object'*or 'JObject'*")]
	[InlineData("select(list(1,2,3), 'n', 'n', 123)", "*Fourth*must be a string*")]
	public void Select_InvalidParameters_ThrowsException(string expressionText, string messagePattern)
		=> TestShouldThrow<FormatException>(expressionText, messagePattern);
}
