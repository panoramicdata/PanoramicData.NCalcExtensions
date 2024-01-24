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

		var result = expression.Evaluate();
		var dictionary = (Dictionary<string, object?>)result;
		dictionary.Should().HaveCount(2);
		dictionary["p1"].Should().Be(true);
		dictionary["p2"].Should().Be(false);
	}

	[Fact]
	public void Dictionary_KeyParameterIsNull_ThrowsFormatException()
	{
		var expression = new ExtendedExpression("dictionary(null, true, 'p2', false)");

		var action = expression.Evaluate;
		action.Should().Throw<FormatException>();
	}

	[Fact]
	public void Dictionary_NullValueParameter_GeneratesDictionaryContainingNullValue()
	{
		var expression = new ExtendedExpression("dictionary('p1', null)");

		var result = expression.Evaluate();
		var dictionary = (Dictionary<string, object>)result;
		dictionary.Should().ContainSingle();
		dictionary["p1"].Should().BeNull();
	}

}
