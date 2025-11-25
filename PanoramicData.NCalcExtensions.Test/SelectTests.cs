using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class SelectTests : NCalcTest
{
	[Fact]
	public void Select_Succeeds()
	{
		var result = new ExtendedExpression($"select(list(1, 2, 3, 4, 5), 'n', 'n + 1')")
			.Evaluate();

		// The result should be 2, 3, 4, 5, 6
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<int> { 2, 3, 4, 5, 6 });
	}

	[Fact]
	public void Select_IntoJObjects_Succeeds()
	{
		var result = new ExtendedExpression($"select(list(jObject('a', 1, 'b', '2'), jObject('a', 3, 'b', '4')), 'n', 'n', 'JObject')")
			.Evaluate();

		result.Should().NotBeNull();
		result.Should().BeOfType<List<JObject>>();
	}

	// Error cases
	[Fact]
	public void Select_NullFirstParameter_ThrowsException()
		=> new ExtendedExpression("select(null, 'n', 'n + 1')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*First*must be an IList*");

	[Fact]
	public void Select_NonListFirstParameter_ThrowsException()
		=> new ExtendedExpression("select(123, 'n', 'n + 1')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*First*must be an IList*");

	[Fact]
	public void Select_NullSecondParameter_ThrowsException()
		=> new ExtendedExpression("select(list(1,2,3), null, 'n + 1')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*Second*must be a string*");

	[Fact]
	public void Select_NullThirdParameter_ThrowsException()
		=> new ExtendedExpression("select(list(1,2,3), 'n', null)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*Third*must be a string*");

	[Fact]
	public void Select_InvalidFourthParameter_ThrowsException()
		=> new ExtendedExpression("select(list(1,2,3), 'n', 'n', 'InvalidType')")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*Fourth*must be either 'object'*or 'JObject'*");

	[Fact]
	public void Select_NonStringFourthParameter_ThrowsException()
		=> new ExtendedExpression("select(list(1,2,3), 'n', 'n', 123)")
			.Invoking(e => e.Evaluate())
			.Should()
			.Throw<FormatException>()
			.WithMessage("*Fourth*must be a string*");

	// Additional coverage tests
	[Fact]
	public void Select_EmptyList_ReturnsEmptyList()
	{
		var result = new ExtendedExpression("select(list(), 'n', 'n + 1')")
			.Evaluate();

		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<object>());
	}

	[Fact]
	public void Select_SingleElement_Works()
	{
		var result = new ExtendedExpression("select(list(5), 'n', 'n * 2')")
			.Evaluate();

		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<int> { 10 });
	}

	[Fact]
	public void Select_Strings_Works()
	{
		var result = new ExtendedExpression("select(list('a', 'b', 'c'), 's', 'toUpper(s)')")
			.Evaluate();

		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<string> { "A", "B", "C" });
	}

	[Fact]
	public void Select_WithNullInResult_Works()
	{
		var result = new ExtendedExpression("select(list(1, 2, 3), 'n', 'if(n == 2, null, n)')")
			.Evaluate();

		result.Should().NotBeNull();
		var list = result as List<object?>;
		list.Should().NotBeNull();
		list.Should().HaveCount(3);
	}

	[Fact]
	public void Select_JObject_WithNullValue_Works()
	{
		var result = new ExtendedExpression("select(list(1, 2, 3), 'n', 'if(n == 2, null, jObject(\"value\", n))', 'JObject')")
			.Evaluate();

		result.Should().NotBeNull();
		result.Should().BeOfType<List<JObject?>>();
		var list = result as List<JObject?>;
		list.Should().HaveCount(3);
		list![1].Should().BeNull();
	}

	[Fact]
	public void Select_JObject_FromNonJObjectValue_Works()
	{
		// When converting non-JObject values, they need to be wrapped in an object first
		var result = new ExtendedExpression("select(list(jObject('a', 1), jObject('a', 2)), 'obj', 'obj', 'JObject')")
			.Evaluate();

		result.Should().NotBeNull();
		result.Should().BeOfType<List<JObject?>>();
		var list = result as List<JObject?>;
		list.Should().HaveCount(2);
		list![0].Should().NotBeNull();
		list![0]!["a"]?.Value<int>().Should().Be(1);
	}

	[Fact]
	public void Select_JObject_ExplicitObjectType_Works()
	{
		var result = new ExtendedExpression("select(list(1, 2, 3), 'n', 'n * 2', 'object')")
			.Evaluate();

		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<int> { 2, 4, 6 });
	}

	[Fact]
	public void Select_ComplexTransformation_Works()
	{
		var result = new ExtendedExpression("select(list(1, 2, 3, 4, 5), 'n', 'if(n % 2 == 0, n * 10, n)')")
			.Evaluate();

		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<int> { 1, 20, 3, 40, 5 });
	}

	[Fact]
	public void Select_Doubles_Works()
	{
		var result = new ExtendedExpression("select(list(1.5, 2.5, 3.5), 'n', 'n * 2')")
			.Evaluate();

		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(new List<double> { 3.0, 5.0, 7.0 });
	}
}
