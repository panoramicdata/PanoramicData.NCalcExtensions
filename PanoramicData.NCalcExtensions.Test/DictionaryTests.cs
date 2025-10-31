using System.Collections.Generic;

namespace PanoramicData.NCalcExtensions.Test;

public class DictionaryTests
{
	[Fact]
	public void Dictionary_OddNumberOfParameters_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("dictionary('p1', true, 'p2')");

		var action = expression.Evaluate;
		action.Should().Throw<FormatException>();
	}

	[Fact]
	public void Dictionary_KeyParameterIsNotString_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("dictionary(0, true, 'p2', false)");

		var action = expression.Evaluate;
		action.Should().Throw<FormatException>();
	}

	[Fact]
	public void Dictionary_EvenNumberOfParameters_ReturnsDictionaryOfStringAndObject()
	{
		var expression = new ExtendedExpression("dictionary('p1', true, 'p2', false)");

		var result = expression.Evaluate();
		result.Should().BeOfType<Dictionary<string, object?>>();
	}

	[Fact]
	public void Dictionary_EvenNumberOfParameters_ReturnsDictionaryContainingCorrectEntries()
	{
		var expression = new ExtendedExpression("dictionary('p1', true, 'p2', false)");

		var result = expression.Evaluate() as Dictionary<string, object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(2);
		result["p1"].Should().Be(true);
		result["p2"].Should().Be(false);
	}

	[Fact]
	public void Dictionary_KeyParameterIsNull_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("dictionary(null, true, 'p2', false)");

		((Func<object?>?)expression.Evaluate)
			.Should()
			.Throw<FormatException>();
	}

	[Fact]
	public void Dictionary_NullValueParameter_GeneratesDictionaryContainingNullValue()
	{
		var expression = new ExtendedExpression("dictionary('p1', null)");

		var result = expression.Evaluate() as Dictionary<string, object?>;
		result.Should().NotBeNull();
		result.Should().HaveCount(1);
		result["p1"].Should().BeNull();
	}

}
