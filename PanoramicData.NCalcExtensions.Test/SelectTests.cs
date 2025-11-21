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
}
